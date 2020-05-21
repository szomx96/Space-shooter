using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelOfDetailMesh : InfiniteTerrain
{
    public Mesh mesh;
    public bool requestedMesh;
    public bool receivedMesh;
    int lod;
    System.Action updateCallback;

    public LevelOfDetailMesh(int lod, System.Action callback)
    {
        this.lod = lod;
        this.updateCallback = callback;
    }

    void MeshInfoReceived(MeshInfo meshInfo)
    {
        mesh = meshInfo.MakeMesh();
        receivedMesh = true;

        updateCallback();
    }

    public void RequestMesh(float[,] heightMap)
    {
        requestedMesh = true;
        //int random = Random.Range(0, 2);
        //Debug.Log(random.ToString());
        mapGenerator.RequestMeshInfo(heightMap, lod, /*random, */MeshInfoReceived);
    }
}