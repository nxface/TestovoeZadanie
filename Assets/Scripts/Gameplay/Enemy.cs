using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class Enemy : MonoBehaviour

{


    public Transform shootPoint;
    public GameObject target;
    public GameObject bulletPrefab;
    public float enemySpeed = 1f;
    public int dir;
    private Rigidbody rb_enemy;
    private void Awake()
    {
        rb_enemy = GetComponent<Rigidbody>();
        target =  GameObject.FindGameObjectWithTag("Ship");
        dir = Random.Range(0, 2);
        dir = (dir > 0) ? 1 : -1;  
    }
    private void Start()
    {
       
        StartCoroutine("Shoot");
        Launch();
    }
	private void Update()
	{
        rb_enemy.AddForce(new Vector3(dir, 0)*50*Time.deltaTime);
    }


    void RotateToTarget()
	{
        Vector3 diff = target.transform.position - transform.position;
        diff.Normalize();
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);


       
    }
	private void Launch()
    {
        rb_enemy.velocity = Random.insideUnitCircle * enemySpeed;
    }


    void OnTriggerEnter(Collider otherCollider)
    {
        if (otherCollider.gameObject.tag == "Bullet" || otherCollider.gameObject.tag == "Ship" || otherCollider.gameObject.tag == "Asteroid")
        {

            Destroy(gameObject);
            GameManager.instance.score += 200;
            GameManager.instance.EnemyDeath();
            SoundManager.instance.PlayClip(EAudioClip.ENEMYDESTROY_SFX, 1);
            if (otherCollider.gameObject.tag == "Ship")
                {
                    SpaceShipController SSC = otherCollider.gameObject.GetComponent<SpaceShipController>();
                    if (!SSC.Immortal)
                    {
                        GameManager.instance.lives--;

                        SSC.StartCoroutine(SSC.ShieldShip(0.2f));
                        GameManager.instance.GameOver();
                        SSC.player.SetActive(true);
                    }

                   

                    if (otherCollider.gameObject.tag == "Bullet")
                        Destroy(otherCollider.gameObject);

                }
            }
        }
    



    protected  IEnumerator Shoot()
    {
		while (true)
        {
            yield return new WaitForSeconds(2f);
            RotateToTarget();
            GameObject bulletClone =  Instantiate(bulletPrefab, shootPoint);
            bulletClone.GetComponent<Bullet>().Fire(shootPoint.position, Quaternion.identity, transform.up*10, Color.red,true);
            bulletClone.GetComponent<Bullet>().RemoveBullet(3f);
            SoundManager.instance.PlayClip(EAudioClip.WEAPONENEMY_SFX, 0.5f);
           
        }

    }

}
