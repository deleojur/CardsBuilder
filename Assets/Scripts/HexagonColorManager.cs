using Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class HexagonColorManager
{
    private string[] colors = { "cool blue", "pastel red", "lawn green", "grape purple" };
    private int colorIndex = 0;

    public Hexagons GenerateHexagonsWithColors(string json)
    {
        Hexagons hexagons = JsonUtility.FromJson<Hexagons>(json);
        foreach (HexagonCategory category in hexagons.categories)
        {
            CreateHexagonCategory(category);
        }
        return hexagons;
    }

    private void CreateHexagonCategory(HexagonCategory hexagonCategory)
    {
        foreach (Hexagon hexagon in hexagonCategory.hexagons)
        {
            hexagon.category = hexagonCategory.name;
            int score = hexagonCategory.score - hexagon.score;

            HexagonEdge[] hexagonEdges = new HexagonEdge[6];
            int i = 0;

            for (int j = 0; j < 6; j++)
            {
                hexagonEdges[j] = new HexagonEdge { colors = new string[0] };
            }

            while (score > 0)
            {
                int index = i++ % 6;
                List<string> edgeColors = hexagonEdges[index].colors.ToList();
                float chance = Random.Range(0f, 1f);

                if (chance > 0.35f)
                {
                    string color = Color;

                    while (edgeColors.Contains(color) || edgeColors.Count == 3)
                    {
                        color = Color;
                    }

                    edgeColors.Add(color);
                    score--;
                }
                hexagonEdges[index] = new HexagonEdge { colors = edgeColors.ToArray() };
            }
            hexagon.edges = hexagonEdges;
        }
    }

    private string Color
    {
        get
        {
            int value = colorIndex++ % colors.Length;
            return colors[value];
        }
    }
}
