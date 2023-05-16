using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponWheelButtonController : MonoBehaviour
{
    public ArrowType type;
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
        WeaponWheelController.Instance.selectedType = type;
    }

    public void HoverExit()
    {
        anim.SetBool("Hover", false);
        itemText.text = "";
    }
}