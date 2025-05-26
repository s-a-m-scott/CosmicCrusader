using System.Collections;
using System;
using UnityEngine;

public class EnemySimple : EnemyBase
{
    
    public override void Start()
    {
        maxHealth = 20f * scaleFactor;
        maxSpeed = 1.5f;
        damage = 10f * scaleFactor;
        value = (int)(5 * scaleFactor);
        current_health = maxHealth;
        base.Start();
    }
    public override void Update()
    {
        base.Update();
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        MoveToPlayer(maxSpeed);
        
    }
}

