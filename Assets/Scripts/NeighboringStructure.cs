using UnityEngine;
using UnityEngine.UI;

public class NeighboringStructure : MonoBehaviour
{
    [SerializeField]
    private Image background;

    [SerializeField]
    private Image logo;

    public void SetImage(Sprite background, Sprite sprite)
    {
        this.background.sprite = background;
        logo.sprite = sprite;
    }
}
