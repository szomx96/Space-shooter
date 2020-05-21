//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public static class TerrainTypes
//{
//    public  enum TerrainType { Mountains, Plane, Valley };
    

//    public static TerrainType RandomTerrainType()
//    {
//        float i = Random.Range(0.0f, 3.0f); //tu jakis powazny warunek trzeba


//        if (i >= 0 && i < 1) {
//            return TerrainType.Mountains;
//        } else if (i >= 1 && i < 2) {
//            return TerrainType.Plane;
//        } else if (i >= 2 && i < 3) {
//            return TerrainType.Valley;
//        }

//        return TerrainType.Plane;
//    }

//    public static NoiseInfo generateNoise(TerrainType terrainType)
//    {
//        if(terrainType == TerrainType.Mountains)
//        {
//            NoiseValues noiseVal = new NoiseValues()
//            NoiseInfo noiseInfo = (Random.Range(NoiseValues.minS))
//        }

//        return new NoiseInfo();
//    }

//    class NoiseValues
//    {
//        public float minScale;
//        public float maxScale;
//        public float minOctaves;
//        public float maxOctaves;
//        public float minPersistance;
//        public float maxPersistance;
//        public float minLacunarity;
//        public float maxLacunarity;
//        public float minSeed;
//        public float maxSeed;
//        public float minOffset;
//        public float maxOffset;
//    }

//}
