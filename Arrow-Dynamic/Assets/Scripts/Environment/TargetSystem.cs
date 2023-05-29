using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSystem : MonoBehaviour
{
    //doors to be moved
    [SerializeField] GameObject door1;
    [SerializeField] GameObject door2;

    //3 skulls needed to activate
    [SerializeField] GameObject target1;
    [SerializeField] GameObject target2;
    [SerializeField] GameObject target3;

    //reference to bool component in target script
    private TargetTrigger target1bool;
    private TargetTrigger target2bool;
    private TargetTrigger target3bool;

    //vector for the doors to be moved
    [SerializeField] float xComp;
    [SerializeField] float yComp;
    [SerializeField] float zComp;

    //speed and slide distance for doors
    [SerializeField] private float Speed = 0.5f;
    [SerializeField] private float SlideAmount = 0.05f;

    //coroutine for door sliding animation
    private Coroutine AnimationCoroutine;

    //audio source for sound
    public AudioSource doorOpenSound;

    //check to make sure doors arent open
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
            doorOpenSound.Play();
            doorsOpen = true;
        }
    }

    private IEnumerator openDoors()
    {
        float time = 0;
        while(time < 1)
        {
            //lerp position from start and end vector for both doors
            Vector3 startPositionDoor1 = door1.transform.position;
            Vector3 endPositionDoor1 = door1.transform.position + (SlideAmount * new Vector3(xComp, yComp, zComp));
            door1.transform.position = Vector3.Lerp(startPositionDoor1, endPositionDoor1, time);

            Vector3 startPositionDoor2 = door2.transform.position;
            Vector3 endPositionDoor2 = door2.transform.position + (SlideAmount * new Vector3(-xComp, -yComp, -zComp));
            door2.transform.position = Vector3.Lerp(startPositionDoor2, endPositionDoor2, time);

            yield return null;

            time += Time.deltaTime * Speed;

        }
    }
    
    ////original open code 
    //private void openDoors()
    //{
    //    door1.transform.position += new Vector3(xComp, yComp, zComp);
    //    door2.transform.position += new Vector3(-xComp, -yComp, -zComp);
    //}
}
