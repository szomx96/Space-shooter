using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainPiece : InfiniteTerrain
{
    Vector2 pos;
    GameObject meshObj;
    GameObject subMeshObj;
    Bounds bounds;

    MeshRenderer meshR;
    MeshRenderer meshRe;
    MeshFilter meshF;
    MeshFilter meshF2;
    MeshCollider meshColl;
    MeshCollider meshColl2;

    LevelOfDetailInfo[] lods;
    LevelOfDetailMesh[] lodMeshes;
    LevelOfDetailMesh collisionLevelOfDetail;

    float[,] heightMap;

    bool mapInfoReceived;

    int prevLODIndex = -1;

    public TerrainPiece(Vector2 coord, int size, LevelOfDetailInfo[] lods, Transform parent, Material waterMaterial, Material mapMaterial)
    {
        this.lods = lods;

        pos = coord * size;
        bounds = new Bounds(pos, Vector2.one * size);
        Vector3 position3D = new Vector3(pos.x, 0, pos.y);
        Vector3 waterPos = new Vector3(pos.x, position3D.y+waterHeight, pos.y);

        meshObj = new GameObject("Terrain Piece");
        meshR = meshObj.AddComponent<MeshRenderer>();
        meshF = meshObj.AddComponent<MeshFilter>();
        meshColl = meshObj.AddComponent<MeshCollider>();
        meshR.material = mapMaterial;
        meshObj.transform.position = position3D * mapGenerator.terrainInfo.scale;
        meshObj.transform.parent = parent;
        meshObj.transform.localScale = Vector3.one * mapGenerator.terrainInfo.scale;

        GameObject waterPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        waterPlane.GetComponent<MeshRenderer>().material = waterMaterial;
        waterPlane.transform.position = waterPos * mapGenerator.terrainInfo.scale;
        waterPlane.transform.parent = parent;
        waterPlane.transform.localScale = Vector3.one * 60;

        SetVisible(false);

        lodMeshes = new LevelOfDetailMesh[lods.Length];

        for (int i = 0; i < lods.Length; i++)
        {
            lodMeshes[i] = new LevelOfDetailMesh(lods[i].lod, UpdateTerrainPiece);
            if (lods[i].makeCollider)
            {
                collisionLevelOfDetail = lodMeshes[i];
            }
        }

        mapGenerator.RequestMapInfo(pos, OnMapInfoReceived);

    }

    public void UpdateTerrainPiece()
    {
        if (mapInfoReceived)
        {
            float playerDistFromEdge = Mathf.Sqrt(bounds.SqrDistance(playerPos));
            bool visible = playerDistFromEdge <= viewDistance;

            if (visible)
            {
                int lodIndex = 0;

                for (int i = 0; i < lods.Length - 1; i++)
                {
                    if (playerDistFromEdge > lods[i].distance)
                    {
                        lodIndex = i + 1;
                    }
                    else
                    {
                        break;
                    }
                }

                if (lodIndex != prevLODIndex)
                {
                    LevelOfDetailMesh lodMesh = lodMeshes[lodIndex];

                    if (lodMesh.receivedMesh)
                    {
                        prevLODIndex = lodIndex;
                        meshF.mesh = lodMesh.mesh;
                    }
                    else if (!lodMesh.requestedMesh)
                    {
                        lodMesh.RequestMesh(heightMap);
                    }
                }

                if (lodIndex == 0)
                {

                    if (collisionLevelOfDetail.receivedMesh)
                    {
                        meshColl.sharedMesh = collisionLevelOfDetail.mesh;
                    }
                    else if (!collisionLevelOfDetail.requestedMesh)
                    {
                        collisionLevelOfDetail.RequestMesh(heightMap);
                    }
                }

                terrainPiecesVisibleLastUpdate.Add(this);
            }
            SetVisible(visible);
        }
    }

    void OnMapInfoReceived(float[,] heightMap)
    {
        this.heightMap = heightMap;
        mapInfoReceived = true;

        UpdateTerrainPiece();
    }

    public void SetVisible(bool visible)
    {
        meshObj.SetActive(visible);
    }

    public bool IsVisible()
    {
        return meshObj.activeSelf;
    }

}

[System.Serializable]
public struct LevelOfDetailInfo
{
    public int lod;
    public float distance;
    public bool makeCollider;
}