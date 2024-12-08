using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayButtonScript : UIClickable
{
    Sprite PressedSprite;
    private Image image;

    void Awake()
    {
        //PlayButton = GetComponent<Button>();
        BaseSprite = Resources.Load<Sprite>("Sprite/Play Button");
        PressedSprite = Resources.Load<Sprite>("Sprite/Play Button - Compressed");

        image = GetComponent<Image>();
        image.sprite = BaseSprite;

        
    }

  

    public override void OnClickHandler()
    {
        transform.parent.gameObject.SetActive(false);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        image.sprite = PressedSprite;
    }
    public override void OnPointerUp(PointerEventData eventData)
    {
        image.sprite = BaseSprite;
    }
}
