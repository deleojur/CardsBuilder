using Json;
using System.Collections.Generic;
using UnityEngine;

public class HexagonColorManager
{
    private string[] colors = { "cool blue", "pastel red", "lawn green", "grape purple" };

    public Hexagons GenerateHexagonsWithColors(string json)
    {
        Hexagons hexagons = JsonUtility.FromJson<Hexagons>(json);
        List<Hexagon> newHexes = new List<Hexagon>();

        foreach (Hexagon hexagon in hexagons.hexagons)
        { 
            
        }
        return hexagons;//new Hexagons { hexagons = newHexes.ToArray() };
    }
}
