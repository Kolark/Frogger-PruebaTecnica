using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class MovableElement : MonoBehaviour
{
    Vector2 initPos;
    Vector2 endPos;
    float duration;
    float speed;
    public Action onMovementEnd;
    bool completed = false;
    public void SetupElement(Vector2 initPos, Vector2 endPos,float speed)
    {
        this.initPos = initPos;
        this.endPos = endPos;
        this.speed = speed;
        this.duration = Vector2.Distance(initPos, endPos) / speed;
        transform.position = initPos;
    }


    float time = 0;
    //t=d/v
    public void Move()
    {
        if (completed) return;
        time += Time.deltaTime;
        transform.position = Vector2.Lerp(initPos,endPos,time/duration);
        if ((Vector2)transform.position == endPos)
        {
        onMovementEnd?.Invoke();
            completed = true;
        }
    }

    public void ReturnToInitialPos()
    {
        time = 0;
        completed = false;
    }
}
