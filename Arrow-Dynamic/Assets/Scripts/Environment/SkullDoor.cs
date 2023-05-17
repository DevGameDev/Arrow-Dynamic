using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullDoor : MonoBehaviour
{
    public static int skullsLeft = 3;
    public bool opened = false;
    public float moveSpeed = 1.0f;
    public float moveDistance = 3.0f;

    private void Update()
    {
        if (!opened && skullsLeft < 1)
        {
            StartCoroutine(MoveDoorDown());
            opened = true;
        }
    }

    IEnumerator MoveDoorDown()
    {
        float distanceMoved = 0; // keep track of how much the door has moved

        while (distanceMoved < moveDistance)
        {
            // calculate how much to move this frame
            float distanceToMove = moveSpeed * Time.deltaTime;

            // ensure we don't move beyond the desired moveDistance
            if (distanceMoved + distanceToMove > moveDistance)
            {
                distanceToMove = moveDistance - distanceMoved;
            }

            // move the door down
            transform.position = transform.position - new Vector3(0, distanceToMove, 0);

            // update the total distance moved
            distanceMoved += distanceToMove;

            // wait until next frame
            yield return null;
        }
    }
}
