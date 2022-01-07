using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollisionManager : MonoBehaviour
{
    public List<ParticleCollider> particleList = new List<ParticleCollider>();
    public List<int> dupes = new List<int>();
    ColliderPlane[] planes;
    Game GM;

    public Text collisionUI;
    public int collisionCount = 0;
    int lastCollisionCount = 0;

    //variables used for calculations
    Vector3 midline,
            movePerMass,
            normal,
            velocityPerMass;

    Particle2D physics1,
                physics2;

    float penatration,
        size,
        totalIMass,
        seperatingVelocity,
        newSeperatingVelocity,
        deltaVelocity,
        impulseVelocity;

    Vector3 gizmoPoint = Vector3.zero;


    // Start is called before the first frame update
    void Start()
    {
        //colliderList = FindObjectsOfType<ParticleCollider>();
        //
        //int length = colliderList.Length;

        //while (length > 0) {
        //    length--;
        //    particles.Add(colliderList[length]);
        //}
        GM = GetComponent<Game>();
        planes = FindObjectsOfType<ColliderPlane>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        lastCollisionCount = collisionCount;
        collisionCount = 0;
    }

    private void Update()
    {
        if (collisionUI != null)
            collisionUI.text = "Collisions\nChecks\n" + lastCollisionCount.ToString();
    }

    public bool ResolveAllCollidersPartitions(ParticleCollider checkThis, ref List<ParticleCollider> colliderList)
    {
        bool collided = false;
        int length = colliderList.Count;

        //check every combination of particles for collision
        for (int i = 0; i < length; i++) 
        {
            //check for dupes
            if (colliderList[i] != checkThis) 
            {
                //do a collision check
                collided = collided || CollisionCheck(colliderList[i], checkThis);

                //dupes.Add(colliderList[i].GetInstanceID());
                //dupes.Add(checkThis.GetInstanceID());
            }
        }

        return collided;
    }

    public bool ResolveAllCollidersPartitionPlane(ParticleCollider checkThis) 
    {
        bool collided = false;

        for (int i = 0; i < planes.Length; i++) {
            collided = collided || CollisionCheck(planes[i], checkThis);
        }

        return collided;
    }

    public bool ResolveAllColliders(ParticleCollider checkThis)
    {
        bool collided = false;
        int length = particleList.Count;

        //check every combination of particles for collision
        for (int i = 0; i < length; i++)
        {

            //check for dupes
            if (particleList[i] != checkThis)

                //do a collision check
                collided = collided || CollisionCheck(particleList[i], checkThis);
        }

        return collided;
    }

    bool CollisionCheck(ParticleCollider collider1, ParticleCollider collider2)
    {
        collisionCount++;

        //validate 
        if (collider1 == null || collider2 == null)
            return false;

        //determine collision type

        if (collider1.collisionType == CollisionType.SPHERE && collider2.collisionType == CollisionType.SPHERE)
        {
            return SphereToSphereCollisionCalc(collider1.GetComponent<ColliderSphere>(), collider2.GetComponent<ColliderSphere>());
        }

        else if (collider1.collisionType == CollisionType.PLANE && collider2.collisionType == CollisionType.SPHERE)
        {
            return SphereToPlaneCollisionCalc(collider1.GetComponent<ColliderPlane>(), collider2.GetComponent<ColliderSphere>());
        }

        else if (collider1.collisionType == CollisionType.AABB && collider2.collisionType == CollisionType.SPHERE)
        {
            return SphereToAABBCollision(collider1.GetComponent<ColliderAABB>(), collider2.GetComponent<ColliderSphere>());
        }

        else if (collider1.collisionType == CollisionType.SPHERE && collider2.collisionType == CollisionType.AABB)
        {
            return SphereToAABBCollision(collider2.GetComponent<ColliderAABB>(), collider1.GetComponent<ColliderSphere>());
        }


        return false;
    }

    bool SphereToSphereCollisionCalc(ColliderSphere sphere1, ColliderSphere sphere2)
    {
        //check for collision
        midline = sphere1.transform.position - sphere2.transform.position;
        size = midline.magnitude;

        penatration = sphere1.radius + sphere2.radius;

        //was there a collision?
        if (penatration > size)
        {
            //get physics component
            physics1 = sphere1.GetComponent<Particle2D>();
            physics2 = sphere2.GetComponent<Particle2D>();

            //penatration adjustment
            penatration -= size;

            //normal calc
            normal = midline / Mathf.Max(size, 0.00001f);

            //physics calculation
            totalIMass = physics1.invMass + physics2.invMass;
            movePerMass = normal * penatration / totalIMass;

            //interpenatration movement
            sphere1.transform.position += (Vector3)movePerMass * physics1.invMass;
            sphere2.transform.position -= (Vector3)movePerMass * physics2.invMass;

            //new velocity calcs
            seperatingVelocity = Vector3.Dot(physics1.velocity - physics2.velocity, normal);
            newSeperatingVelocity = -seperatingVelocity * (sphere1.restitution + sphere2.restitution) * 0.5f;
            deltaVelocity = newSeperatingVelocity - seperatingVelocity;
            impulseVelocity = deltaVelocity / totalIMass;
            velocityPerMass = normal * impulseVelocity;

            //change velocity
            physics1.velocity += velocityPerMass * physics1.invMass;
            physics2.velocity -= velocityPerMass * physics2.invMass;

            //for scoring
            if (physics2.gameObject.tag == "Target" || physics1.gameObject.tag == "Target")
            {
                GM.increaseScore();
            }

            return true;
        }

        return false;
    }

    bool SphereToPlaneCollisionCalc(ColliderPlane plane, ColliderSphere sphere)
    {
        //get midline
        float distance = Vector3.Dot(plane.normal, sphere.transform.position);
        float offset = Vector3.Dot(plane.normal, plane.transform.position);

        //check if we are colliding
        size = distance - offset;
        penatration = sphere.radius;

        if (distance - offset < sphere.radius) 
        {
            //get physics component
            physics1 = sphere.GetComponent<Particle2D>();
            physics2 = plane.GetComponent<Particle2D>();

            //penatration adjustment
            penatration -= size;

            //normal calc
            normal = plane.normal;

            //physics calculation
            totalIMass = physics1.invMass + physics2.invMass;
            movePerMass = normal * penatration / totalIMass;

            //interpenatration movement
            sphere.transform.position += (Vector3)movePerMass * physics1.invMass;
            //plane.transform.position -= (Vector3)movePerMass * physics2.invMass;

            //new velocity calcs
            seperatingVelocity = Vector3.Dot(physics1.velocity - physics2.velocity, normal);
            newSeperatingVelocity = -seperatingVelocity * sphere.restitution;
            deltaVelocity = newSeperatingVelocity - seperatingVelocity;
            impulseVelocity = deltaVelocity / totalIMass;
            velocityPerMass = normal * impulseVelocity;

            //change velocity
            physics1.velocity += velocityPerMass * physics1.invMass;

            return true;
        }

        return false;

    }


    bool SphereToAABBCollision(ColliderAABB box, ColliderSphere sphere)
    {
        //get closestpoint to sphere
        float xPos = Mathf.Max(box.bottomLeftBack.x, Mathf.Min(sphere.transform.position.x, box.topRightFront.x));
        float yPos = Mathf.Max(box.bottomLeftBack.y, Mathf.Min(sphere.transform.position.y, box.topRightFront.y));
        float zPos = Mathf.Max(box.bottomLeftBack.z, Mathf.Min(sphere.transform.position.z, box.topRightFront.z));

        //check if point is inside sphere
        Vector3 SphereToPoint = new Vector3(xPos - sphere.transform.position.x,
                                            yPos - sphere.transform.position.y,
                                            zPos - sphere.transform.position.z);

        //get the square of the dist
        float dist = SphereToPoint.x * SphereToPoint.x + 
                            SphereToPoint.y * SphereToPoint.y +
                            SphereToPoint.z * SphereToPoint.z;

        if (dist < sphere.radius * sphere.radius && dist != 0) 
        {
            //get the square root of dist (true distance)
            dist = Mathf.Sqrt(dist);

            //move object
            normal = SphereToPoint / dist;
            movePerMass = normal * (sphere.radius - dist);
            sphere.transform.position -= movePerMass;

            //new velocity
            physics1 = sphere.GetComponent<Particle2D>();

            //new velocity calcs
            totalIMass = physics1.invMass + box.GetComponent<Particle2D>().invMass;
            seperatingVelocity = Vector3.Dot(physics1.velocity, normal);
            newSeperatingVelocity = -seperatingVelocity * sphere.restitution;
            deltaVelocity = newSeperatingVelocity - seperatingVelocity;
            impulseVelocity = deltaVelocity / totalIMass;
            velocityPerMass = normal * impulseVelocity;

            physics1.velocity += velocityPerMass;

            //for scoring
            if (box.gameObject.tag == "Target")
            {
                GM.increaseScore();
            }

            return true;
        }

        return false;
    }

    public void processCollisions(ref List<ParticleCollider> cellParticles, ref List<ParticleCollider> neighborParticles)
    {
        for (int k = 0; k < cellParticles.Count; k++)
        {
                ResolveAllCollidersPartitions(cellParticles[k], ref neighborParticles);
                //processedParticles.Add(cellParticles[k]);
                //collision happened
        }
        //!dupes.Contains(cellParticles[k].GetInstanceID())
        //dupes.Clear();
    }

    public void processCollisionsPlanes(ref List<ParticleCollider> cellParticles) 
    {
        for (int k = 0; k < cellParticles.Count; k++) {
            ResolveAllCollidersPartitionPlane(cellParticles[k]);
        }
    }

    public void clearCollisionCount()
    {
        collisionCount = 0;
    }

    public void clearDupes() {
        dupes.Clear();
    }
}
