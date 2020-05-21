using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PerlinNoise
{
    public static float[,] GenerateMap(int width,
        int height,
        float scale,
        int octaves,
        float persistence,
        float lacunarity,
        int seed,
        Vector2 offset
        )
    {
        float[,] map = new float[width, height];

        System.Random randomGenerator = new System.Random(seed);
        Vector2[] octOffsets = new Vector2[octaves];

        float maxGlobalHeight = 0;
        float ampl = 1;

        for (int i = 0; i< octaves; i++)
        {
            float offX = randomGenerator.Next(-100000, 100000) + offset.x;
            float offY = randomGenerator.Next(-100000, 100000) - offset.y;
            octOffsets[i] = new Vector2(offX, offY);

            maxGlobalHeight += ampl;
            ampl *= persistence;
        }

        for(int y = 0; y<height; y++)
        {
            for(int x = 0; x<width; x++)
            {
                ampl = 1;
                float freq = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++)
                {
                    float sampX = (x - width + octOffsets[i].x) / scale * freq;
                    float sampY = (y - height + octOffsets[i].y) / scale * freq;

                    float perlin = Mathf.PerlinNoise(sampX, sampY) * 2  - 1; //change range from [0,1] to [-1, 1]
                    map[x, y] = perlin;

                    noiseHeight += perlin * ampl;
                    ampl *= persistence;
                    freq *= lacunarity; 
                }

                map[x, y] = noiseHeight;
            }
        }

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float normHeight = (map[x, y] + 1) / (maxGlobalHeight / 0.9f);
                map[x, y] = Mathf.Clamp(normHeight, 0, 1);
            }
        }

        return map;
    }
}
