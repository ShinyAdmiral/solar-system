using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGun : Gun
{
    [SerializeField] float spread = 3f;
    override protected void FireGun() 
    {
        if (Input.GetKeyDown(KeyCode.Return) && projectile != null) 
        {
            Vector2 initDir = new Vector2(Mathf.Cos(transform.eulerAngles.z * Mathf.Deg2Rad), Mathf.Sin(transform.eulerAngles.z * Mathf.Deg2Rad));
            float initCalc = (Mathf.PI / spread) / 2;

            for (int i = -2; i < 2; i++)
            {
                Transform inst = Instantiate(projectile).transform;
                inst.parent = null;
                inst.position = (Vector2)transform.position + initDir * spawnDistance;

                float extraCalc = initCalc + (Mathf.PI / spread) * i;

                Vector2 dir = new Vector2(Mathf.Cos(transform.eulerAngles.z * Mathf.Deg2Rad + extraCalc), Mathf.Sin(transform.eulerAngles.z * Mathf.Deg2Rad + extraCalc));

                inst.GetComponent<Particle2D>().velocity = dir * bulletSpeed;
            }
        }
    }
}
