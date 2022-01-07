using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        //find Game Object
        var bulletList = GameObject.FindGameObjectsWithTag("bullet");
        var hardBulletList = GameObject.FindGameObjectsWithTag("hardBullet");

        int length = bulletList.Length;

        //destory if in range (destory other too)
        for (var i = 0; i < length; i++) 
        {
            float dist = Vector2.Distance(bulletList[i].transform.position, transform.position);
            if (dist < transform.localScale.x * 0.5 + bulletList[i].transform.localScale.x * 0.5)
            {
                GameObject.Find("GameManager").GetComponent<Game>().increaseScore();
                Destroy(bulletList[i]);
                Destroy(gameObject);
                return;
            }
        }

        length = hardBulletList.Length;

        //destory if in range
        for (var i = 0; i < length; i++)
        {
            float dist = Vector2.Distance(hardBulletList[i].transform.position, transform.position);
            if (dist < transform.localScale.x * 0.5 + hardBulletList[i].transform.localScale.x * 0.5)
            {
                GameObject.Find("GameManager").GetComponent<Game>().increaseScore();
                Destroy(gameObject);
            }
        }

    }
}
