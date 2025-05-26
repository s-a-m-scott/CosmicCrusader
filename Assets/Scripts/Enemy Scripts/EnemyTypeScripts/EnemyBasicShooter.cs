using UnityEngine;

public class EnemyBasicShooter : EnemyBase
{
    public GameObject bullet; 
    public float range = 4;
    private int state=0;//0 default movement, 1 shooting
    private int accuracy = 8;
    public override void Start()
    {
        maxHealth = 35f * scaleFactor;
        maxSpeed = 2.5f;
        damage = 10f * scaleFactor;
        fireRate = 3;
        value = (int)(12f * scaleFactor);
        bulletSpeed = 9;

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

        if (shootTimer < Time.time) {
            Shoot(bullet,transform.position, angle + Random.Range(-accuracy,accuracy));
            Next(0);
        }
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
        }
    }
}
