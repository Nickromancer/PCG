using System;
using UnityEngine;
using UnityEngine.UI;

using Random = UnityEngine.Random;

public class VoronoiDiagram : MonoBehaviour
{
    [SerializeField] private Color[] possibleColors;
    [SerializeField] private Transform[] transforms;
    [SerializeField] private bool showPoints = false;
    [SerializeField] private int pointSize = 2;
    [SerializeField] private int gridSize = 10;

    private int imgSize;
    private RawImage img;
    private int pixelsPerCell;
    private Vector2Int[,] pointsPositions;
    private Color[,] colors;

    void Awake()
    {
        img = GetComponent<RawImage>();
        imgSize = Mathf.RoundToInt(img.GetComponent<RectTransform>().sizeDelta.x);
        pixelsPerCell = imgSize / gridSize;
    }

    void Start()
    {
        GeneratePoints();
        GenerateDiagram();
    }

    private void GeneratePointsFromTransforms()
    {
        int counter = 0;
        pointsPositions = new Vector2Int[gridSize, gridSize];
        //colors = new Color[gridSize, gridSize];
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                pointsPositions[i, j] = new Vector2Int((int)transforms[counter].position.x, (int)transforms[counter].position.y);
                //colors[i, j] = possibleColors[Random.Range(0, possibleColors.Length)];
                counter++;
            }
        }
    }

    void Update()
    {
        //MovePoints();
        //GeneratePointsFromTransforms();
        GenerateDiagram();
    }

    private void GeneratePoints()
    {
        pointsPositions = new Vector2Int[gridSize, gridSize];
        colors = new Color[gridSize, gridSize];
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                pointsPositions[i, j] = new Vector2Int(
                    i * pixelsPerCell + Random.Range(0, pixelsPerCell),
                    j * pixelsPerCell + Random.Range(0, pixelsPerCell));
                colors[i, j] = possibleColors[Random.Range(0, possibleColors.Length)];
            }
        }
    }

    private void MovePoints()
    {
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                int random = Random.Range(1, 3);

                pointsPositions[i, j] = new Vector2Int(
                    pointsPositions[i, j].x + (int)Math.Round(Math.Sin(Time.time)) * random,
                    pointsPositions[i, j].y + (int)Math.Round(Math.Cos(Time.time)) * random);
            }
        }
    }

    private void GenerateDiagram()
    {
        Texture2D texture = new Texture2D(imgSize, imgSize, TextureFormat.RGBA32, false);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        img.color = Color.white;

        for (int i = 0; i < imgSize; i++)
        {
            for (int j = 0; j < imgSize; j++)
            {
                texture.SetPixel(i, j, Color.white);
            }
        }

        for (int i = 0; i < imgSize; i++)
        {
            for (int j = 0; j < imgSize; j++)
            {
                int gridX = i / pixelsPerCell;
                int gridY = j / pixelsPerCell;

                float nearestDistance = Mathf.Infinity;
                Vector2Int nearestPoint = new Vector2Int();

                for (int a = -1; a < 2; a++)
                {
                    for (int b = -1; b < 2; b++)
                    {
                        int x = gridX + a;
                        int y = gridY + b;
                        if (x < 0 || y < 0 || x >= gridSize || y >= gridSize) continue;

                        float distance = Vector2Int.Distance(new Vector2Int(i, j), pointsPositions[x, y]);
                        if (distance < nearestDistance)
                        {
                            nearestDistance = distance;
                            nearestPoint = new Vector2Int(x, y);
                        }
                    }
                }
                texture.SetPixel(i, j, colors[nearestPoint.x, nearestPoint.y]);
            }
        }

        if (showPoints)
        {
            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    int px = pointsPositions[i, j].x;
                    int py = pointsPositions[i, j].y;
                    if (px >= 0 && py >= 0 && px < imgSize && py < imgSize)
                    {
                        for (int a = -1; a < pointSize; a++)
                        {
                            for (int b = -1; b < pointSize; b++)
                            {
                                texture.SetPixel(px + a, py + b, Color.black);
                            }
                        }
                    }
                }
            }
        }
        texture.Apply();
        img.texture = texture;

        System.IO.File.WriteAllBytes("VoronoiDiagram.png", texture.EncodeToPNG());
    }
}