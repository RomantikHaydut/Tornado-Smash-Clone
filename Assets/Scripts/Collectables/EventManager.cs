using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class EventManager 
{
    public static UnityEvent OnLevelStart = new UnityEvent();
    public static UnityEvent OnLevelEnd = new UnityEvent();
    public static UnityEvent OnCollectedProp = new UnityEvent();
}
