using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourcePrefab : MonoBehaviour
{
    [SerializeField]
    private Image resourceForeground;

    [SerializeField]
    private Image resourceBackground;

    public void SetImage(Sprite background, Sprite foreground)
    {
        resourceBackground.sprite = background;
        resourceForeground.sprite = foreground;
    }
}
