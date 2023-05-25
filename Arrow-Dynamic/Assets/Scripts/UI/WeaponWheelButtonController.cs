using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponWheelButtonController : MonoBehaviour
{
    public string itemName;
    public TextMeshProUGUI itemText;
    public Image selectedItem;
    public Sprite icon;

    private Animator anim;
    private bool selected;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void HoverEnter()
    {
        anim.SetBool("Hover", true);
        WeaponWheelController.Instance.selectedItem.sprite = icon;
        itemText.text = itemName;
    }

    public void HoverExit()
    {
        anim.SetBool("Hover", false);
        itemText.text = "";
    }

    public void ChangeBasicArrow()
    {
        Bow.Instance.SetArrowType(ArrowType.Basic);
    }

    public void ChangeIceArrow()
    {
        Bow.Instance.SetArrowType(ArrowType.Ice);
    }

    public void ChangeGrappleArrow()
    {
        Bow.Instance.SetArrowType(ArrowType.Grapple);
        
    }

    public void ChangeBombArrow()
    {
        Bow.Instance.SetArrowType(ArrowType.Bomb);
    }

    public void ChangeGravityArrow()
    {
        Bow.Instance.SetArrowType(ArrowType.Gravity);
    }

    public void ChangeTeleportArrow()
    {
        Bow.Instance.SetArrowType(ArrowType.Teleport);
    }

    public void ChangeTimeArrow()
    {
        Bow.Instance.SetArrowType(ArrowType.Time);
    }

    public void ChangeWindArrow()
    {
        Bow.Instance.SetArrowType(ArrowType.Wind);
    }
}