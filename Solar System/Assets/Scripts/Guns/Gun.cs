using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Rotations")]
    [SerializeField] protected float rotateSpeed = 200.0f;

    [Header("Weapon Swap")]
    [SerializeField] protected GameObject nextWeapon = null;
    [SerializeField] protected bool firstGun = false;

    [Header("Projectile")]
    [SerializeField] protected GameObject projectile = null;
    [SerializeField] protected float spawnDistance = 1.0f;
    [SerializeField] protected float bulletSpeed = 10.0f;

    Transform pointer;
    Game GM;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        if (!firstGun) 
        {
            gameObject.SetActive(false);
        }

        pointer = GameObject.FindGameObjectWithTag("Pointer").GetComponent<Transform>();
        GM = FindObjectOfType<Game>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (GM.pause == false)
        {
            Rotate();
            SwapWeapon();
            FireGun();
        }
    }

    protected virtual void Rotate()
    {
        //controls
        transform.rotation = Quaternion.LookRotation(pointer.position - transform.position, Vector3.up);

        //if (Input.GetKey(KeyCode.Alpha1)) 
        //{
        //    transform.Rotate(new Vector3(0, 0, rotateSpeed * Time.deltaTime), Space.Self);
        //}
        //
        //if (Input.GetKey(KeyCode.Alpha2))
        //{
        //    transform.Rotate(new Vector3(0, 0, -rotateSpeed * Time.deltaTime), Space.Self);
        //}
    }

    protected virtual void SwapWeapon() 
    {
        //swap weapon
        if (Input.GetKeyDown(KeyCode.W) && nextWeapon != null)
        {
            nextWeapon.SetActive(true);
            nextWeapon.transform.rotation = transform.rotation;
            gameObject.SetActive(false);
        }
    }

    protected virtual void FireGun() 
    {
        if (Input.GetKeyDown(KeyCode.Return) && projectile != null) 
        {
            //fire projectile
            Transform inst = Instantiate(projectile).transform;
            inst.parent = null;

            //set direction and velocity
            Vector3 dir = (pointer.position - transform.position).normalized;
            inst.position = transform.position + dir * spawnDistance;
            Particle3D physics = inst.GetComponent<Particle3D>();

            //do more if it is a chain weapon
            if (physics != null)
                physics.velocity = dir * bulletSpeed;
            else
            {
                Particle2D[] listPhysics = inst.GetComponentsInChildren<Particle2D>();

                int len = listPhysics.Length;
                for (int i = 0; i < len; i++) 
                {
                    listPhysics[i].velocity = dir * bulletSpeed * Mathf.Lerp(0f, 1.0f, i+1/len);
                }
            }
        }
    }
}
