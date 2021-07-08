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

        [SerializeField]
        private TextAsset JSONCards;

        [SerializeField]
        private Image background;

        [SerializeField]
        private TextMeshProUGUI title;

        [SerializeField]
        private RectTransform factionPanel_1;

        [SerializeField]
        private RectTransform factionPanel_2;

        [SerializeField]
        private Image faction_1;

        [SerializeField]
        private Image[] faction_2;

        [SerializeField]
        private TextMeshProUGUI description;

        [SerializeField]
        private RectTransform resourcesPanel;

        [SerializeField]
        private Image type;

        [SerializeField]
        private Image tierBackground;

        [SerializeField]
        private TextMeshProUGUI tierNumber;

        [SerializeField]
        private ScreenshotWriter screenshotWriter;

        [SerializeField]
        private ResourcePrefab[] resources;

        private void Awake()
        {
            cards = JsonUtility.FromJson<Cards>(JSONCards.text);
            CreateCard(cards.cards[0]);
            screenshotWriter.MakeScreenshot(cards.cards[0].title);
        }

        private void CreateCard(Card card)
        {
            background.sprite = LoadSprite(card.backgroundUrl);
            type.sprite = LoadSprite(card.typeUrl);
            title.text = card.title;
            description.text = card.description;

            CreateFactions(card);
            CreateTier(card);
            CreateResources(card);
        }

        private void CreateFactions(Card card)
        {
            if (card.faction.Length == 1)
            {
                factionPanel_1.gameObject.SetActive(true);
                factionPanel_2.gameObject.SetActive(false);

                faction_1.sprite = LoadSprite(card.faction[0]);
            }
            else if (card.faction.Length == 2)
            {
                factionPanel_1.gameObject.SetActive(false);
                factionPanel_2.gameObject.SetActive(true);

                for (int i = 0; i < card.faction.Length; i++)
                {
                    faction_2[i].sprite = LoadSprite(card.faction[i]);
                }
            }
        }

        private void CreateTier(Card card)
        {
            if (card.tier > 0)
            {
                tierBackground.gameObject.SetActive(true);
                tierNumber.text = card.tier.ToString();
            }
            else
            {
                tierBackground.gameObject.SetActive(false);
            }
        }

        private void CreateResources(Card card)
        {
            for (int i = 0; i < 4; i++)
            {
                try
                {
                    Resource resource = card.cost[i];
                    resources[i].gameObject.SetActive(true);

                    resources[i].SetImageAndAmound(LoadSprite(resource.typeUrl), resource.amount);
                }
                catch (IndexOutOfRangeException)
                {
                    resources[i].gameObject.SetActive(false);   
                }                
            }
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