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
            (!breakpointRules[0].isInt && breakpointRules[0].Check()))
        {
            breakpoints[0].breakpoint = false;

        }
    }
}
