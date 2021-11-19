using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Json
{
    public class JsonSerializer : MonoBehaviour
    {
        private Cards cards;
        private ResourceSort resourceSort;
        private PrerequisitesSort prerequisitesSort;

        [SerializeField]
        private TextAsset JSONCards;

        [SerializeField]
        private Image background;

        [SerializeField]
        private Image foreground;

        [SerializeField]
        private Image cardBorder;

        [SerializeField]
        private Image cardNameBorder;

        [SerializeField]
        private Image cardIllustrationBorder;

        [SerializeField]
        private Image loyaltyBox;

        [SerializeField]
        private TextMeshProUGUI title;

        [SerializeField]
        private RectTransform guildPanel_1;

        [SerializeField]
        private RectTransform guildPanel_2;

        [SerializeField]
        private Image guild_1;

        [SerializeField]
        private Image[] guild_2;

        [SerializeField]
        private TextMeshProUGUI description;

        [SerializeField]
        private TextMeshProUGUI flavor;

        [SerializeField]
        private RectTransform resourcesPanel;

        [SerializeField]
        private Image type;

        [SerializeField]
        private TextMeshProUGUI loyaltyNumber;

        [SerializeField]
        private ScreenshotWriter screenshotWriter;

        [SerializeField]
        private ResourcePrefab[] resources;

        [SerializeField]
        private Image[] prerequisites;

        private Dictionary<int, Dictionary<string, Sprite>> cardBorderSprites;

        private void Awake()
        {
            LoadCardBordersSprites();
            resourceSort = new ResourceSort();
            prerequisitesSort = new PrerequisitesSort();
            cards = JsonUtility.FromJson<Cards>(JSONCards.text);
            StartCoroutine(YieldAndBuildCards());
        }

        private void LoadCardBordersSprites()
        {
            cardBorderSprites = new Dictionary<int, Dictionary<string, Sprite>>();
            string[] colors = { "Bronze", "Silver", "Gold", "Platinum", "Purple" };
            string[] spriteNames = { "border_card", "border_cardname", "border_illustration", "box_loyalty" };
            for (int i = 0; i < colors.Length; i++)
            {
                Dictionary<string, Sprite> colorBorderSprites = new Dictionary<string, Sprite>();
                string color = colors[i];

                foreach (string spriteName in spriteNames)
                {
                    Sprite sprite = LoadSprite(string.Format("Cards/{0}/{1}.png", color, spriteName));
                    colorBorderSprites.Add(spriteName, sprite);
                }
                cardBorderSprites.Add(i + 1, colorBorderSprites);
            }
        }

        private IEnumerator YieldAndBuildCards()
        {            
            for (int i = 0; i < cards.cards.Length; i++)
            {
                yield return new WaitForEndOfFrame();

                CreateCard(cards.cards[i]);
                screenshotWriter.MakeScreenshot(cards.cards[i]);
            }
        }

        private int NumberOfSpaces(int numberOfPrerequisites)
        {
            switch (numberOfPrerequisites)
            {
                default:
                    return 0;
                case 1:
                    return 4;
                case 2:
                    return 6;
                case 3:
                    return 8;
                case 4:
                    return 10;
            }
        }

        private void CreateCard(Card card)
        {
            int numberOfPrerequisites = CreatePrerequisites(card);

            background.sprite = LoadSprite(string.Format("Guilds/Backgrounds/{0}.png", card.background.ToLower()));
            foreground.sprite = LoadSprite(string.Format("Guilds/Avatars/{0}.png", card.foreground.ToLower()));
            type.sprite = LoadSprite(string.Format("Types/{0}.png", card.type.ToLower()));
            title.text = card.title;

            description.text = card.description.PadLeft(card.description.Length + NumberOfSpaces(numberOfPrerequisites), ' ');
            description.fontSize = card.descriptionFontSize > 0 ? card.descriptionFontSize : 22;


            if (card.flavor != null)
            {
                flavor.gameObject.SetActive(true);
                flavor.text = card.flavor;
            }
            else flavor.gameObject.SetActive(false);
            
            CreateGuilds(card);
            CreateLoyaltyAndBorderColor(card);
            CreateResources(card);            
        }      

        private void CreateGuilds(Card card)
        {
            if (card.guilds.Length == 0)
            {
                guildPanel_1.gameObject.SetActive(false);
                guildPanel_2.gameObject.SetActive(false);
            }
            else if (card.guilds.Length == 1)
            {
                guildPanel_1.gameObject.SetActive(true);
                guildPanel_2.gameObject.SetActive(false);

                guild_1.sprite = LoadSprite(string.Format("Guilds/{0}.png", card.guilds[0].ToLower()));
            }
            else if (card.guilds.Length == 2)
            {
                guildPanel_1.gameObject.SetActive(false);
                guildPanel_2.gameObject.SetActive(true);

                for (int i = 0; i < card.guilds.Length; i++)
                {
                    guild_2[i].sprite = LoadSprite(string.Format("Guilds/{0}.png", card.guilds[i].ToLower()));
                }
            }
        }

        private void CreateLoyaltyAndBorderColor(Card card)
        {
            if (card.loyalty > 0)
            {
                loyaltyBox.gameObject.SetActive(true);
                loyaltyNumber.text = card.loyalty.ToString();
                
                cardBorder.sprite = cardBorderSprites[card.loyalty]["border_card"];
                cardNameBorder.sprite = cardBorderSprites[card.loyalty]["border_cardname"];
                cardIllustrationBorder.sprite = cardBorderSprites[card.loyalty]["border_illustration"];
                loyaltyBox.sprite = cardBorderSprites[card.loyalty]["box_loyalty"];

                switch (card.loyalty)
                {
                    case 1:
                        loyaltyBox.rectTransform.sizeDelta = new Vector2(75, 75);
                        loyaltyNumber.fontSize = 50;
                        break;
                    case 2:
                        loyaltyBox.rectTransform.sizeDelta = new Vector2(80, 80);
                        loyaltyNumber.fontSize = 55;
                        break;
                    case 3:
                        loyaltyBox.rectTransform.sizeDelta = new Vector2(85, 85);
                        loyaltyNumber.fontSize = 60;
                        break;
                    case 4:
                        loyaltyBox.rectTransform.sizeDelta = new Vector2(90, 90);
                        loyaltyNumber.fontSize = 65;
                        break;
                    case 5:
                        loyaltyBox.rectTransform.sizeDelta = new Vector2(95, 95);
                        loyaltyNumber.fontSize = 70;
                        break;
                }
            }
            else
            {
                loyaltyBox.gameObject.SetActive(false);
            }
        }

        private void CreateResources(Card card)
        {
            Array.Sort(card.cost, resourceSort);

            for (int i = 0; i < 4; i++)
            {
                try
                {
                    Resource resource = card.cost[i];
                    resources[i].gameObject.SetActive(true);

                    resources[i].SetImageAndAmound(LoadSprite(string.Format("Resources/{0}.png", resource.type.ToLower())), resource.amount);
                }
                catch (IndexOutOfRangeException)
                {
                    resources[i].gameObject.SetActive(false);   
                }                
            }
        }

        private int CreatePrerequisites(Card card)
        {
            Array.Sort(card.cost, prerequisitesSort);
            int numberOfPrerequisites = 0;
            for (int i = 0; i < 5; i++)
            {
                prerequisites[i].gameObject.SetActive(false);
            }

            for (int i = 0; i < card.prerequisites.Length; i++)
            {                
                Resource prerequisite = card.prerequisites[i];

                for (int j = 0; j < prerequisite.amount; j++)
                {
                    prerequisites[numberOfPrerequisites].gameObject.SetActive(true);
                    prerequisites[numberOfPrerequisites].sprite = LoadSprite(string.Format("Guilds/{0}.png", prerequisite.type.ToLower()));
                    numberOfPrerequisites++;
                }
            }
            return numberOfPrerequisites;
        }

        private Sprite LoadSprite(string filePath)
        {
            byte[] pngBytes = File.ReadAllBytes(string.Format("{0}/Images/{1}", Application.dataPath, filePath));
            Texture2D texture = new Texture2D(2, 2);

            texture.LoadImage(pngBytes);

            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(.5f, .5f), 100.0f);
        }
    }
}