using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SphereCollider))]
public class Collector : MonoBehaviour
{
    public static UnityEvent OnCollectProp = new UnityEvent();
    [SerializeField] private float _attractDistance = 5f;
    [SerializeField] private float _collectDistance = 2f;
    [SerializeField] private float _attractForce = 1f;

    private SphereCollider _collider;

    private void Awake()
    {
        _collider = GetComponent<SphereCollider>();
        _collider.radius = _attractDistance;
        DOTween.SetTweensCapacity(1500, 1500);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IAttractable attractable))
        {
            attractable.StartedAttract();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IAttractable attractable))
        {
            attractable.EndedAttract();
        }
    }
}
