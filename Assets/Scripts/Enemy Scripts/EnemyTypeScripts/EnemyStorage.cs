using System.Collections;
using System;
using UnityEngine;

public class EnemyStorage : EnemyBase
{
    private int childrenSpawned = 4;
    private float spawnRange = 0.3f;
    public GameObject child;
    public override void Start()
    {
        maxHealth = 75f * scaleFactor;
        maxSpeed = 2.5f;
        damage = 10f * scaleFactor;
        value = (int)(5 * scaleFactor);
        current_health = maxHealth;//this cant be done in base class, which is dumb and annoying
        base.Start();
    }
    public override void Update()
    {
        base.Update();
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        MoveToPlayer(maxSpeed);
        
    }

    public override IEnumerator destroyActor (bool giveMoney, bool outOfRange=false) {
        for (int i=0; i<childrenSpawned; i++) { SpawnChild(); }

        StartCoroutine(base.destroyActor(giveMoney));
        yield return new WaitForFixedUpdate();
    } 

    void SpawnChild() {
        Quaternion aimRotation = Quaternion.AngleAxis(UnityEngine.Random.Range(0,360), Vector3.forward);
        Vector3 spawnOffset = aimRotation * Vector3.right * UnityEngine.Random.Range(0,spawnRange);
        Instantiate(child,transform.position+spawnOffset,aimRotation);
    }
}

