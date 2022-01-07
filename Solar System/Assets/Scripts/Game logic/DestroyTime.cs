using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTime : MonoBehaviour
{
    [SerializeField] float time = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Die(time));
    }

    IEnumerator Die(float _time) {
        yield return new WaitForSeconds(_time);
        Destroy(gameObject);
    }
}
