using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PerlinNoiseDiagram : MonoBehaviour
{
    public int width = 256;
    public int height = 256;
    public int scale = 10;
    Renderer renderer;

    void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    void OnValidate()
    {
        renderer = GetComponent<Renderer>();
        renderer.sharedMaterial.mainTexture = GenerateTexture();
    }

    private Texture GenerateTexture()
    {
        Texture2D texture = new Texture2D(width, height);

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Color color = PerlinNoiseColor(i, j);
                texture.SetPixel(i, j, color);
            }
        }
        texture.Apply();
        return texture;
    }

    private Color PerlinNoiseColor(int i, int j)
    {
        float xCord = (float)i / width * scale;
        float yCord = (float)j / height * scale;
        float sample = Mathf.PerlinNoise(xCord, yCord);
        return new Color(sample, sample, sample);
    }
}
