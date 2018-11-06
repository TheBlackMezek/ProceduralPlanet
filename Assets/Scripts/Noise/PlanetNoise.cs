using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CoherentNoise.Generation;

public class PlanetNoise {

    private ValueNoise noiseClass;

    private int octaves = 1;
    private float frequency = 1f;
    private float lacunarity = 2f;
    private float amplitude = 1f;
    private float persistence = 0.5f;

    private int seed = 0;



    public PlanetNoise()
    {
        noiseClass = new ValueNoise(seed);
    }

    public PlanetNoise(int octaves, float frequency, float lacunarity, float amplitude, float persistence, int seed)
    {
        if (octaves < 1)
            octaves = 1;

        this.octaves = octaves;
        this.frequency = frequency;
        this.lacunarity = lacunarity;
        this.amplitude = amplitude;
        this.persistence = persistence;

        this.seed = seed;

        noiseClass = new ValueNoise(seed);
    }

    public float GetValue(float x, float y, float z)
    {
        float localFreq = frequency;
        float localAmp = amplitude;

        float maxValue = 0f;
        float ret = 0f;

        for(int i = 0; i < octaves; ++i)
        {
            ret += noiseClass.GetValue(x * localFreq, y * localFreq, z * localFreq) * localAmp;

            maxValue += localAmp;

            localFreq *= lacunarity;
            localAmp *= persistence;
        }

        return ret / maxValue;
    }

    public float GetValue(Vector3 pos) { return GetValue(pos.x, pos.y, pos.z); }

}
