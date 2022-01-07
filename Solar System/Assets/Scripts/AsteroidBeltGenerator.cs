using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidBeltGenerator : MonoBehaviour
{
    [SerializeField] GameObject asteroidPrefab;
    [SerializeField] int asteroidCount;
    [SerializeField] float asteroidDistMax;
    [SerializeField] float asteroidDistMin;
    [SerializeField] float sunMass = 1000000000;

    // Start is called before the first frame update
    void Start()
    {
        float G = GameObject.FindObjectOfType<Game>().gravityConstant;

        float radRatio = 2 * Mathf.PI / asteroidCount;

        for (int i = 0; i < asteroidCount; ++i) 
        {
            Particle3D physics = Instantiate(asteroidPrefab).GetComponent<Particle3D>();
            //physics.transform.parent = null;

            float rad = radRatio * i;
            Vector2 position2D = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
            float randomRange = Random.Range(asteroidDistMin, asteroidDistMax);

            physics.transform.position = new Vector3(position2D.y * randomRange, 0, position2D.x * randomRange);

            float speed = Mathf.Sqrt(G * sunMass / randomRange);

            physics.velocity = new Vector3(position2D.x, 0, -position2D.y) * speed;
        }
    }
}
