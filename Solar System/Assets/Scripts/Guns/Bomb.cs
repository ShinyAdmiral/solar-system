using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] float timeTimeExplosion = 0.5f;
    [SerializeField] int explosonNumber = 8;
    [SerializeField] GameObject bulletInst = null;
    [SerializeField] float bulletSpeed = 10f;
    private void Start()
    {
        StartCoroutine(explosion(timeTimeExplosion));
    }

    IEnumerator explosion(float time) 
    {
        yield return new WaitForSeconds(time);

        for (var i = 0; i < explosonNumber; i++) 
        {
            Transform inst = Instantiate(bulletInst).transform;
            inst.parent = null;
            Vector2 dir = new Vector2(Mathf.Cos(2 * Mathf.PI/ explosonNumber * i), Mathf.Sin(2 * Mathf.PI / explosonNumber * i));
            inst.position = (Vector2)transform.position;
            inst.GetComponent<Particle2D>().velocity = dir * bulletSpeed;
        }

        Destroy(gameObject);
    }
}
