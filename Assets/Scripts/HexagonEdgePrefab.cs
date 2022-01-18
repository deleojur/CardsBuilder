using Json;
using UnityEngine;
using UnityEngine.UI;
using UnityEssentials.Extensions;

public class HexagonEdgePrefab : MonoBehaviour
{
    [SerializeField]
    private GameObject oneColor;

    [SerializeField]
    private Image oneColorImage;

    [SerializeField]
    private GameObject twoColors;

    [SerializeField]
    private Image[] twoColorsImages;

    [SerializeField]
    private GameObject threeColors;

    [SerializeField]
    private Image[] threeColorsImages;

    public void SetEdgeColors(HexagonEdge edge)
    {
        switch (edge.colors.Length)
        {
            case 0:
                SetNoColors();
                break;
            case 1:
                SetOneColor(edge.colors[0]);
                break;
            case 2:
                SetTwoColors(edge.colors);
                break;
            case 3:
                SetThreeColors(edge.colors);
                break;
        }
    }
    
    private void SetNoColors()
    {
        oneColor.SetActive(false);
        twoColors.SetActive(false);
        threeColors.SetActive(false);
    }

    private void SetOneColor(string color)
    {
        SetNoColors();
        oneColor.SetActive(true);

        oneColorImage.color = ColorsToNames.GetColor(color);
    }

    private void SetTwoColors(string[] colors)
    {
        SetNoColors();
        twoColors.SetActive(true);

        for (int i = 0; i < colors.Length; i++)
        {
            twoColorsImages[i].color = ColorsToNames.GetColor(colors[i]);
        }
    }

    private void SetThreeColors(string[] colors)
    {
        SetNoColors();
        threeColors.SetActive(true);

        for (int i = 0; i < colors.Length; i++)
        {
            threeColorsImages[i].color = ColorsToNames.GetColor(colors[i]);
        }
    }
}
