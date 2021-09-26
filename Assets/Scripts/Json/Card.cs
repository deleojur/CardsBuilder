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
        public string background;
        public string type;
        public string[] guilds;
        public string description;
        public string flavor;
        public int tier;
        public int descriptionFontSize;
        public Resource[] cost;        
    }

    [Serializable]
    public class Resource
    {
        public string type;
        public int amount;
    }  

    public class ResourceSort : IComparer
    {
        private string[] resourceTypesSort = { "gold", "food", "sheep", "wool", "gems" };

        public int Compare(object x, object y)
        {
            Resource res1 = x as Resource;
            Resource res2 = y as Resource;

            int res1Idx = Array.IndexOf(resourceTypesSort, res1.type);
            int res2Idx = Array.IndexOf(resourceTypesSort, res2.type);

            return res1Idx < res2Idx ? -1 : 1;
        }
    }
}