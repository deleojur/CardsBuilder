using System.IO;
using System;
using System.Collections;
using UnityEngine;
using Json;

public class ScreenshotWriter : MonoBehaviour
{
    public RectTransform rectT; // Assign the UI element which you wanna capture
    int width; // width of the object to capture
    int height; // height of the object to capture

    // Use this for initialization
    void Start()
    {
        if (rectT != null)
        {
            width = Convert.ToInt32(rectT.rect.width);
            height = Convert.ToInt32(rectT.rect.height);
        }
    }

    private IEnumerator YieldAndTakeScreenshot(Card card)
    {
        yield return new WaitForEndOfFrame(); // it must be a coroutine 

        Vector2 temp = rectT.transform.position;
        var startX = temp.x - width / 2;
        var startY = temp.y - height / 2;

        var tex = new Texture2D(width, height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(startX, startY, width, height), 0, 0);
        tex.Apply();

        // Encode texture into PNG
        var bytes = tex.EncodeToPNG();
        Destroy(tex);

        string cardSpecs = "";
        if (card.tier > 0)
        {
            if (card.guilds.Length == 1)
            {
                cardSpecs = string.Format("{0}_tier_{1}", card.guilds[0], card.tier);
            }
        }
        else if (card.type.ToLower() == "building")
        {
            cardSpecs = "building";
        }
        else if (card.type.ToLower() == "promote")
        {
            cardSpecs = "promote";
        }
        else cardSpecs = "other";


        File.WriteAllBytes(string.Format("C:/Users/Jur/OneDrive - HvA/Monarchy/Cards/NewCards/Screenshots/{0}_{1}.png", cardSpecs, card.title), bytes);
    }   

    public void MakeScreenshot(Card card)
    {        
        StartCoroutine(YieldAndTakeScreenshot(card));        
    }
}
