using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Particle3D))]
public class ForceGravity : ForceGenerator
{
    protected Game GM;

    [HideInInspector]
    public float mass;

    // Start is called before the first frame update
    protected void Start()
    {
        GM = GameObject.Find("GameManager").GetComponent<Game>();
        GM.planets.Add(this);
        mass = GetComponent<Particle3D>().mass;
    }

    // Update is called once per frame
    public override void UpdateGenerator()
    {
        force = Vector3.zero;
        int size = GM.planets.Count;
        for (int i = 0; i < size; ++i)
        {
            if (GM.planets[i] != this)
            {
                Vector3 dist = GM.planets[i].transform.position - transform.position;
                force += GM.gravityConstant * (mass * GM.planets[i].mass / dist.sqrMagnitude) * dist.normalized;
            }
        }
    }
}
