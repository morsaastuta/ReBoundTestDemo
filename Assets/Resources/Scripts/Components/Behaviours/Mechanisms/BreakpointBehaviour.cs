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
        bool willBreak = false;

        if (breakpointRules[0].isInt)
        {
            if (breakpointRules[0].CheckInt() >= rules[0]) willBreak = true;
        }
        else
        {
            if (Convert.ToInt32(breakpointRules[0].CheckBool()) == rules[0]) willBreak = true;
        }

        if (willBreak)
        {
            breakpoints[0].breakpoint = false;
        }
    }
}
