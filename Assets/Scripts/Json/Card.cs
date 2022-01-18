using System;
using System.Collections;

namespace Json
{
    [Serializable]
    public class Hexagons
    {
        public Hexagon[] hexagons;
    }

    [Serializable]
    public class Hexagon
    {
        public string title;
        public string type;
        public string logo;
        public string amount;
        public string toLogo;
        public string toAmount;
        public HexagonEdge[] edges;
    }

    [Serializable]
    public class HexagonEdge
    {
        public string[] colors;
    }
}