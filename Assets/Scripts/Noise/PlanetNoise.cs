using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CoherentNoise.Generation;
using Unity.Entities;

public class PlanetNoise {

    private ValueNoise noiseClass;

    [System.Serializable]
    public struct PlanetNoiseSettings
    {
        public int octaves;
        public float frequency;
        public float lacunarity;
        public float amplitude;
        public float persistence;
        
        public AnimationCurve heightCurve;

        public PlanetColorLayer[] colorLayers;

        public int seed;
    }

    [System.Serializable]
    public struct PlanetColorLayer
    {
        public float heightThreshold;
        public Color vertexColor;
    }

    private PlanetNoiseSettings settings = new PlanetNoiseSettings();



    public PlanetNoise()
    {
        settings.octaves = 1;
        settings.frequency = 1f;
        settings.lacunarity = 2f;
        settings.amplitude = 1f;
        settings.persistence = 0.5f;

        settings.seed = 0;
        noiseClass = new ValueNoise(settings.seed);
    }

    public PlanetNoise(PlanetNoiseSettings settings)
    {
        if (settings.octaves < 1)
            settings.octaves = 1;

        this.settings = settings;
        
        noiseClass = new ValueNoise(settings.seed);
    }

    public float GetValue(float x, float y, float z, int level = 0)
    {
        float ret = GetNoiseValue(x, y, z, level);
        ret = settings.heightCurve.Evaluate(ret);
        
        return ret;
    }

    public float GetNoiseValue(float x, float y, float z, int level = 0)
    {
        float localFreq = settings.frequency;
        float localAmp = settings.amplitude;

        float maxValue = 0f;
        float ret = 0f;

        for(int i = 0; i < settings.octaves + level * 1; ++i)
        {
            ret += noiseClass.GetValue(x * localFreq, y * localFreq, z * localFreq) * localAmp;

            maxValue += localAmp;

            localFreq *= settings.lacunarity;
            localAmp *= settings.persistence;
        }

        return ret / maxValue;
    }

    public float GetValue(Vector3 pos, int level = 0) { return GetValue(pos.x, pos.y, pos.z, level); }



    public int ColorLayersLength() { return settings.colorLayers.Length; }
    public PlanetColorLayer ColorLayer(int idx) { return settings.colorLayers[idx]; }

}
