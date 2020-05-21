Shader "Custom/TerrainShader"
{
	Properties
	{
        _MainTex("Albedo (RGB)", 2D) = "white" {}
		[NoScaleOffset] _BumpMap("Normal Map", 2D) = "bump" {}
	}
	SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadowszz

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		#include "UnityStandardUtils.cginc"

		// flip UVs horizontally to correct for back side projection
		#define TRIPLANAR_CORRECT_PROJECTED_U

		half3 blend_rnm(half3 n1, half3 n2)
		{
			n1.z += 1;
			n2.xy = -n2.xy;

			return n1 * dot(n1, n2) / n1.z - n2;
		}


        sampler2D _MainTex;
        float4 _MainTex_ST;

        sampler2D _BumpMap;

		const static int maxColors = 8;
		const static float e = 1E-3;
		int terrainsCount;
		float heights[maxColors];
		float minHeight;
		float maxHeight;
		float blends[maxColors];

		float scales[maxColors];

		UNITY_DECLARE_TEX2DARRAY(textures);

        struct Input
        {
			float3 worldPos;
			float3 worldNormal; 
			INTERNAL_DATA
        };

		float3 WorldToTangentNormalVector(Input IN, float3 normal) {
			float3 t2w0 = WorldNormalVector(IN, float3(1, 0, 0));
			float3 t2w1 = WorldNormalVector(IN, float3(0, 1, 0));
			float3 t2w2 = WorldNormalVector(IN, float3(0, 0, 1));
			float3x3 t2w = float3x3(t2w0, t2w1, t2w2);
			return normalize(mul(t2w, normal));
		}

        //// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        //// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        //// #pragma instancing_options assumeuniformscaling
        //UNITY_INSTANCING_BUFFER_START(Props)
        //    // put more per-instance properties here
        //UNITY_INSTANCING_BUFFER_END(Props)

		float inverseLerp(float minVal, float maxVal, float currVal) {
			
			return saturate((currVal - minVal) / (maxVal - minVal));
		}

		float3 applyText(float3 worldPos, float testScale, float3 axes, int index) {
			float3 scWorldPos = worldPos / testScale;

			float3 projX = UNITY_SAMPLE_TEX2DARRAY(textures, float3(scWorldPos.y, scWorldPos.z, index)) * axes.x;
			float3 projY = UNITY_SAMPLE_TEX2DARRAY(textures, float3(scWorldPos.x, scWorldPos.z, index)) * axes.y;
			float3 projZ = UNITY_SAMPLE_TEX2DARRAY(textures, float3(scWorldPos.x, scWorldPos.y, index)) * axes.z;

			return projX + projY + projZ;
		}

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // work around bug where IN.worldNormal is always (0,0,0)!
            IN.worldNormal = WorldNormalVector(IN, float3(0, 0, 1));

            // calculate triplanar blend
            half3 triblend = saturate(pow(IN.worldNormal, 4));
            triblend /= max(dot(triblend, half3(1, 1, 1)), 0.0001);

            // calculate triplanar uvs
            // applying texture scale and offset values ala TRANSFORM_TEX macro
            float2 uvX = IN.worldPos.zy * _MainTex_ST.xy + _MainTex_ST.zy;
            float2 uvY = IN.worldPos.xz * _MainTex_ST.xy + _MainTex_ST.zy;
            float2 uvZ = IN.worldPos.xy * _MainTex_ST.xy + _MainTex_ST.zy;

            // offset UVs to prevent obvious mirroring
            #if defined(TRIPLANAR_UV_OFFSET)
                uvY += 0.33;
                uvZ += 0.67;
            #endif

            // minor optimization of sign(). prevents return value of 0
            half3 axisSign = IN.worldNormal < 0 ? -1 : 1;

            // flip UVs horizontally to correct for back side projection
            #if defined(TRIPLANAR_CORRECT_PROJECTED_U)
                uvX.x *= axisSign.x;
                uvY.x *= axisSign.y;
                uvZ.x *= -axisSign.z;
            #endif

            //// albedo textures
            //fixed4 colX = tex2D(_MainTex, uvX);
            //fixed4 colY = tex2D(_MainTex, uvY);
            //fixed4 colZ = tex2D(_MainTex, uvZ);
            //fixed4 col = colX * triblend.x + colY * triblend.y + colZ * triblend.z;

            //// occlusion textures
            //half occX = tex2D(_OcclusionMap, uvX).g;
            //half occY = tex2D(_OcclusionMap, uvY).g;
            //half occZ = tex2D(_OcclusionMap, uvZ).g;
            //half occ = LerpOneTo(occX * triblend.x + occY * triblend.y + occZ * triblend.z, _OcclusionStrength);

            // tangent space normal maps
            half3 tnormalX = UnpackNormal(tex2D(_BumpMap, uvX));
            half3 tnormalY = UnpackNormal(tex2D(_BumpMap, uvY));
            half3 tnormalZ = UnpackNormal(tex2D(_BumpMap, uvZ));

            // flip normal maps' x axis to account for flipped UVs
            #if defined(TRIPLANAR_CORRECT_PROJECTED_U)
                tnormalX.x *= axisSign.x;
                tnormalY.x *= axisSign.y;
                tnormalZ.x *= -axisSign.z;
            #endif

            half3 absVertNormal = abs(IN.worldNormal);

            // swizzle world normals to match tangent space and apply reoriented normal mapping blend
            tnormalX = blend_rnm(half3(IN.worldNormal.zy, absVertNormal.x), tnormalX);
            tnormalY = blend_rnm(half3(IN.worldNormal.xz, absVertNormal.y), tnormalY);
            tnormalZ = blend_rnm(half3(IN.worldNormal.xy, absVertNormal.z), tnormalZ);

            // apply world space sign to tangent space Z
            tnormalX.z *= axisSign.x;
            tnormalY.z *= axisSign.y;
            tnormalZ.z *= axisSign.z;

            // sizzle tangent normals to match world normal and blend together
            half3 worldNormal = normalize(
                tnormalX.zyx * triblend.x +
                tnormalY.xzy * triblend.y +
                tnormalZ.xyz * triblend.z
            );

            // set surface ouput properties
            //o.Albedo = col.rgb;
            //o.Metallic = _Metallic;
            //o.Smoothness = _Glossiness;
            //o.Occlusion = occ;

            // convert world space normals into tangent normals
            o.Normal = WorldToTangentNormalVector(IN, worldNormal);

			float height = inverseLerp(minHeight, maxHeight, IN.worldPos.y);
			float3 absWorldNormal = abs(IN.worldNormal);
			absWorldNormal /= absWorldNormal.x + absWorldNormal.y + absWorldNormal.z; //to avoid value greater than 1

			for (int i = 0; i < terrainsCount; i++) {
				float drawStr = inverseLerp(-blends[i] / 2 - e, blends[i] / 2, height - heights[i]);

				float3 textMat = applyText(IN.worldPos, scales[i], absWorldNormal, i);
				o.Albedo = o.Albedo * (1 - drawStr) + textMat * drawStr;
			}
			
           
        }
        ENDCG
    }
    FallBack "Diffuse"
}
