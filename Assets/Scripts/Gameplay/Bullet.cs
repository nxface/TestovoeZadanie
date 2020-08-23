using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int speed = 800;

    public Vector3 dir ;

    Rigidbody rb_bullet;




    public void Fire(Vector3 position, Quaternion rotation, Vector3 velocity,Color color,bool  isEnemy)
    {
        transform.position = position;
        transform.rotation = rotation;
        rb_bullet.velocity = velocity;
        gameObject.GetComponent<Renderer>().material.color = color;
		if (isEnemy)
		{
            gameObject.tag = "EnemyBullet";
		}

    }
    private void Awake()
    {
        rb_bullet = GetComponent<Rigidbody>();
        
      //  dir = gameObject.transform.up;
    }
    void Start()
    {
         //   GetComponent<Rigidbody>().AddForce(dir * speed);
       
    }

  

    public void RemoveBullet(float time)
    {
        Destroy(gameObject, time);
    }
    
}


