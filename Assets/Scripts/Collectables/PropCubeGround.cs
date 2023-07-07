using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropCubeGround : Props
{
    protected override Tween GoUp()
    {
        return transform.DOMoveY(transform.position.y + riseAmount, _collectTime / 2).SetEase(Ease.Linear).OnComplete(() =>
        {
            transform.DOMoveY(transform.position.y - riseAmount, _collectTime / 2).SetEase(Ease.Linear);
        });
        //return base.GoUp();

    }
}
