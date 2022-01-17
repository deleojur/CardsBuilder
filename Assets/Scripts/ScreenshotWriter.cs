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

    private int index = 0;
    private IEnumerator YieldAndTakeScreenshot()
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

        File.WriteAllBytes(($"C:/Users/Jur/OneDrive - HvA/Monarchy/Cards/NewCards/Screenshots/card_{index++}.png"), bytes);
    }   

    public void MakeScreenshot()
    {        
        StartCoroutine(YieldAndTakeScreenshot());        
    }
}
