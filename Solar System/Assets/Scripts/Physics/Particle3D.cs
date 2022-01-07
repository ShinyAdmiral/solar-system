using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle3D : Particle2D{
    //update physics
    protected override void FixedUpdate() {
        //get acculmative force
        if (forceGeneratorManager != null)
        {
            forceGeneratorManager.UpdateGenerators();
            accumulatedAccerlation += forceGeneratorManager.GetAcceleration(invMass);
        }

        //integrate
        accumulatedAccerlation += gravity;

        //collsion
        if (hitBox != null && GM.partitionsActive == false)
        {
            bool collided = CM.ResolveAllColliders(hitBox);

            //add to the score
            //if (collided && tag == "Target")
            //{
            //    GM.increaseScore();
            //}
        }

        //integrate
        if (GM.pause == false)
        {
            Integrator.Integrate(physics, realative);
        }
        accumulatedAccerlation = Vector3.zero;
    }
}
