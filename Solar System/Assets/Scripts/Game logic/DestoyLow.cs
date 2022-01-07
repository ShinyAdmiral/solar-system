using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoyLow : MonoBehaviour
{
    [SerializeField] float killPoint = -9f;
    void Update()
    {
        if (transform.position.y < killPoint)
            Destroy(gameObject);
    }
}
