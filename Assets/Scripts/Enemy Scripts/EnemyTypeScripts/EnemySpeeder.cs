using System.Collections;
using System;
using UnityEngine;

public class EnemySpeeder : EnemyBase
{
    private int state=0;//0 for move, 1 for stop
    private float hitTimer=0;
    public float hitTime = 3;
    private float currentSpeed;
    private float accel = 2;

    public override void Start()
    {
        maxHealth = 40f * scaleFactor;
        maxSpeed = 12.0f;
        damage = 15f * scaleFactor;
        value = (int)(10 * scaleFactor);
        current_health = maxHealth;//this cant be done in base class, which is dumb and annoying
        base.Start();
    }
    public override void Update()
    {
        base.Update();
        
        switch (state) {//lazy state machine
            case 0: { MoveState(); } return;
            case 1: { StopState(); } return;
        }
        
    }
    public void MoveState() {
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        currentSpeed = Math.Min( currentSpeed + (accel *Time.deltaTime), maxSpeed);
        MoveToPlayer(currentSpeed);
    }

    public void StopState() {
        if (hitTimer < Time.time) {
            state=0;
            currentSpeed=0;
        }
    }

    public override IEnumerator takeDamage(float damage) { 
        state=1;
        hitTime=Time.time+hitTime;
        
        StartCoroutine(base.takeDamage(damage));
        yield return new WaitForFixedUpdate();
    }

    public override IEnumerator destroyActor (bool giveMoney, bool outOfRange=false) {
        state=1;
        hitTime=Time.time+hitTime;
        if (current_health <= 0) { StartCoroutine(base.destroyActor(giveMoney)); }//this is hacky but eh
        yield return new WaitForFixedUpdate();
    } 
}

