using UnityEngine;

public class EnemyFourShooter : EnemyBase
{
    public GameObject bullet;
    private int rotSpeed = 50;
    private float shootAngle=0;
    public override void Start()
    {
        maxHealth = 35f * scaleFactor;
        maxSpeed = 0.5f;
        damage = 5f * scaleFactor;
        fireRate = 2f;
        value = (int)(3 * scaleFactor);
        bulletSpeed = 5;

        current_health = maxHealth;//this cant easily be done in base class, which is dumb and annoying
        base.Start();
        shootTimer += Random.Range(0,fireRate);//offset fire rates when spawned simultaneously
        shootAngle= Random.Range(0,360);
    }

    public override void Update()
    {
        base.Update();
        MoveToPlayer(maxSpeed);

        shootAngle += rotSpeed*Time.deltaTime;
        transform.rotation = Quaternion.AngleAxis(shootAngle, Vector3.forward);

        float lerpCol = (shootTimer-Time.time)/fireRate;
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1,lerpCol,1);
        if (shootTimer < Time.time) {
            for (int i=0; i<4;i++) {
                Shoot(bullet,transform.position, shootAngle+(i*90));
            }
            shootTimer = Time.time+fireRate;
        }
    }

}
