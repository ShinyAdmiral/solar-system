using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour{
    [SerializeField] Vector3 spawnSize = Vector3.one;
    [SerializeField] float spawnTimer = 0.5f;
    [SerializeField] GameObject target = null;
    [SerializeField] GameObject particle;
    public int score = 0;
    public bool stop = false;
    public bool pause = false;
    public bool partitionsActive = true;
    [SerializeField] int startingParticles = 1;
    [SerializeField] Vector3 speedRange = Vector3.one;
    [SerializeField] Text text;

    [HideInInspector]
    public List<ForceGravity> planets = new List<ForceGravity>();

    public float gravityConstant = 0.0000000000667430f;

    // Start is called before the first frame update
    void Start(){
        //StartCoroutine(spawn(spawnTimer));

        if (text != null)
            text.text = "Score\n" + score.ToString();

        //dont destory on load
        DontDestroyOnLoad(gameObject);

        //spawn particles with random position and velocity
        for (int i = 0; i < startingParticles; i++)
            randomSpawn();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
            partitionsActive = !partitionsActive;
        
        if (Input.GetKeyDown(KeyCode.Space))
            pause = !pause;
    }

    IEnumerator spawn(float time) {
        yield return new WaitForSeconds(time);

        if (!stop) {
            GameObject inst = Instantiate(target);
            inst.transform.position = new Vector2(Random.Range(0f, spawnSize.x) + transform.position.x - spawnSize.x * 0.5f, transform.position.y);
            inst.transform.parent = null;
            StartCoroutine(spawn(spawnTimer));
        }
    }

    void randomSpawn()
    {
        GameObject inst = Instantiate(particle);
        Particle3D component = inst.GetComponent<Particle3D>(); 

        component.transform.position = new Vector3(Random.Range(-12, 7), Random.Range(-2, 8), Random.Range(-15, 10));
        component.velocity = new Vector3(Random.Range(-speedRange.x, speedRange.x), Random.Range(-speedRange.y, speedRange.y), Random.Range(-speedRange.z, speedRange.z));
    }

    public void increaseScore() 
    {
        score += 100;

        if (text != null)
            text.text = "Score\n" + score.ToString();
    }
}
