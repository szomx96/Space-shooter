using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class TerrainInfo : ScriptableObject
{
    public float scale = 0.2f;
    public float meshHeightFactor;
    public AnimationCurve meshHeightCur;

    public float minH
    {
        get
        {
            return scale * meshHeightFactor * meshHeightCur.Evaluate(0);
        }
    }
        public float maxH
    {
        get
        {
            return scale * meshHeightFactor * meshHeightCur.Evaluate(1);
        }
    }
}
