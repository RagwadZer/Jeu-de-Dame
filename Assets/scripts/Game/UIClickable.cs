using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UIClickable : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    internal Sprite BaseSprite;
    

    public abstract void OnClickHandler();

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClickHandler();
    }

    public virtual void OnPointerDown(PointerEventData eventData) 
    {
    
    }

    public virtual void OnPointerUp(PointerEventData eventData) 
    {
      
    }
}
