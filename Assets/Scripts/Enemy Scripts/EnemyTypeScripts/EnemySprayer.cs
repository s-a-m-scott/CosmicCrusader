using UnityEngine;

public class EnemySprayer : EnemyBase
{
    public GameObject bullet; 
    public float range = 4;
    private int state=0;//0 default movement, 1 shoot wait, 2 shooting, 3 post shoot delay
    private int accuracy = 5;
    private float bulletSpread = 0.3f;
    private float preShootDelay = 0.5f;
    private float shootTime = 2;
    private float postShootDelay = 1;
    private float bulletSpMax = 3;
    private float bulletSpMin = 0.5f;
    private float stateTimer=0;
    public override void Start()
    {
        maxHealth = 100f * scaleFactor;
        maxSpeed = 3.0f;
        damage = 10f * scaleFactor;
        fireRate = 0.08f;//shoot v fast
        value = (int)(12 * scaleFactor);
        bulletSpeed = 2;

        current_health = maxHealth;//this cant easily be done in base class, which is dumb and annoying
        range += Random.Range(-(range/10),(range/10));//create variance in ranges
        base.Start();
        Next(0);
    }

    public override void Update()
    {
        base.Update();
        switch (state) {//lazy state machine
            case 0: { MoveState(); } return;
            case 1: { PreShootDelay(); } return;
            case 2: { ShootState(); } return;
            case 3: { PostShootDelay(); } return;
        }
    }

    public void MoveState() {
        //state behaviours
        MoveToPlayer(maxSpeed);
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        //state transitions
        if (Vector3.Distance(transform.position, playerObject.transform.position) < range) {
            Next(1);
        }
    }

    void PreShootDelay() {
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        float lerpCol = (stateTimer-Time.time)/preShootDelay;
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1,lerpCol,1);
        if (stateTimer < Time.time) { Next(2); }
    }

    void PostShootDelay() {
        angle = CalculateAngle();
        if (stateTimer < Time.time) { Next(0); }
    }

    public void ShootState() {
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1,0,1);//has to be set every frame, ooops

        transform.position += transform.rotation * Vector3.up * -maxSpeed * 0.7f * Time.deltaTime;//move back when shooting
        
        if (shootTimer < Time.time) {
            Quaternion aimRotation = Quaternion.AngleAxis(angle, Vector3.forward);
            Vector3 spawnOffset = aimRotation * Vector3.right * Random.Range(-bulletSpread,bulletSpread);

            bulletSpeed = UnityEngine.Random.Range(bulletSpMin,bulletSpMax);
            GameObject bulletObject = Shoot(bullet,transform.position+spawnOffset, angle + Random.Range(-accuracy,accuracy));
            bulletObject.GetComponent<EnemyBullet>().type = EnemyBullet.bulletType.round;

            shootTimer = Time.time + fireRate;
        }

        if (stateTimer < Time.time) { Next(3); }
    }

    public void Next(int _state) {
        state = _state;
        switch (_state) {//lazy state machine
            case 0: { //move
                this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1,1,1);
            } return;
            case 1: { //pre shoot
                stateTimer = Time.time + preShootDelay;
            } return;
            case 2: {//shooting
                shootTimer = Time.time + fireRate;
                stateTimer = Time.time + shootTime;
            } return;
            case 3: {//post shoot
                stateTimer = Time.time + postShootDelay;
            } return;
        }
    }
}
