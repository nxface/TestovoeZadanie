using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SpaceShipController : MonoBehaviour
{

    public float boost = 500f;
    public float speedRotate = 500f;
    public float maxSpeed = 20f;
    public GameObject bullet;
    public Transform shootPoint;
    public GameObject player;
    public bool Immortal = false;
    public float sp = 10f;
    private Vector2 cD = new Vector3(0, 1f, 0);
    private Transform tO;

    private Rigidbody rb_ship;
    private float boostInput;
    private float rotateInput;
    private bool shemeInput =true;
    private Camera cam;
    private Transform cameraPos;

    private void Awake()
    {
        tO = this.transform;
        cameraPos = GameObject.FindGameObjectWithTag("MainCamera").transform;
        rb_ship = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        boostInput = 0f;
        rotateInput = 0f;
    }

    void Update()
    {
        if (GameManager.instance.presentState == GameManager.GameState.IS_PLAYING)
        {
            if (shemeInput)
            {
                if (Input.GetKeyDown("space"))
                {
                    Shoot();
                }
                rotateInput = Input.GetAxis("Horizontal");
                boostInput = Mathf.Clamp01(Input.GetAxis("Vertical"));
            }
            else
            {
                if (Input.GetMouseButtonDown(0) || Input.GetKeyDown("space"))
                {
                    Shoot();
                }
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 objectPos = tO.position;
                Vector2 direction = mousePos - objectPos;
                direction.Normalize();
                cD = Vector2.Lerp(cD, direction, Time.deltaTime * sp);
                tO.up = cD;

				boostInput = Mathf.Clamp01(Input.GetAxis("Vertical")+Input.GetAxis("Vertical1"));
            }

        }
    }

    public void ChangeInput(bool change)
    {
        if (change)
        {
            shemeInput = true;
        }
        else
        {
            shemeInput = false;
        }


    }

    void FixedUpdate()
    {
        Move();
        Turn();
        Speed();
    }

    void Move()
    {
        Vector3 thrustForce = boostInput * boost * Time.deltaTime * transform.up;
        rb_ship.AddForce(thrustForce);
    }

    void Turn()
    {
        float turn = rotateInput * speedRotate * Time.deltaTime;
        Vector3 zTorque = transform.forward * -turn;
        rb_ship.AddTorque(zTorque);
    }

    void Speed()
    {
        rb_ship.velocity = Vector3.ClampMagnitude(rb_ship.velocity, maxSpeed);
    }

    void Shoot()
    {

        GameObject bulletClone = Instantiate(bullet, new Vector2(shootPoint.transform.position.x, shootPoint.transform.position.y), transform.rotation);

        bulletClone.GetComponent<Bullet>().Fire(shootPoint.transform.position, Quaternion.identity, transform.up * 10, Color.green, false);
        bulletClone.GetComponent<Bullet>().RemoveBullet(3f);
        SoundManager.instance.PlayClip(EAudioClip.BULLET_SFX, 0.8f);

    }
  public  IEnumerator ShieldShip(float sec2)
    {

        Immortal = true;

        for (int i = 0; i < 7; i++)
        {
            player.SetActive(false);
            yield return new WaitForSeconds(sec2);
            player.SetActive(true);
            yield return new WaitForSeconds(sec2);
        }
       
        Immortal = false;

    }

    void OnCollisionEnter(Collision otherCollision)
    {
        if (otherCollision.gameObject.tag == "EnemyBullet" && !Immortal)
        {
            GameManager.instance.lives--;

            StartCoroutine(ShieldShip(0.2f));
            GameManager.instance.GameOver();
            player.SetActive(true);
            Destroy(otherCollision.gameObject);
        }
    }


}

