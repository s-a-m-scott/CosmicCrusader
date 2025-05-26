using System.Collections;
using System;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    protected const float flashLength = 0.2f;


    protected float maxSpeed;
    protected float maxHealth;
    protected int value;
    protected float hitStun;
    protected float fireRate;
    protected float damage;
    public float Damage { get { return damage; } }
    protected float bulletSpeed;



    protected GameObject playerObject;
    public GameObject items;
    public GameObject explosion;

    public bool alive = false;
    public float scaleFactor;
    protected bool control = true;
    protected float current_health;
    protected float angle;


    protected float flashTimer = 0;
    protected float controlTimer = 0;
    protected float shootTimer = 0;
    protected AudioSource audioSource;
    public virtual void Start()
    {
        this.gameObject.GetComponent<Rigidbody>().linearDamping = 20;
        playerObject = GameObject.Find("PlayerShip");
        alive = true;
        shootTimer = Time.time + fireRate;
        if (!gameObject.CompareTag("EnemyShip")) { gameObject.tag = "EnemyShip"; }


        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = (AudioClip)Resources.Load("Sounds/enemy_alarm");
        audioSource.volume = 0.1f;
        audioSource.Play();
    }

    public virtual void Update()
    {
        if (flashTimer > Time.time)
        {
            //set colour to red
            float lerpCol = 1 - ((flashTimer - Time.time) / flashLength);//1 at 0 time left, 0 at just hit
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, lerpCol, lerpCol);
        }
        else
        {
            //reset colour
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
        }
        if (controlTimer < Time.time) { control = true; }
        if (current_health <= 0) { StartCoroutine(destroyActor(true)); }
        if (CheckTooFarPlayer()) { StartCoroutine(destroyActor(false, true)); }
    }

    public virtual IEnumerator destroyActor(bool giveMoney, bool outOfRange = false)
    {
        alive = false;
        GetComponent<Collider>().enabled = false;

        if (!outOfRange) Instantiate(explosion, transform.position, transform.rotation);
        if (giveMoney) spawnCoins();
        Destroy(this.gameObject);
        yield return new WaitForFixedUpdate();
    }

    public virtual IEnumerator takeDamage(float damage)
    {
        current_health -= damage;
        flashTimer = Time.time + flashLength;//flash red to indicate dmg
        control = false;
        controlTimer = Time.time + hitStun;
        //AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = (AudioClip)Resources.Load("Sounds/enemy_hit");
        audioSource.volume = 0.1f;
        audioSource.Play();
        yield return new WaitForFixedUpdate();
    }


    public void spawnCoins()
    {
        float total = value;
        int coinVal;
        while (total > 0)
        {
            coinVal = Convert.ToInt16(UnityEngine.Random.Range(1, Math.Min(total, 10)));
            total -= coinVal;
            GameObject item = Instantiate(items, transform.position, transform.rotation);
            item.GetComponent<ItemAction>().value = coinVal;
        }
    }
    public float CalculateAngle()
    {
        float _angle = Vector3.Angle(playerObject.transform.position - transform.position, Vector3.up);
        //flip angle around y axis
        if (playerObject.transform.position.x > transform.position.x)
        {
            _angle = 360 - _angle;
        }
        return _angle;
    }

    public void MoveToPlayer(float speed)
    {
        if (alive && control)
        {
            angle = CalculateAngle();
            transform.position += transform.rotation * Vector3.up * speed * Time.deltaTime;
        }
    }

    public GameObject Shoot(GameObject bullet, Vector3 pos, float _angle)
    {
        // spawns a bullet at a given angle/pos
        Quaternion aimRotation = Quaternion.AngleAxis(_angle, Vector3.forward);
        GameObject bulletObject = Instantiate(bullet, pos, aimRotation);
        bulletObject.GetComponent<EnemyBullet>().Damage = damage;
        bulletObject.GetComponent<EnemyBullet>().Speed = bulletSpeed;
        return bulletObject;
    }

    public bool CheckTooFarPlayer()//returns true if out of range
    {
        if (Vector3.Distance(playerObject.transform.position, this.gameObject.transform.position) > 15)
        {
            return true;
        }
        else return false;
    }
}

