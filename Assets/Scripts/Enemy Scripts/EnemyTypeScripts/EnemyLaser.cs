using System.Collections;
using UnityEngine;

public class EnemyLaser : EnemyBase
{
    public GameObject bullet; 
    public float range = 4;
    private int state=0;//0 default movement, 1 aiming, 2 shoot
    private float shootTime=0.3f;
    private float shots=10;//shoot 5 shots real fast
    public override void Start()
    {
        maxHealth = 35f * scaleFactor;
        maxSpeed = 2.5f;
        damage = 8f * scaleFactor;
        fireRate = 3;//aim time
        value = (int)(12 * scaleFactor);
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
            case 1: { AimState(); } return;
            case 2: { ShootState(); } return;
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

    public void AimState() {
        angle = CalculateAngle();
        float lerpCol = (shootTimer-Time.time)/fireRate;
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1,lerpCol,1);

        if (shootTimer < Time.time) { Next(2); }
    }

    public void ShootState() {
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1,0,1);
        
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
                //activate laser in no-hit mode
                shootTimer = Time.time+fireRate;
            } return;
            case 2: { 
                //activate laser hit mode
                for (int i=0; i<shots; i++) { StartCoroutine(shootAtTime((shootTime/shots)*i)); }
                shootTimer = Time.time+shootTime;
            } return;
        }
    }

    IEnumerator shootAtTime(float _time) {
        yield return new WaitForSeconds(_time);
        Shoot(bullet,transform.position, angle);
    }
}
