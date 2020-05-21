using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


[CreateAssetMenu()]
public class TextureInfo : ScriptableObject
{
    const int textureSize = 2048;
    const TextureFormat textureFormat = TextureFormat.RGB565;

    public TerrainTypes[] terrainTypes;

    public void ApplyToMat(Material mat)
    {
        
        mat.SetInt("terrainsCount", terrainTypes.Length);
        //mat.SetColorArray("colors", terrainTypes.Select(x => x.color).ToArray());
        mat.SetFloatArray("heights", terrainTypes.Select(x => x.height).ToArray());
        mat.SetFloatArray("blends", terrainTypes.Select(x => x.blendStrength).ToArray());
        //mat.SetFloatArray("colorStrengths", terrainTypes.Select(x => x.colorStrength).ToArray());
        mat.SetFloatArray("scales", terrainTypes.Select(x => x.scale).ToArray());

        Texture2DArray textArr = generateTextures(terrainTypes.Select(x => x.text).ToArray());
        Texture2DArray normalMapArr = generateTextures(terrainTypes.Select(x => x.normalMap).ToArray());

        mat.SetTexture("textures", textArr);
        mat.SetTexture("normalMaps", normalMapArr);
    }

    Texture2DArray generateTextures(Texture2D[] textures)
    {
        int texturesLen = textures.Length;

        Texture2DArray textArr = new Texture2DArray(textureSize, textureSize, texturesLen, textureFormat, true);

        for(int i = 0; i < texturesLen; i++)
        {
            textArr.SetPixels(textures[i].GetPixels(), i);
        }
        textArr.Apply();

        return textArr;
    }

    public void ChangeHeights(Material mat, float minH, float maxH)
    {
        mat.SetFloat("minHeight", minH);
        mat.SetFloat("maxHeight", maxH);
    }

    [System.Serializable]
    public class TerrainTypes
    {
        public Texture2D text;
        public Texture2D normalMap;
        
        public float scale;
        //public Color color;
        //[Range(0,1)]
        //public float colorStrength;
        [Range(0, 1)]
        public float blendStrength;
        [Range(0, 1)]
        public float height;        
    }
}
