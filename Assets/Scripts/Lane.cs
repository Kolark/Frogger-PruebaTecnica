using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lane : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    //PlayerController > Frog
    //PlayerController positions the frog
    //PlayerController > Inputs
    //PlayerController > LaneManager > Lanes
    //LaneManager > Instance Lanes By Default 1st last are Normal Line.
    //Lane > River & Road
    //Lane - Can walk Everywhere
    //River - Can Only Walk on certain Areas.
    //Road - Can walk everywhere 
    //Safe- Not Safe
    //Interface ou Class
    //GetYPos
    //------------------------------
    //------------------------------
    //------------------------------
    //------------------------------
    //------------------------------
    //------------------------------
    //------------------------------
    //------------------------------
    // Update is called once per frame
    void Update()
    {
        
    }
}


public enum LaneType
{
    Default,
    Road,
    River
}