using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Json
{
    public class JsonSerializer : MonoBehaviour
    {
        private Cards cards;

        [SerializeField]
        private TextAsset JSONCards;

        [Header("Color images")]
        [SerializeField]
        private Image cardFace;

        [SerializeField]
        private Image titleBackground;

        [SerializeField]
        private Image border;

        [SerializeField]
        private Image leftResourceBackground;

        [SerializeField]
        private Image rightResourceBackground;
        
        [SerializeField]
        private Image background;

        [SerializeField]
        private Image foreground;

        [SerializeField]
        private Image descriptionBackground;

        [SerializeField]
        private TextMeshProUGUI title;

        [SerializeField]
        private TextMeshProUGUI description;

        [SerializeField]
        private Image logoBackground;

        [SerializeField]
        private Image logo;        
        
        [SerializeField]
        private ScreenshotWriter screenshotWriter;

        [SerializeField]
        private ResourcePrefab[] resources;

        [SerializeField]
        private NeighboringStructure northNeighbor;

        [SerializeField]
        private NeighboringStructure eastNeighbor;

        [SerializeField]
        private NeighboringStructure southNeighbor;

        [SerializeField]
        private NeighboringStructure westNeighbor;

        private string[] guildnames = new string[6] {"Blue", "Green", "Purple", "Red", "Tan", "Yellow"};

        private void Awake()
        {
            cards = JsonUtility.FromJson<Cards>(JSONCards.text);
            StartCoroutine(YieldAndBuildCards());
        }

        private string[] GetPossibleGuildNamesForNeighboringStructures(string guildname)
        {
            List<string> possibleGuildNames = new List<string>();
            int indexOfCurrentGuild = Array.IndexOf(guildnames, guildname);

            for (int i = 0; i < 5; i++)
            {
                int index = (indexOfCurrentGuild + 1 + i) % 6;
                possibleGuildNames.Add(guildnames[index]);
            }
            return possibleGuildNames.ToArray<string>();
        }

        private enum Direction
        { 
            West    = 0,
            East    = 1,
            North   = 2,
            South   = 3
        }

        private class Neighbors
        {
            private Dictionary<Direction, string> neighboringStructures;

            public Neighbors()
            {
                neighboringStructures = new Dictionary<Direction, string>()
                {
                    { Direction.West, "" },
                    { Direction.East, "" },
                    { Direction.North, "" },
                    { Direction.South, "" }
                };
            }

            public string WestNeighbor
            {
                set { neighboringStructures[Direction.West] = value; }
                get { return neighboringStructures[Direction.West]; }
            }

            public string EastNeighbor 
            {
                set { neighboringStructures[Direction.East] = value; }
                get { return neighboringStructures[Direction.East]; }
            }

            public string NorthNeighbor
            {
                set { neighboringStructures[Direction.North] = value; }
                get { return neighboringStructures[Direction.North]; }
            }

            public string SouthNeighbor
            {
                set { neighboringStructures[Direction.South] = value; }
                get { return neighboringStructures[Direction.South]; }
            }            
        }


        private List<Neighbors> CreateNeighboringStructures(string[] guilds)
        {
            return new List<Neighbors>
            {
                new Neighbors { NorthNeighbor = guilds[0], EastNeighbor = guilds[1], SouthNeighbor = guilds[2], WestNeighbor = guilds[3] }, 
                new Neighbors { NorthNeighbor = guilds[1], EastNeighbor = guilds[2], SouthNeighbor = guilds[3], WestNeighbor = guilds[4] },
                new Neighbors { NorthNeighbor = guilds[2], EastNeighbor = guilds[3], SouthNeighbor = guilds[4], WestNeighbor = guilds[0] },
                new Neighbors { NorthNeighbor = guilds[3], EastNeighbor = guilds[4], SouthNeighbor = guilds[0], WestNeighbor = guilds[1] },
                new Neighbors { NorthNeighbor = guilds[4], EastNeighbor = guilds[0], SouthNeighbor = guilds[1], WestNeighbor = guilds[2] },

                new Neighbors { NorthNeighbor = guilds[4], EastNeighbor = guilds[3], SouthNeighbor = guilds[1], WestNeighbor = guilds[2] },
                new Neighbors { NorthNeighbor = guilds[0], EastNeighbor = guilds[4], SouthNeighbor = guilds[2], WestNeighbor = guilds[3] },
                new Neighbors { NorthNeighbor = guilds[1], EastNeighbor = guilds[0], SouthNeighbor = guilds[3], WestNeighbor = guilds[4] },
                new Neighbors { NorthNeighbor = guilds[2], EastNeighbor = guilds[1], SouthNeighbor = guilds[4], WestNeighbor = guilds[0] },
                new Neighbors { NorthNeighbor = guilds[3], EastNeighbor = guilds[2], SouthNeighbor = guilds[0], WestNeighbor = guilds[1] },

                new Neighbors { NorthNeighbor = guilds[3], EastNeighbor = guilds[1], SouthNeighbor = guilds[0], WestNeighbor = guilds[4] },
                new Neighbors { NorthNeighbor = guilds[0], EastNeighbor = guilds[3], SouthNeighbor = guilds[2], WestNeighbor = guilds[1] },
                new Neighbors { NorthNeighbor = guilds[2], EastNeighbor = guilds[0], SouthNeighbor = guilds[4], WestNeighbor = guilds[3] },
                new Neighbors { NorthNeighbor = guilds[4], EastNeighbor = guilds[2], SouthNeighbor = guilds[1], WestNeighbor = guilds[0] },
                new Neighbors { NorthNeighbor = guilds[1], EastNeighbor = guilds[4], SouthNeighbor = guilds[3], WestNeighbor = guilds[2] },

                new Neighbors { NorthNeighbor = guilds[4], EastNeighbor = guilds[0], SouthNeighbor = guilds[3], WestNeighbor = guilds[1] },
                new Neighbors { NorthNeighbor = guilds[3], EastNeighbor = guilds[2], SouthNeighbor = guilds[1], WestNeighbor = guilds[0] },
                new Neighbors { NorthNeighbor = guilds[4], EastNeighbor = guilds[1], SouthNeighbor = guilds[0], WestNeighbor = guilds[2] },
                new Neighbors { NorthNeighbor = guilds[1], EastNeighbor = guilds[4], SouthNeighbor = guilds[2], WestNeighbor = guilds[3] },
                new Neighbors { NorthNeighbor = guilds[0], EastNeighbor = guilds[2], SouthNeighbor = guilds[3], WestNeighbor = guilds[4] },
            };
        }

        private IEnumerator YieldAndBuildCards()
        {
            string color = "NOCOLOR";
            for (int i = 0; i < cards.cards.Length; i++)
            {
                Card card = cards.cards[i];
                string[] possibleGuildnames = GetPossibleGuildNamesForNeighboringStructures(card.color);
                List<Neighbors> neighbors = CreateNeighboringStructures(possibleGuildnames);

                while(neighbors.Count > 0)
                {
                    yield return new WaitForEndOfFrame();

                    if (card.color != color)
                    {
                        SetImages(card);
                    }
                    color = card.color;

                    CreateCard(card, neighbors[0]);
                    neighbors.RemoveAt(0);
                    screenshotWriter.MakeScreenshot();
                }
            }
        }        

        private void CreateCard(Card card, Neighbors neighbors)
        {
            northNeighbor.SetImage(LoadSprite($"/Cards/{card.color}/LogoBackground.png"), LoadSprite($"/Cards/{neighbors.NorthNeighbor}/Logo.png"));
            eastNeighbor.SetImage(LoadSprite($"/Cards/{card.color}/LogoBackground.png"), LoadSprite($"/Cards/{neighbors.EastNeighbor}/Logo.png"));
            southNeighbor.SetImage(LoadSprite($"/Cards/{card.color}/LogoBackground.png"), LoadSprite($"/Cards/{neighbors.SouthNeighbor}/Logo.png"));
            westNeighbor.SetImage(LoadSprite($"/Cards/{card.color}/LogoBackground.png"), LoadSprite($"/Cards/{neighbors.WestNeighbor}/Logo.png"));

            title.text = card.title;
            description.text = card.description;
        }

        private void SetImages(Card card)
        {
            CreateResources(card);

            //effect.sprite = LoadSprite($"Guilds/Effects/{card.effect}.png");
            background.sprite = LoadSprite($"Guilds/Backgrounds/{card.background}.png");
            foreground.sprite = LoadSprite($"Guilds/Foregrounds/{card.foreground}.png");
            border.sprite = LoadSprite($"Cards/{card.color}/Border.png");
            logo.sprite = LoadSprite($"Cards/{card.color}/Logo.png");
            cardFace.sprite = LoadSprite($"Cards/{card.color}/Face.png");
            descriptionBackground.sprite = LoadSprite($"Cards/{card.color}/Description.png");
            logoBackground.sprite = LoadSprite($"Cards/{card.color}/LogoBackground.png");
            titleBackground.sprite = LoadSprite($"Cards/{card.color}/Title.png");
            leftResourceBackground.sprite = LoadSprite($"Cards/{card.color}/Resource.png");
            rightResourceBackground.sprite = LoadSprite($"Cards/{card.color}/Resource.png");
        }
               
        private void CreateResources(Card card)
        {
            for (int i = 0; i < resources.Length; i++)
            {
                try
                {
                    string resource = card.cost[i];
                    resources[i].gameObject.SetActive(true);
                    resources[i].SetImage(LoadSprite($"Resources/Backgrounds/{resource.ToLower()}.png"), LoadSprite($"Resources/Foregrounds/{resource.ToLower()}.png"));
                }
                catch (IndexOutOfRangeException)
                {
                    resources[i].gameObject.SetActive(false);   
                }                
            }
        }      

        private Sprite LoadSprite(string filePath)
        {
            byte[] pngBytes = File.ReadAllBytes($"{Application.dataPath}/Images/{filePath}");
            Texture2D texture = new Texture2D(2, 2);

            texture.LoadImage(pngBytes);

            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(.5f, .5f), 100.0f);
        }
    }
}