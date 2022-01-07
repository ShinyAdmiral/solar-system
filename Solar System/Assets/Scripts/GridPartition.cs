using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPartition : MonoBehaviour
{
    [SerializeField] Vector3 gridSize = Vector3.one;
    [SerializeField] Vector3 cellSize = Vector3.one;
    List<ParticleCollider>[] partitionList;

    //hemispheres for planes                                          
    [SerializeField] ColliderPlane xy_back;                           
    [SerializeField] ColliderPlane xy_front;                          
    [SerializeField] ColliderPlane xz_back;                           
    [SerializeField] ColliderPlane xz_front;                          
    [SerializeField] ColliderPlane zy_back;                           
    [SerializeField] ColliderPlane zy_front;                          
                                                                      
    protected CollisionManager CM;
    Game gpGame;
    int z;                                                            

    [SerializeField] ParticleCollider testParticle = null;

    Vector3 inverseCellSize = Vector3.one;

    // Start is called before the first frame update
    void Awake()
    {
        gridSize.x = Mathf.Floor(gridSize.x);
        gridSize.y = Mathf.Floor(gridSize.y);
        gridSize.z = Mathf.Floor(gridSize.z);

        //initialize list
        partitionList = new List<ParticleCollider>[(int)gridSize.x * (int)gridSize.y * (int)gridSize.z];

        for (int i = 0; i < partitionList.Length; i++)
        {
            partitionList[i] = new List<ParticleCollider>();
        }

        inverseCellSize = new Vector3(1 / cellSize.x, 1 / cellSize.y, 1 / cellSize.z);
        z = (int)gridSize.x * (int)gridSize.y;
        CM = FindObjectOfType<CollisionManager>();
        gpGame = FindObjectOfType<Game>();
        //set up initial
        //InitStartUp();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.C) && gpGame.pause == false) {

            for (int j = 0; j < partitionList.Length; j++) {
                int length = partitionList[j].Count;
                //check every combination of particles for collision
                for (int i = length - 1; i > -1; i--) {
                    if (partitionList[j][i].tag != "Target" && partitionList[j][i] != null) {
                        ParticleCollider pc = partitionList[j][i];
                        if (pc.collisionType != CollisionType.PLANE)
                            Destroy(pc.gameObject);
                    }
                }

                partitionList[j].Clear();
            }

            ParticleCollider[] newColliders = FindObjectsOfType<ParticleCollider>();

            for (int i = 0; i < newColliders.Length; i++)
            {
                if (newColliders[i].gameObject.tag == "Target")
                    Add(newColliders[i]);
            }

            //InitStartUp();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        //checked each partition and make sure objects haven't moved
        //do collision checks

        if (gpGame.pause == false && gpGame.partitionsActive)
        {
            //CM.clearCollisionCount();
            for (int i = 0; i < partitionList.Length; i++)
            {
                int k;

                //main partition
                for (k = partitionList[i].Count - 1; k > -1; k--)
                {
                    ParticleCollider temp = partitionList[i][k];
                    if (getPos(ref temp) != i && temp.collisionType != CollisionType.PLANE)
                    {
                        //Remove(temp);
                        //quick
                        partitionList[i].RemoveAt(k);
                        Add(temp);
                    }
                }

                //process own collision and planes
                CM.processCollisions(ref partitionList[i], ref partitionList[i]);
                CM.processCollisionsPlanes(ref partitionList[i]);

                //check neighbors
                if (partitionList[i].Count > 0)
                {
                    //partition neighbors - x axis
                    if (i + 1 < partitionList.Length)
                        CM.processCollisions(ref partitionList[i], ref partitionList[i + 1]);

                    if (i - 1 > 0)
                        CM.processCollisions(ref partitionList[i], ref partitionList[i - 1]);

                    //y axis
                    if (i + (int)gridSize.x < partitionList.Length)
                        CM.processCollisions(ref partitionList[i], ref partitionList[i + (int)gridSize.x]);

                    if (i - (int)gridSize.x > 0)
                        CM.processCollisions(ref partitionList[i], ref partitionList[i - (int)gridSize.x]);


                    //z axis
                    if (i + z < partitionList.Length)
                        CM.processCollisions(ref partitionList[i], ref partitionList[i + z]);

                    if (i - z > 0)
                        CM.processCollisions(ref partitionList[i], ref partitionList[i - z]);


                    //corners
                    if (i + 1 + (int)gridSize.y < partitionList.Length)
                        CM.processCollisions(ref partitionList[i], ref partitionList[i + 1 + (int)gridSize.y]);


                    if (i - 1 + (int)gridSize.y > 0 && i - 1 + (int)gridSize.y < partitionList.Length)
                        CM.processCollisions(ref partitionList[i], ref partitionList[i - 1 + (int)gridSize.y]);

                    if (i + 1 - (int)gridSize.y < partitionList.Length && i + 1 - (int)gridSize.y > 0)
                        CM.processCollisions(ref partitionList[i], ref partitionList[i + 1 - (int)gridSize.y]);

                    if (i - 1 - (int)gridSize.y > 0)
                        CM.processCollisions(ref partitionList[i], ref partitionList[i - 1 - (int)gridSize.y]);

                    if (i + 1 - z < partitionList.Length && i + 1 - z > 0)
                        CM.processCollisions(ref partitionList[i], ref partitionList[i + 1 - z]);

                    if (i - 1 + z > 0 && i - 1 + z < partitionList.Length)
                        CM.processCollisions(ref partitionList[i], ref partitionList[i - 1 + z]);

                    if (i + 1 + z < partitionList.Length)
                        CM.processCollisions(ref partitionList[i], ref partitionList[i + 1 + z]);

                    if (i - 1 - z > 0)
                        CM.processCollisions(ref partitionList[i], ref partitionList[i - 1 - z]);
                }

            
            }

            //CM.clearDupes();
        }
        
    }

    int getPos(ref ParticleCollider particle) 
    {
        Vector3 localPos = particle.transform.position - transform.position;
        localPos = new Vector3(localPos.x * inverseCellSize.x, localPos.y * inverseCellSize.y, localPos.z * inverseCellSize.z);

        return getVectorArrayLoc(localPos);
    }

    int getVectorArrayLoc(Vector3 pos) 
    {
        return (int)pos.x + (int)((int)pos.y * gridSize.x) + (int)((int)pos.z * gridSize.y * gridSize.x);
    }

    //used for debugging
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;

        for (int x = 0; x < gridSize.x; x++)
            for (int y = 0; y < gridSize.y; y++)
                for (int z = 0; z < gridSize.z; z++)
                {
                    if (testParticle != null)
                    {
                        int testPos = getPos(ref testParticle);

                        if (testPos == getVectorArrayLoc(new Vector3(x, y, z)))
                        {
                            Gizmos.color = Color.green;
                            Gizmos.DrawWireSphere(new Vector3(x * cellSize.x, y * cellSize.y, z * cellSize.z) + cellSize * 0.5f + transform.position, cellSize.x);
                        }
                        else
                            Gizmos.color = Color.blue;

                   
                    }

                    Gizmos.DrawWireCube(new Vector3(x * cellSize.x, y * cellSize.y, z * cellSize.z) + cellSize * 0.5f + transform.position, cellSize);
                }
    }

    void InitStartUp() 
    {
        for (int x = 0; x < gridSize.x; x++)
            for (int y = 0; y < gridSize.y; y++)
                partitionList[getVectorArrayLoc(new Vector3(x, y, 0))].Add(xy_back);

        for (int x = 0; x < gridSize.x; x++)
            for (int y = 0; y < gridSize.y; y++)
                partitionList[getVectorArrayLoc(new Vector3(x, y, gridSize.z - 1))].Add(xy_front);

        for (int z = 0; z < gridSize.z; z++)
            for (int x = 0; x < gridSize.x; x++)
                partitionList[getVectorArrayLoc(new Vector3(x, 0, z))].Add(xz_back);


        for (int z = 0; z < gridSize.z; z++)
            for (int x = 0; x < gridSize.x; x++)
                partitionList[getVectorArrayLoc(new Vector3(x, gridSize.y - 1, z))].Add(xz_front);

        for (int z = 0; z < gridSize.z; z++)
            for (int y = 0; y < gridSize.y; y++)
                partitionList[getVectorArrayLoc(new Vector3(0, y, z))].Add(zy_back);


        for (int z = 0; z < gridSize.z; z++)
            for (int y = 0; y < gridSize.y; y++)
                partitionList[getVectorArrayLoc(new Vector3(gridSize.x - 1, y, z))].Add(zy_front);


    }

    public void Add(ParticleCollider newCollider) 
    {
        if (newCollider.collisionType != CollisionType.PLANE)
        {
            int location = Mathf.Clamp(getPos(ref newCollider), 0, partitionList.Length - 1);
            partitionList[location].Add(newCollider);
        }
    }

    public void Remove(ParticleCollider newCollider)
    {
        int location = getPos(ref newCollider);
        partitionList[location].Remove(newCollider);
    }
}
