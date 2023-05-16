using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
public class WeaponWheelController : MonoBehaviour
{
    public Animator anim;
    private bool weaponWheelSelected = false;
    public Image selectedItem;
    public Sprite noImage;
    public static int weaponID;

    // Update is called once per frame
    public void input_action(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            weaponWheelSelected = !weaponWheelSelected;
        }
        if (weaponWheelSelected)
        {
            anim.SetBool("OpenWeaponWheel", true);
        }
        else
        {
            anim.SetBool("OpenWeaponWheel", false);
        }

        switch (weaponID)
        {
            case 0:
                selectedItem.sprite = noImage;
                break;
            case 1:
                Debug.Log("Regular_Arrow1");
                break;
            case 2:
                Debug.Log("Regular_Arrow2");
                break;
            case 3:
                Debug.Log("Regular_Arrow3");
                break;
            case 4:
                Debug.Log("Regular_Arrow4");
                break;
            case 5:
                Debug.Log("Regular_Arrow5");
                break;
            case 6:
                Debug.Log("Regular_Arrow6");
                break;
            case 7:
                Debug.Log("Regular_Arrow7");
                break;
            case 8:
                Debug.Log("Regular_Arrow8");
                break;
        }


    }
}
