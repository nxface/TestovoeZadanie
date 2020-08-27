using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    public enum asteroidType { Large , Medium , Small }
    public float initialForce = 100f;
    public float initialTorque = 100f;
    public asteroidType sizeType;
    public GameObject explosion;

    Rigidbody rb_asteroid;


    void Start()
    {
        rb_asteroid = GetComponent<Rigidbody>();
		if (rb_asteroid)
		{
			SetRandomForce(rb_asteroid, initialForce);
			SetRandomTorque(rb_asteroid, initialTorque);
        }
      

        if (sizeType == asteroidType.Large)
            transform.position = FindOpenPosition();
    }



   public void SetRandomForce(Rigidbody rb, float maxForce)
    {
        Vector3 randomForce = maxForce * Random.insideUnitSphere;
        rb.velocity = Vector3.zero;
        rb.AddForce(randomForce);
    }

    public void SetRandomTorque(Rigidbody rb, float maxTorque)
    {
        Vector3 randomTorque = maxTorque * Random.insideUnitSphere;
        rb.angularVelocity = Vector3.zero;
        rb.AddTorque(randomTorque);
    }

    Vector3 FindOpenPosition(int layerMask = ~0)
    {
        float x = transform.localScale.x;
        float y = transform.localScale.y;
        float collisionSphereRadius = x > y ? x : y;
     
        bool overlap = false;
        Vector3 openPosition;
        do
        { 
            openPosition = Camera.main.ViewportToWorldPoint(new Vector3(Random.value, Random.value));
            openPosition.z = 0;
            overlap = Physics.CheckSphere(openPosition, collisionSphereRadius, layerMask);
        } while (overlap);
        return openPosition;
    }

   void OnTriggerEnter(Collider otherCollider)
    {
        if (otherCollider.gameObject.tag == "Bullet" || otherCollider.gameObject.tag == "EnemyBullet")
        {
            gameObject.SetActive(false);
            switch (sizeType)
            {
                case asteroidType.Large:
                    GameManager.instance.GenerateAsteroids(asteroidType.Medium, 2,this.transform.position);
                    GameManager.instance.score+=20;
                    break;
                case asteroidType.Medium:
                    GameManager.instance.GenerateAsteroids(asteroidType.Small, 2, this.transform.position);
                    GameManager.instance.score+=50;
                    break;
                case asteroidType.Small:
                    GameManager.instance.score+=100;
                    break;
            }
            SoundManager.instance.PlayClip(EAudioClip.DESTROY_SFX,1);


            if(otherCollider.gameObject.tag == "Bullet" || otherCollider.gameObject.tag == "EnemyBullet")
                Destroy(otherCollider.gameObject); 
            if (explosion != null)
            {
                Instantiate(explosion, transform.position, transform.rotation);
            }
        }

		else if (otherCollider.gameObject.tag == "Enemy")
		{
      
            gameObject.SetActive(false);
            if (explosion != null)
            {
                Instantiate(explosion, transform.position, transform.rotation);
            }
            SoundManager.instance.PlayClip(EAudioClip.DESTROY_SFX, 1);
        }
		
    }
    void OnCollisionEnter(Collision otherCollision)
    {
        if (otherCollision.gameObject.tag == "Ship")
        {
      SpaceShipController SSC = otherCollision.gameObject.GetComponent<SpaceShipController>();
			if (!SSC.Immortal)
			{
                GameManager.instance.lives--;

                SSC.StartCoroutine(SSC.ShieldShip(0.2f));                   
                SSC.player.SetActive(true);
                switch (sizeType)
                {
                    case asteroidType.Large:
                        GameManager.instance.score += 20;
                        break;
                    case asteroidType.Medium:
                        GameManager.instance.score += 50;
                        break;
                    case asteroidType.Small:
                        GameManager.instance.score += 100;
                        break;
                }
                gameObject.SetActive(false);
                GameManager.instance.GameOver();
                if (explosion != null)
                {
                    Instantiate(explosion, transform.position, transform.rotation);
                }
                SoundManager.instance.PlayClip(EAudioClip.DESTROY_SFX, 1);
            }
           
        }
    }
    
}
