using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GenerateMesh
{
    public static MeshInfo GenerateMeshInfo(float[,] heightMap, float heightFactor, AnimationCurve heightCur, int detailLvl)
    {
        AnimationCurve hCurve = new AnimationCurve(heightCur.keys);

        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        float tlX = (width - 1) / -2f; //top left x
        float tlZ = (height - 1) / 2f;

        //Debug.Log(detailLvl.ToString());

        int simplifyFactor = (detailLvl == 0) ? 1 : detailLvl * 2;
        int vertInLine = (width - 1) / simplifyFactor + 1;

        MeshInfo mesh = new MeshInfo(vertInLine, vertInLine);
        int vertIndx = 0;

        for(int y = 0; y < height; y+=simplifyFactor)
        {
            for(int x = 0; x < width; x+=simplifyFactor)
            {
                mesh.vertices[vertIndx] = new Vector3(tlX + x, hCurve.Evaluate(heightMap[x, y]) * heightFactor, tlZ - y);
                mesh.uv[vertIndx] = new Vector2(x / (float)width, y / (float)height);

                if(x < width - 1 && y < height - 1)
                {
                    mesh.AddTriangle(vertIndx, vertIndx + vertInLine + 1, vertIndx + vertInLine);
                    mesh.AddTriangle(vertIndx + vertInLine + 1, vertIndx, vertIndx + 1);
                }

                vertIndx++;
            }
        }
        return mesh;
    }
}
