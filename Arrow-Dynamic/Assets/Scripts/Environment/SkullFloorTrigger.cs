using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullFloorTrigger : MonoBehaviour
{
    [SerializeField] GameObject floor;
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
    public AudioSource doorOpenSound;

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
            doorOpenSound.Play();
            doorsOpen = false;
        }
    }

    private IEnumerator openDoors()
    {
        float time = 0;
        while(time < 1)
        {
            //new open code
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
    //}
}
