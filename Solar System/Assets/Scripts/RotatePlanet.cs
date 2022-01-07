using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePlanet : MonoBehaviour
{
    [SerializeField]float rotationSpeed = 1;

    // Update is called once per frame
    void Update() 
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0, Space.Self);
    }
}
