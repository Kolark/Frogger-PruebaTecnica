using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneManager : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] bool onValidate;
#endif

    int currentIndex = 0;

    [SerializeField]
    List<GameObject> lanesObjects;

    List<ILane> lanes;

    public int CurrentIndex { get { return currentIndex; } }
    public int LanesCount { get { return lanesObjects.Count; } }

    private void Awake()
    {
        lanes = new List<ILane>(lanesObjects.Count);
        for (int i = 0; i < lanesObjects.Count; i++)
        {
            lanes.Add(lanesObjects[i].GetComponent<ILane>());
        }
    }
    public ILane GoUp()
    {
        currentIndex = Mathf.Clamp(currentIndex + 1,0,lanes.Count -1 );
        return lanes[currentIndex];
    }
    public ILane GoDown()
    {
        currentIndex = Mathf.Clamp(currentIndex - 1, 0, lanes.Count - 1);
        return lanes[currentIndex];
    }

    public ILane GetCurrent()
    {
        return lanes[currentIndex];
    }

    public ILane GetStarter()
    {
        return lanes[0];
    }

    public void ResetIndex()
    {
        currentIndex = 0;
    }

    private void OnValidate()
    {
        ReOrganizeChildren();
    }

    void ReOrganizeChildren()
    {
        int childCount = transform.childCount;
        lanesObjects = new List<GameObject>(childCount);
        int startPos = -Mathf.CeilToInt(childCount / 2);
        for (int i = 0; i < childCount; i++)
        {
            Transform child = transform.GetChild(childCount - 1 - i);
            lanesObjects.Add(child.gameObject);
            child.transform.position = new Vector2(child.position.x,startPos+i);
        }
    }
}
