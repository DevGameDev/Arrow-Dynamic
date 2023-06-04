using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullFloorTrigger : MonoBehaviour
{/*
    //floor to be moved
    [SerializeField] GameObject floor;

    //3 targets needed to shoot
    [SerializeField] GameObject target1;
    [SerializeField] GameObject target2;
    [SerializeField] GameObject target3;

    //reference to bool component in skull scripts
    private TargetTrigger target1bool;
    private TargetTrigger target2bool;
    private TargetTrigger target3bool;

    //vector to move the floor
    [SerializeField] float xComp;
    [SerializeField] float yComp;
    [SerializeField] float zComp;

    //speed and slide distance to move
    [SerializeField] private float Speed;
    [SerializeField] private float SlideAmount;

    //coroutine for the door sliding animtation
    private Coroutine AnimationCoroutine;

    private bool doorsOpen = false;

    private void Start()
    {
        //get reference to bool in target scripts
        target1bool = target1.GetComponent<TargetTrigger>();
        target2bool = target2.GetComponent<TargetTrigger>();
        target3bool = target3.GetComponent<TargetTrigger>();
    }

    private void Update()
    {
        //check to make sure all skulls active and doors are closed
        if (target1bool.isOpen && target2bool.isOpen && target3bool.isOpen && !doorsOpen) {
            AnimationCoroutine = StartCoroutine(openDoors());
            doorsOpen = true;
        }
    }

    private IEnumerator openDoors()
    {
        float time = 0;
        while(time < 1)
        {
            //lerp the position from start to end
            Vector3 startPositionFloor = floor.transform.position;
            Vector3 endPositionFloor = floor.transform.position + (SlideAmount * new Vector3(xComp, yComp, zComp));
            floor.transform.position = Vector3.Lerp(startPositionFloor, endPositionFloor, time);

            yield return null;

            time += Time.deltaTime * Speed;

        }
    }

    ////original open code 
    //private void openDoors()
    //{
    //    door1.transform.position += new Vector3(xComp, yComp, zComp);
    //    door2.transform.position += new Vector3(-xComp, -yComp, -zComp);
    //} */
}