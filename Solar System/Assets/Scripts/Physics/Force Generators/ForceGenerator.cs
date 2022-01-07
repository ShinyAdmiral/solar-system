using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ForceGeneratorManager))]
public abstract class ForceGenerator : MonoBehaviour
{
    protected Vector3 force = Vector3.zero;     //force

    public abstract void UpdateGenerator();     //update method

    public Vector3 GetForce() { return force; } //accessor
}
