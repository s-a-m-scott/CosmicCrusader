using UnityEngine;

public class EnemyChaser : EnemyBase
{
    public GameObject bullet; 
    public float range = 2;
    private int state=0;//0 default movement, 1 shooting, 2 shoot delay
    private int shoot_delay = 2;
    private float bulletSeparation = 0.2f;
    public override void Start()
    {
        maxHealth = 60f * scaleFactor;
        maxSpeed = 4.0f;
        damage = 20f * scaleFactor;
        fireRate = 1;
        value = (int)(20f * scaleFactor);
        bulletSpeed = 15;

        current_health = maxHealth;//this cant easily be done in base class, which is dumb and annoying
        range += Random.Range(-(range/10),(range/10));//create variance in ranges
        base.Start();
        Next(0);
    }

    public override void Update()
    {
        base.Update();
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        switch (state) {//lazy state machine
            case 0: { MoveState(); } return;
            case 1: { ShootState(); } return;
            case 2: { ShootDelay(); } return;
        }
    }

    public void MoveState() {
        //state behaviours
        MoveToPlayer(maxSpeed);

        //state transitions
        if (Vector3.Distance(transform.position, playerObject.transform.position) < range) {
            Next(1);
        }
    }

    public void ShootState() {
        angle = CalculateAngle();
        float lerpCol = (shootTimer-Time.time)/fireRate;
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1,lerpCol,1);

        if (shootTimer < Time.time) {//shoot two side-by-side projectiles

            Quaternion aimRotation = Quaternion.AngleAxis(angle, Vector3.forward);

            Vector3 spawnOffset = aimRotation * Vector3.right * bulletSeparation;
            Shoot(bullet,transform.position + spawnOffset, angle);
            Shoot(bullet,transform.position - spawnOffset, angle);

            Next(2);
        }
    }

    public void ShootDelay() {
        if (shootTimer < Time.time) { Next(0); }
    }

    public void Next(int _state) {
        state = _state;
        switch (_state) {//lazy state machine
            case 0: { 
                shootTimer = Time.time+fireRate;
                this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1,1,1);
            } return;
            case 1: { 
                shootTimer = Time.time+fireRate;
            } return;
            case 2: {
                shootTimer = Time.time+shoot_delay;
            } return;
        }
    }
}
