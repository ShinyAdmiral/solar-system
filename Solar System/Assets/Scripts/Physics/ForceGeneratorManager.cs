using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceGeneratorManager : MonoBehaviour
{
    [SerializeField] ForceGenerator[] forceGenerators;  //list of forces
    int length = 0;// array length

    //validate
    private void OnValidate()
    {
        forceGenerators = GetComponents<ForceGenerator>();
    }

    private void Start()
    {
        //get forces
        forceGenerators = GetComponents<ForceGenerator>();
        length = forceGenerators.Length;
    }

    //update method
    public void UpdateGenerators()
    {
        for (int i = 0; i < length; i++)
            forceGenerators[i].UpdateGenerator();

    }

    //get acculmative acceleration
    public Vector3 GetAcceleration(float invMass)
    {
        Vector3 force = Vector3.zero;

        for (int i = 0; i < length; i++)
            force += forceGenerators[i].GetForce();

        return force * invMass;
    }
}
