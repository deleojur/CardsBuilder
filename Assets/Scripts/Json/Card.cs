using System;
using System.Collections;

namespace Json
{
    [Serializable]
    public class Cards
    {
        public Card[] cards;
    }

    [Serializable]
    public class Card
    {
        public string title;
        public string description;
        public string background;
        public string foreground;        
        public string color;
        public string[] cost;
    }
}