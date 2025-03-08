using System;
using System.Collections.Generic;
using UnityEngine;

public class BreakpointManager : MonoBehaviour
{
    [SerializeField] List<EventBehaviour> breakpoints = new();
    
    [SerializeField] List<CheckerBehaviour> keys = new();

    [SerializeField] List<int> rules = new();

    void FixedUpdate()
    {
        if ((keys[0].isInt && keys[0].Check(rules[0])) ||
            (!keys[0].isInt && Convert.ToInt32(keys[0].Check()) == rules[0]))
        {
            Break();
        }
    }

    void Break()
    {
        breakpoints[0].breakpoint = false;

        breakpoints.RemoveAt(0);
        keys.RemoveAt(0);
        rules.RemoveAt(0);
    }
}
