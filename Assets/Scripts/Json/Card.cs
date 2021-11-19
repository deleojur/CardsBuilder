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
        public string foreground;
        public string type;
        public string[] guilds;
        public string description;
        public string flavor;
        public int loyalty;
        public int descriptionFontSize;
        public Resource[] cost;
        public Resource[] prerequisites;
    }

    [Serializable]
    public class Resource
    {
        public string type;
        public int amount;
    }


    public class ResourceSort : IComparer
    {
        private string[] resourceTypesSort = { "gold", "grain", "sheep", "mutton", "gems", "barter", "shadow", "battle", "science", "celestial" };

        public int Compare(object x, object y)
        {
            Resource res1 = x as Resource;
            Resource res2 = y as Resource;

            int res1Idx = Array.IndexOf(resourceTypesSort, res1.type);
            int res2Idx = Array.IndexOf(resourceTypesSort, res2.type);

            return res1Idx < res2Idx ? -1 : 1;
        }
    }

    public class PrerequisitesSort : IComparer
    {
        private string[] prerequisitesTypesSort = { "barter", "shadow", "battle", "science", "celestial" };

        public int Compare(object x, object y)
        {
            Resource res1 = x as Resource;
            Resource res2 = y as Resource;

            int res1Idx = Array.IndexOf(prerequisitesTypesSort, res1.type);
            int res2Idx = Array.IndexOf(prerequisitesTypesSort, res2.type);

            return res1Idx < res2Idx ? -1 : 1;
        }
    }
}