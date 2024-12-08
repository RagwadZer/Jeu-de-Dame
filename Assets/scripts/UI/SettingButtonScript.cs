using UnityEngine;
using UnityEngine.UI;

public class SettingButtonScript : UIClickable
{
    int settingTriggerAnimatorHash;
    Animator animator;
    private Image image;

    private void Awake()
    {
        BaseSprite = Resources.Load<Sprite>("Sprite/Menu");
        image = GetComponent<Image>();
        image.sprite = BaseSprite;

        settingTriggerAnimatorHash = Animator.StringToHash("Setting");
        animator = GetComponentInParent<Animator>();
    }
    public override void OnClickHandler()
    {
        animator.SetTrigger(settingTriggerAnimatorHash);
    }
}
