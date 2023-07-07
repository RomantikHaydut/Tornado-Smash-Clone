using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public abstract class Props : MonoBehaviour, ICollectable
{
    protected float _collectTime = 1.5f;
    private float rotateSpeed = 3f;
    protected float riseAmount = 4f;
    protected bool isCollecting = false;

    public virtual void Collect(Transform player)
    {
        if (isCollecting)
        {
            return;
        }
        isCollecting = true;
        StartCoroutine(RotateAndMove_Coroutine(player));
        GoUp();
        GetSmall().OnComplete(() =>
        {
            EventManager.OnCollectedProp.Invoke();
            Die();
        });

    }


    private IEnumerator RotateAndMove_Coroutine(Transform player)
    {
        transform.parent = player;
        float timer = 0f;
        float distanceToPlayer = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(player.position.x, player.position.z));
        while (timer < _collectTime)
        {
            yield return new WaitForFixedUpdate();
            Vector3 lookPosition = new Vector3(player.position.x, transform.localPosition.y, player.position.z);
            transform.LookAt(lookPosition);
            transform.localPosition += transform.right * rotateSpeed * Time.fixedDeltaTime;
            transform.localPosition += transform.forward * distanceToPlayer * timer * Time.fixedDeltaTime;
            timer += Time.fixedDeltaTime;
        }
    }


    private Tween GetSmall()
    {
        return transform.DOScale(Vector3.zero, _collectTime).SetEase(Ease.InQuad);
    }

    protected virtual Tween GoUp()
    {
        return transform.DOMoveY(transform.position.y + riseAmount, _collectTime).SetEase(Ease.Linear);
    }

    private void Die()
    {
        gameObject.SetActive(false);
    }
}
