using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementTest : MonoBehaviour
{
    float time = 0;

    Vector3 initPos;
    Vector3 endPos;

    private void Awake()
    {
        initPos = transform.position;
        endPos = initPos + Vector3.right * 10;
    }

    private void Update()
    {
        time += Time.deltaTime;
        transform.position = Vector3.Lerp(initPos, endPos, time / 10);
    }
}
