using System;


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
        public string backgroundUrl;
        public string typeUrl;
        public string[] faction;
        public string description;
        public int tier;
        public Resource[] cost;
    }

    [Serializable]
    public class Resource
    {
        public string typeUrl;
        public int amount;
    }
}