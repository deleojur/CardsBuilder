using System.Collections;
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
        private Image transformFromLogo;

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

        private void Awake()
        {
            HexagonColorManager hexagonColorManager = new HexagonColorManager();
            Hexagons hexagons = hexagonColorManager.GenerateHexagonsWithColors(JSONHexagons.text);
            StartCoroutine(YieldAndBuildCards(hexagons));
        }
        
        private IEnumerator YieldAndBuildCards(Hexagons hexagons)
        {
            
            for (int i = 0; i < hexagons.hexagons.Length; i++)
            {
                yield return new WaitForEndOfFrame();
                Hexagon hexagon = hexagons.hexagons[i];
                CreateHexagon(hexagon);

                screenshotWriter.MakeScreenshot();
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

                amount.SetActive(hexagon.amount != null);
                gainAmountText.text = hexagon.amount;
            } else if  (hexagon.type == "transform")
            {
                gain.SetActive(false);
                transform.SetActive(true);

                transformFromLogo.sprite = LoadSprite($"Logos/{hexagon.logo}.png");
                transformToLogo.sprite = LoadSprite($"Logos/{hexagon.toLogo}.png");

                transformAmountFrom.SetActive(hexagon.amount != null);
                transformAmountTo.SetActive(hexagon.toAmount != null);

                transformFromAmountText.text = hexagon.amount;
                transformToAmountText.text = hexagon.toAmount;
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