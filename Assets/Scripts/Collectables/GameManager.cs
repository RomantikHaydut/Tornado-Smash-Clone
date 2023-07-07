using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int propCount;
    [SerializeField] private int collectedPropCount;
    private bool isLevelEnded = false;

    private void Awake()
    {
        EventManager.OnLevelStart.AddListener(CalculatePropCount);
        EventManager.OnCollectedProp.AddListener(CollectProp);
    }

    private void Start()
    {
        EventManager.OnLevelStart.Invoke();
    }

    private void CollectProp()
    {
        if (!isLevelEnded)
        {
            collectedPropCount++;
            if (collectedPropCount >= propCount)
            {
                EventManager.OnLevelEnd.Invoke();
                isLevelEnded = true;
            }
        }

    }

    private void CalculatePropCount()
    {
        var props = FindObjectsOfType<Props>();
        foreach (var prop in props)
        {
            if (prop.TryGetComponent(out IAttractable x))
            {
                propCount++;
            }
        }
    }
}
