using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshInfo
{
    public Vector3[] vertices;
    public int[] triangles;
    public Vector2[] uv;

    int triangleIndx;

    public MeshInfo(int width, int height)
    {
        vertices = new Vector3[width * height];
        triangles = new int[(width - 1) * (height - 1) * 6];
        uv = new Vector2[width * height];
    }

    public void AddTriangle(int a, int b, int c)
    {
        triangles[triangleIndx] = a;
        triangles[triangleIndx + 1] = b;
        triangles[triangleIndx + 2] = c;

        triangleIndx += 3;
    }

    public Mesh MakeMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;        
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        return mesh;
    }
}
