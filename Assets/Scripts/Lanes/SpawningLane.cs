using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningLane : MonoBehaviour, ILane
{
    [SerializeField] MovableElement objToSpawn;
    [SerializeField] int maxAmountToSpawn;
    [SerializeField] float defaultDelay;
    [SerializeField] float tweenSpeed;
    [SerializeField] float randomForce;
    [SerializeField] float distanceSpawn;
    [SerializeField] Direction spawnDirection;

    List<MovableElement> currentlyMovedElements;
    Queue<MovableElement> reservedElements;

    float time = 0;
    int currentIndex = 0;
    float[] spawningTimes;

    private void Awake()
    {
        spawningTimes = new float[maxAmountToSpawn];
        spawningTimes[0] = defaultDelay + (float)UnityEngine.Random.Range(0f, 1f) * Mathf.Abs(randomForce);

        currentlyMovedElements = new List<MovableElement>(maxAmountToSpawn);
        reservedElements = new Queue<MovableElement>(maxAmountToSpawn);

        for (int i = 1; i < spawningTimes.Length; i++)
        {
            spawningTimes[i] = spawningTimes[i - 1] + defaultDelay + (float)UnityEngine.Random.Range(0f,1f)*Mathf.Abs(randomForce);
        }

        for (int i = 0; i < maxAmountToSpawn; i++)
        {
            GameObject obj = Instantiate(objToSpawn.gameObject,transform);
            MovableElement movable = obj.GetComponent<MovableElement>();
            movable.onMovementEnd += AddBackToQueue;
            Vector2 initPos = (Vector2)transform.position + Vector2.right * distanceSpawn * (int)spawnDirection;
            Vector2 endPos = (Vector2)transform.position + Vector2.right * distanceSpawn * (int)spawnDirection * -1;
            movable.SetupElement(initPos, endPos, tweenSpeed);
            reservedElements.Enqueue(movable);
        }
    }

    private void Update()
    {
        time += Time.deltaTime;

        if (time > spawningTimes[currentIndex])
        {
            if(reservedElements.Count > 0)
            {
                currentIndex++;
                if (currentIndex == spawningTimes.Length - 1)
                {
                    currentIndex = 0;
                    time = 0;
                }

                MovableElement element = reservedElements.Dequeue();
                element.ReturnToInitialPos();
                currentlyMovedElements.Add(element);
            }
        }

        for (int i = 0; i < currentlyMovedElements.Count; i++)
        {
            currentlyMovedElements[i].Move();
        }
    }

    public void AddBackToQueue()
    {
        MovableElement element = currentlyMovedElements[0];
        currentlyMovedElements.RemoveAt(0);
        reservedElements.Enqueue(element);
    }

    public float GetYPos()
    {
        return transform.position.y;
    }

    public Vector3 GetLanePosition()
    {
        return transform.position;
    }
}

public enum Direction
{
    Left = -1,
    Right = 1
}