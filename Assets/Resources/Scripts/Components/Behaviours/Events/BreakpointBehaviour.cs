using System;
using System.Collections.Generic;
using UnityEngine;

public class BreakpointBehaviour : MonoBehaviour
{
    [SerializeField] List<EventBehaviour> breakpoints = new();
    
    [SerializeField] List<CheckerBehaviour> breakpointRules = new();

    [SerializeField] List<int> rules = new();

    void FixedUpdate()
    {
        if ((breakpointRules[0].isInt && breakpointRules[0].Check(rules[0])) ||
            (!breakpointRules[0].isInt && Convert.ToInt32(breakpointRules[0].Check()) == rules[0]))
        {
            Break();
        }
    }

    void Break()
    {
        breakpoints[0].breakpoint = false;

        breakpoints.RemoveAt(0);
        breakpointRules.RemoveAt(0);
        rules.RemoveAt(0);
    }
}
