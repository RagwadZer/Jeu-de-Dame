using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BGScrollerScript : MonoBehaviour
{
    RawImage rawImage;
    /// <summary>
    /// a utilisé pour faire scroller le BG
    /// </summary>
    float ScrollOnX = 0.2f , ScrollOnY = 0.3f;
    float ZoomCoef = 0.25f;

    private void Start()
    {
        rawImage = GetComponent<RawImage>();
    }

    private void Update()
    {
        //Scroll(ScrollOnX, ScrollOnY);
        Zoom();
    }

    private void Scroll(float x, float y)
    {
        Rect rect = rawImage.uvRect;

        rect.x += x * Time.deltaTime; 
        rect.y += y * Time.deltaTime;

        rawImage.uvRect = rect;
    }

    private void Zoom()
    {
        Rect uvRect = rawImage.uvRect;
        uvRect.width -= ZoomCoef * Time.deltaTime;
        uvRect.height -= ZoomCoef * Time.deltaTime;

        // Centre le rectangle UV pour que le zoom se fasse depuis le centre
        uvRect.x = (1f - uvRect.width) / 2f;
        uvRect.y = (1f - uvRect.height) / 2f;

        rawImage.uvRect = uvRect;
    }
}
