using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultLane : MonoBehaviour, ILane
{
    public float GetYPos()
    {
        return transform.position.y;
    }
    public Vector3 GetLanePosition()
    {
        return transform.position;
    }
}
