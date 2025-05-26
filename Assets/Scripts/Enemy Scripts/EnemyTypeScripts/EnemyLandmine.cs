using UnityEngine;
using System.Collections;
using System;

public class EnemyLandmine : EnemyBase
{
    public GameObject bullet;
    private int rotSpeed = 120;
    private float bulletSpMax = 6;
    private float bulletSpMin = 1;
    private int bulletsSpawned = 15;
    public override void Start()
    {
        maxHealth = 50f * scaleFactor;
        maxSpeed = 0.5f;
        damage = 5f * scaleFactor;
        fireRate = 2f;
        value = (int)(15 * scaleFactor);
        bulletSpeed = 5;

        current_health = maxHealth;//this cant easily be done in base class, which is dumb and annoying
        base.Start();
    }

    public override void Update()
    {
        base.Update();
        MoveToPlayer(maxSpeed);

        angle += rotSpeed*Time.deltaTime;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public override IEnumerator destroyActor (bool giveMoney, bool outOfRange=false) {
        for (int i=0; i<bulletsSpawned; i++) { SpawnBullet(); }

        StartCoroutine(base.destroyActor(giveMoney));
        yield return new WaitForFixedUpdate();
    } 

    public override IEnumerator takeDamage(float damage) {
        SpawnBullet();

        StartCoroutine(base.takeDamage(damage));
        yield return new WaitForFixedUpdate();
    }

    void SpawnBullet() {
        bulletSpeed = UnityEngine.Random.Range(bulletSpMin,bulletSpMax);
        GameObject bulletObject = Shoot(bullet,transform.position, UnityEngine.Random.Range(0,360));
        bulletObject.GetComponent<EnemyBullet>().type = EnemyBullet.bulletType.round;
    }
}
