using System;
using System.Collections;

namespace Json
{
    [Serializable]
    public class Hexagons
    {
        public HexagonCategory[] categories;
    }

    [Serializable]
    public class HexagonCategory
    {
        public string name;
        public int score;
        public Hexagon[] hexagons;
    }

    [Serializable]
    public class Hexagon
    {
        public string title;
        public string category;
        public string type;
        public string logo;
        public string logo2;
        public int amount;
        public int amount2;
        public string toLogo;
        public string toLogoBackgroundColor;
        public int toAmount;
        public int score;
        public string background;
        public HexagonEdge[] edges;
    }

    [Serializable]
    public class HexagonEdge
    {
        public string[] colors;
    }
}