using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseAim3D : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        Plane plane = new Plane(Vector3.left, 0);

        float distance;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(ray, out distance))
        {
            transform.position = ray.GetPoint(distance);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, 0.5f);
    }
}
