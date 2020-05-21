using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public class MapGenerator : MonoBehaviour
{
    public TerrainInfo terrainInfo;
    public NoiseInfo noiseInfo;
    public TextureInfo textureInfo;

    //private enum TerrainType { Mountains, Plane, Valley };

    public const int mapPieceSize = 241;

    public Material terrainMat;
   // public Material terrainHeightMap;

    Queue<MapThreadInfo<float[,]>> mapInfoThreadInfoQueue = new Queue<MapThreadInfo<float[,]>>();
    Queue<MapThreadInfo<MeshInfo>> meshInfoThreadInfoQueue = new Queue<MapThreadInfo<MeshInfo>>();

    void Awake()
    {
        textureInfo.ApplyToMat(terrainMat);

        textureInfo.ChangeHeights(terrainMat, terrainInfo.minH, terrainInfo.maxH);
    }

    public void RequestMapInfo(Vector2 center, Action<float[,]> callback)
    {
        ThreadStart threadStart = delegate
        {
            MapInfoThread(center, callback);
        };

        new Thread(threadStart).Start();
    }

    void MapInfoThread(Vector2 center, Action<float[,]> callback)
    {
        float[,] heightMap = GenerateHeightmap(center);

        lock (mapInfoThreadInfoQueue)
        {
            mapInfoThreadInfoQueue.Enqueue(new MapThreadInfo<float[,]>(callback, heightMap));
        }
    }

    public void RequestMeshInfo(float[,] heightMap, int detailLevel, /*int i, */Action<MeshInfo> callback)
    {
        ThreadStart threadStart = delegate {
            MeshInfoThread(heightMap, detailLevel, callback);
        };
        new Thread(threadStart).Start();

    }

    void MeshInfoThread(float[,] heightMap, int detailLevel, /*int i, */Action<MeshInfo> callback)
    {
        MeshInfo meshInfo = GenerateMesh.GenerateMeshInfo(heightMap, terrainInfo.meshHeightFactor, terrainInfo.meshHeightCur, detailLevel);
        lock (meshInfoThreadInfoQueue)
        {
            meshInfoThreadInfoQueue.Enqueue(new MapThreadInfo<MeshInfo>(callback, meshInfo));
        }
    }

    void Update()
    {
        if (mapInfoThreadInfoQueue.Count > 0)
        {
            for (int i = 0; i < mapInfoThreadInfoQueue.Count; i++)
            {
                MapThreadInfo< float[,]> threadInfo = mapInfoThreadInfoQueue.Dequeue();
                threadInfo.callback(threadInfo.parameter);
            }
        }

        if (meshInfoThreadInfoQueue.Count > 0)
        {
            for(int i = 0; i < meshInfoThreadInfoQueue.Count; i++)
            {
                MapThreadInfo<MeshInfo> threadInfo = meshInfoThreadInfoQueue.Dequeue();
                threadInfo.callback(threadInfo.parameter);
            }
        }
    }


    float[,] GenerateHeightmap(Vector2 center)
    {
       //TerrainTypes.RandomTerrainType();

        float[,] heightMap = PerlinNoise.GenerateMap(mapPieceSize, mapPieceSize, noiseInfo.scale, noiseInfo.octaves, noiseInfo.persistance, noiseInfo.lacunarity, noiseInfo.seed, center + noiseInfo.offset);

        return heightMap;
    }

    struct MapThreadInfo<T>
    {
        public readonly Action<T> callback;
        public readonly T parameter;

        public MapThreadInfo(Action<T> callback, T parameter)
        {
            this.callback = callback;
            this.parameter = parameter;
        }
    }
}