using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourcePrefab : MonoBehaviour
{
    [SerializeField]
    private Image resourceImage;

    [SerializeField]
    private TextMeshProUGUI resourceAmount;

    public void SetImageAndAmound(Sprite sprite, int amount)
    {
        resourceImage.sprite = sprite;
        resourceAmount.text = amount.ToString();
    }
}
