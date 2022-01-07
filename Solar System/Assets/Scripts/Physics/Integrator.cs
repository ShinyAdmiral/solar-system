using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class Integrator
{
    //intergrate physics
    public static void Integrate(Particle2D physicsCom, bool realative)
    {
        if (realative) {
            physicsCom.transform.localPosition += (Vector3)physicsCom.velocity * Time.fixedDeltaTime;
            physicsCom.velocity += physicsCom.accumulatedAccerlation * Time.fixedDeltaTime;

            physicsCom.velocity *= Mathf.Pow(physicsCom.dampingConstant, Time.fixedDeltaTime * 50f);
        }

        else {
            physicsCom.transform.position += (Vector3)physicsCom.velocity * Time.fixedDeltaTime;
            physicsCom.velocity += physicsCom.accumulatedAccerlation * Time.fixedDeltaTime;

            physicsCom.velocity *= Mathf.Pow(physicsCom.dampingConstant, Time.fixedDeltaTime * 50f);
        }
    }
}
