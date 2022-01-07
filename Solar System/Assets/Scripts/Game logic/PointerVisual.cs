using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerVisual : MonoBehaviour
{
    Renderer meshRenderer = null;   //RENDERER

    // Start is called before the first frame update
    void Start()
    {
        //get mesh renderer and turn off
        meshRenderer = GetComponent<Renderer>();
        meshRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //show input
        if (Input.GetMouseButtonDown(0))
        {
            meshRenderer.enabled = true;
           transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        else if (Input.GetMouseButtonUp(0))
        {
            meshRenderer.enabled = false;
        }
    }
}
