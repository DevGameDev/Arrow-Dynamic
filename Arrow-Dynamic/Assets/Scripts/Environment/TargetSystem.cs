using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSystem : MonoBehaviour
{
    [SerializeField] GameObject door1;
    [SerializeField] GameObject door2;
    [SerializeField] GameObject target1;
    [SerializeField] GameObject target2;
    [SerializeField] GameObject target3;
    private TargetTrigger target1bool;
    private TargetTrigger target2bool;
    private TargetTrigger target3bool;
    [SerializeField] float xComp;
    [SerializeField] float yComp;
    [SerializeField] float zComp;

    private bool doorsOpen = true;

    private void Start()
    {
        target1bool = target1.GetComponent<TargetTrigger>();
        target2bool = target2.GetComponent<TargetTrigger>();
        target3bool = target3.GetComponent<TargetTrigger>();
    }

    private void Update()
    {
        if (target1bool.isOpen && target2bool.isOpen && target3bool.isOpen && doorsOpen) {
            openDoors();
            doorsOpen = false;
        }
    }

    private void openDoors()
    {
        door1.transform.position += new Vector3(xComp, yComp, zComp);
        door2.transform.position += new Vector3(-xComp, -yComp, -zComp);
    }
}
