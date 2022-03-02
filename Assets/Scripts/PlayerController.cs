using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PlayerController : MonoBehaviour
{
    const string PLATFORM_TAG = "Platform";
    const string HAZARD_TAG = "Hazard";
    const string HAZARDGROUND_TAG = "HazardGround";

    [SerializeField] LayerMask groundMask;

    public Action onHorizontalMove;
    public Action<float> onVerticalMove;
    public Action onDeath;

    bool isActive = false;

    void Update()
    {
        if (!isActive) return;
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            onVerticalMove?.Invoke(1);
        }
        else if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            onVerticalMove?.Invoke(-1);

        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveHorizontal(1);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveHorizontal(-1);
        }
    }

    public void ActivatePlayer()
    {
        isActive = true;
    }
    public void DeActivatePlayer()
    {
        isActive = false;
    }
    public void MoveVertical(float posY)
    {
        Vector2 pos = new Vector2(transform.position.x, posY);
        transform.position = pos;
        CheckOnMove(pos);
    }

    void MoveHorizontal(float dir)
    {
        int toMove = dir > 0 ? 1 : -1;
        Vector3 newPos = transform.position + Vector3.right * toMove;
        transform.position = newPos;
        CheckOnMove(newPos);
    }


    void CheckOnMove(Vector2 nextPosition)
    {
        Collider2D[] collisions = Physics2D.OverlapBoxAll(nextPosition, Vector2.one * 0.8f, 0, groundMask);
        bool foundHazard = false;
        Collider2D foundPlatForm = null;
        for (int i = 0; i < collisions.Length; i++)
        {
            if (collisions[i].CompareTag(HAZARDGROUND_TAG))
            {
                foundHazard = true;
            }
            if (collisions[i].CompareTag(PLATFORM_TAG))
            {
                foundPlatForm = collisions[i];
            }
        }
        
        if (foundPlatForm)
        {
            transform.parent = foundPlatForm.transform;
        }
        else
        {
            transform.parent = null;
        }

        if (foundHazard && foundPlatForm == null)
        {
            onDeath?.Invoke();
            Debug.Log("DEAD");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(HAZARD_TAG))
        {
            onDeath?.Invoke();
            Debug.Log("DEAD From Trigger");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position + Vector3.left, Vector2.one * 0.8f);
        Gizmos.DrawWireCube(transform.position + Vector3.up, Vector2.one * 0.8f);
        Gizmos.DrawWireCube(transform.position + Vector3.right, Vector2.one * 0.8f);
    }
}
