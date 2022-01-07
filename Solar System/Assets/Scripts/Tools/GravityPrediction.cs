using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityPrediction : MonoBehaviour
{
    [SerializeField] bool run = false;
    [SerializeField] Color pathColor = Color.red;
    
    [Range(0, 50000)]
    [SerializeField] int iterations = 100;

    [Range(0, 100)]
    [SerializeField] int renderFactor = 10;

    [SerializeField] ForceGravity target = null;
    [SerializeField] ForceGravity relativeTo = null;

    int targetID = -1;

    List<Vector3> listPoints = new List<Vector3>();

    private void Update()
    {
        float fixedRatio = 0.02f;

        if (target == null) 
        {
            listPoints.Clear();
            return;
        }

        if (run) 
        {
            listPoints.Clear();

            Game GM = GameObject.Find("GameManager").GetComponent<Game>();
            ForceGravity[] planets = GameObject.FindObjectsOfType<ForceGravity>();
            Particle3D[] otherMasses = new Particle3D[planets.Length];
            Vector3[] currentVel = new Vector3[planets.Length];
            Vector3[] currentPos = new Vector3[planets.Length];
            Vector3 accumulatedForce;

            for (int i = 0; i < planets.Length; i++) 
            {
                otherMasses[i] = planets[i].GetComponent<Particle3D>();
                currentPos[i] = otherMasses[i].transform.position;
                currentVel[i] = otherMasses[i].velocity;

                if (planets[i] == relativeTo)
                    targetID = i;
            }

            listPoints.Add(target.transform.position);

            for (int i = 0; i < iterations; i++) 
            {
                for (int h = 0; h < planets.Length; h++)
                {
                    accumulatedForce = Vector3.zero;
                    
                    for (int j = 0; j < planets.Length; j++)
                    {
                        if (planets[h] == planets[j])
                            continue;

                        Vector3 dist = currentPos[j] - currentPos[h];
                        accumulatedForce += GM.gravityConstant * (otherMasses[h].mass * otherMasses[j].mass / dist.sqrMagnitude) * dist.normalized;
                    }

                    Vector3 acceleration = accumulatedForce / otherMasses[h].mass;

                    currentPos[h] += currentVel[h] * fixedRatio;
                    currentVel[h] += acceleration * fixedRatio;
                    currentVel[h] *= otherMasses[h].dampingConstant;

                    if (planets[h] == target && i % renderFactor == renderFactor-1) 
                    {
                        if (targetID != -1)
                            listPoints.Add(currentPos[h] - currentPos[targetID] + relativeTo.transform.position);
                        else
                            listPoints.Add(currentPos[h]);
                    }
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (listPoints.Count > 1)
        {
            Gizmos.color = pathColor;
            
            for (int i = 1; i < listPoints.Count; i++)
            {
                Gizmos.DrawLine(listPoints[i - 1], listPoints[i]);
            }
        }
    }
}
