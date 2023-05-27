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
    [SerializeField] private float Speed = 0.5f;
    [SerializeField] private float SlideAmount = 0.05f;
    private Coroutine AnimationCoroutine;

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
            AnimationCoroutine = StartCoroutine(openDoors());
            doorsOpen = false;
        }
    }

    private IEnumerator openDoors()
    {
        float time = 0;
        while(time < 1)
        {
            //new open code
            Vector3 startPositionDoor1 = door1.transform.position;
            Vector3 endPositionDoor1 = door1.transform.position + (SlideAmount * new Vector3(xComp, yComp, zComp));
            Vector3 startPositionDoor2 = door2.transform.position;
            Vector3 endPositionDoor2 = door2.transform.position + (SlideAmount * new Vector3(-xComp, -yComp, -zComp));
            door1.transform.position = Vector3.Lerp(startPositionDoor1, endPositionDoor1, time);
            door2.transform.position = Vector3.Lerp(startPositionDoor2, endPositionDoor2, time);

            yield return null;

            time += Time.deltaTime * Speed;

        }
        //original open code 
        //door1.transform.position += new Vector3(xComp, yComp, zComp);
        //door2.transform.position += new Vector3(-xComp, -yComp, -zComp);
    }
    
    ////original open code 
    //private void openDoors()
    //{
    //    door1.transform.position += new Vector3(xComp, yComp, zComp);
    //    door2.transform.position += new Vector3(-xComp, -yComp, -zComp);
    //}
}
