using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttractable
{
    void Attract(float attractDistance, float attractForce);
    void StartedAttract();
    void EndedAttract();
}
