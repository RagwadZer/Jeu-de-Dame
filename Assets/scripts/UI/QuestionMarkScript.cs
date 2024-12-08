using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionMarkScript : UIClickable
{
    Animator animator;
    int CredithashCode=0;
    private void Awake()
    {
        BaseSprite = Resources.Load<Sprite>("Sprite/QuestionMark");
        Sprite sprite = GetComponent<Image>().sprite;
        sprite = BaseSprite;
        
        animator = GetComponentInParent<Animator>();
        CredithashCode = Animator.StringToHash("Credit");
    }
    public override void OnClickHandler()
    {
        animator.SetTrigger(CredithashCode);
    }

 

    
}
