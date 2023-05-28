using UnityEngine;
using Cinemachine;

public class PathFollower : MonoBehaviour
{
    public CinemachinePathBase path;
    public float speed = 1f;

    private float pathPosition;

    void Update()
    {
        pathPosition += speed * Time.deltaTime;
        pathPosition %= path.PathLength;
        transform.position = path.EvaluatePositionAtUnit(pathPosition, CinemachinePathBase.PositionUnits.Distance);
    }
}