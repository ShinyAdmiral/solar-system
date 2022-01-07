using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grow : MonoBehaviour
{
    [SerializeField] float growRate = 0;

    // Update is called once per frame
    void Update()
    {
        transform.localScale += new Vector3(growRate, growRate, growRate) * Time.deltaTime;
    }
}
