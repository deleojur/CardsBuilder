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
        [SerializeField]
        private TextAsset JSONHexagons;

        [SerializeField]
        private GameObject gain;

        [SerializeField]
        private new GameObject transform;

        [SerializeField]
        private Image gainLogo;

        [SerializeField]
        private TextMeshProUGUI titleText;

        [SerializeField]
        private GameObject amount;

        [SerializeField]
        private TextMeshProUGUI gainAmountText;

        [SerializeField]
        private GameObject transformFrom1;

        [SerializeField]
        private Image transformFromLogo;

        [SerializeField]
        private GameObject transformFrom2;

        [SerializeField]
        private Image[] transformFrom2Logo;

        [SerializeField]
        private TextMeshProUGUI[] transformAmountFrom2Text;

        [SerializeField]
        private Image transformToLogo;

        [SerializeField]
        private GameObject transformAmountFrom;       

        [SerializeField]
        private GameObject transformAmountTo;

        [SerializeField]
        private TextMeshProUGUI transformFromAmountText;

        [SerializeField]
        private TextMeshProUGUI transformToAmountText;

        [SerializeField]
        private ScreenshotWriter screenshotWriter;

        [SerializeField]
        private HexagonEdgePrefab[] hexagonEdges;

        [SerializeField]
        private GameObject hexagon;

        [SerializeField]
        private GameObject hexagonBack;

        [SerializeField]
        private TextMeshProUGUI backCategory;

        [SerializeField]
        private Sprite transformToYellow;

        [SerializeField]
        private Sprite transformToGreen;

        [SerializeField]
        private Sprite transformToBlue;

        [SerializeField]
        private Sprite transformToRed;

        [SerializeField]
        private Image transformToBackground;

        private void Awake()
        {
            HexagonColorManager hexagonColorManager = new HexagonColorManager();
            Hexagons hexagons = hexagonColorManager.GenerateHexagonsWithColors(JSONHexagons.text);
            
            StartCoroutine(YieldAndBuildCards(hexagons));
        }
        
        private IEnumerator YieldAndBuildCards(Hexagons hexagons)
        {
            Dictionary<string, int> titles = new Dictionary<string, int>();
            foreach (HexagonCategory category in hexagons.categories)
            {
                yield return new WaitForEndOfFrame();

                hexagon.SetActive(false);
                hexagonBack.SetActive(true);
                backCategory.text = category.name;
                screenshotWriter.MakeScreenshot(category.name);

                foreach (Hexagon hexagon in category.hexagons)
                {
                    yield return new WaitForEndOfFrame();

                    this.hexagon.SetActive(true);
                    hexagonBack.SetActive(false);
                    CreateHexagon(hexagon);

                    string cardTitle = $"{category.name}_{hexagon.title}";
                    if (titles.ContainsKey(cardTitle))
                    {
                        titles[cardTitle]++;
                        screenshotWriter.MakeScreenshot($"{cardTitle}_{titles[cardTitle]}");
                    }
                    else
                    {
                        titles.Add(cardTitle, 0);
                        screenshotWriter.MakeScreenshot($"{cardTitle}_0");
                    }
                }                
            }
        }        

        private void CreateHexagon(Hexagon hexagon)
        {
            titleText.text = hexagon.title;
            if (hexagon.type == "gain")
            {
                gainLogo.sprite = LoadSprite($"Logos/{hexagon.logo}.png");
                gain.SetActive(true);
                transform.SetActive(false);

                amount.SetActive(hexagon.amount != 0);
                gainAmountText.text = hexagon.amount.ToString();
            } else if  (hexagon.type == "transform")
            {
                gain.SetActive(false);
                transform.SetActive(true);
                transformToLogo.sprite = LoadSprite($"Logos/{hexagon.toLogo}.png");

                string toLogoBackgroundColor = hexagon.toLogoBackgroundColor;

                switch (toLogoBackgroundColor)
                {
                    default:
                    case "yellow":
                        transformToBackground.sprite = transformToYellow;
                        break;
                    case "green":
                        transformToBackground.sprite = transformToGreen;
                        break;
                    case "blue":
                        transformToBackground.sprite = transformToBlue;
                        break;
                    case "red":
                        transformToBackground.sprite = transformToRed;
                        break;
                }

                if (hexagon.logo2 != null && hexagon.amount2 > 0)
                {
                    transformFrom1.SetActive(false);
                    transformFrom2.SetActive(true);

                    transformFrom2Logo[0].sprite = LoadSprite($"Logos/{hexagon.logo}.png");
                    transformFrom2Logo[1].sprite = LoadSprite($"Logos/{hexagon.logo2}.png");

                    transformAmountFrom2Text[0].text = hexagon.amount.ToString();
                    transformAmountFrom2Text[1].text = hexagon.amount2.ToString();

                    transformAmountTo.SetActive(hexagon.toAmount > 0);
                    transformToAmountText.text = hexagon.toAmount.ToString();
                }
                else
                {
                    transformFrom1.SetActive(true);
                    transformFrom2.SetActive(false);

                    transformFromLogo.sprite = LoadSprite($"Logos/{hexagon.logo}.png");

                    transformAmountFrom.SetActive(hexagon.amount > 0);
                    transformAmountTo.SetActive(hexagon.toAmount > 0);

                    transformFromAmountText.text = hexagon.amount.ToString();
                    transformToAmountText.text = hexagon.toAmount.ToString();
                }
            }

            for (int i = 0; i < 6; i++)
            {
                HexagonEdge edge = hexagon.edges[i];
                hexagonEdges[i].SetEdgeColors(edge);
            }
        }

        private void SetImages(Hexagon hexagon)
        {
           
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
