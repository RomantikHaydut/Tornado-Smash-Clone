using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropCube : Props, IAttractable
{
    private State _state;
    private Transform _collectorTransform;
    private Rigidbody _rigidbody;
    private float _distanceToCollector;

    private float _attractDistance = 5f;
    private float _collectDistance = 2f;
    private float _attractForce = 1f;

    private enum State
    {
        Idle,
        Attracting,
        Collecting,
    }


    private void Awake()
    {
        _state = State.Idle;
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.useGravity = false;
        _collectorTransform = FindObjectOfType<Collector>().transform;
    }


    private void Update()
    {
        CheckState();
        ExecuteState();
    }

    public override void Collect(Transform player)
    {
        base.Collect(player);
        _rigidbody.useGravity = false;
        _rigidbody.velocity = Vector3.zero;
    }

    private void CheckState()
    {
        Vector2 _myPosition = new Vector2(transform.position.x, transform.position.z);
        Vector2 _collectorPosition = new Vector2(_collectorTransform.position.x, _collectorTransform.position.z);
        _distanceToCollector = Vector2.Distance(_myPosition, _collectorPosition);

        if (_distanceToCollector <= _attractDistance && _distanceToCollector > _collectDistance)
        {
            _state = State.Attracting;
        }
        else if (_distanceToCollector <= _collectDistance)
        {
            _state = State.Collecting;
        }
    }

    private void ExecuteState()
    {
        switch (_state)
        {
            case State.Idle:
                break;
            case State.Attracting:
                Attract(_attractDistance, _attractForce);
                break;
            case State.Collecting:
                Collect(FindObjectOfType<Collector>().transform);
                break;
        }
    }


    public void Attract(float attractDistance, float attractForce)
    {
        if (!isCollecting)
        {
            _rigidbody.useGravity = true;
            Vector3 _attractDirection = _collectorTransform.position - transform.position;
            _rigidbody.AddForce(_attractDirection * attractForce, ForceMode.Force);
        }
    }

    public void StartedAttract()
    {
        _state = State.Attracting;
    }

    public void EndedAttract()
    {
        _state = State.Idle;
    }
}
