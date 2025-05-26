using System.Collections;
using UnityEngine;

public class EnemyBoss : EnemyBase
{
    #region properties etc
    public GameObject bullet;
    public GameObject speederObject;
    public GameObject landmineObject;
    private ShipGameState gameState;
    public float range = 4;
    private bool hasFired = false;
    private BossStates state = BossStates.chase;

    private float chaseMin = 1f;
    private float chaseMax = 3f;

    private float basicShootTime = 2f;//total time of attack
    private float basicShootWindup = 1f;
    private float basicShootGap = 0.3f;//time between attacks
    private float basicShootSpread = 100;///120 degrees attack
    private float basicShootBulletSP = 4.5f;

    private float circleSprayDuration = 1.5f;
    private int circleSprayShotsMin = 18;
    private int circleSprayShotsMax = 36;
    private float circleSprayRate = 0.25f;
    private float circleSprayBulletSP = 7;

    private float machineGunDuration = 3.5f;
    private float machineGunAccuracy = 8;
    private float machineGunRate = 0.1f;
    private float machineGunBulletSP = 12;

    private float speederDelay = 0.2f;
    private float speederDuration = 3f;

    private float fastSpeed = 10;
    private float slowSpeed = 4;

    private float landmineDuration = 2.5f;
    private float landmineDelay = 0.1f;

    private static float deathDuration = 8;
    private float deathExplosionRate = 0.1f;
    private float bigExplodeTime = deathDuration * 0.4f;

    private float stateTimer = 0;
    private float stateStartTime;
    private enum BossStates { chase, basicShoot, circleSpray, machineGun, spawnSpeeders, spawnLandmine, dead }
    private BossStates[] attacks;
    #endregion
    public override void Start()
    {
        GameObject gsObject = GameObject.FindWithTag("GameState");
        gameState = gsObject.GetComponent<ShipGameState>();
        attacks = new BossStates[5] {
            BossStates.basicShoot,
            BossStates.circleSpray,
            BossStates.machineGun,
            BossStates.spawnSpeeders,
            BossStates.spawnLandmine
        };

        maxHealth = 100f * scaleFactor;
        maxSpeed = 3.0f;
        damage = 10f * scaleFactor;
        fireRate = 0.08f;
        value = (int)(12 * scaleFactor);
        bulletSpeed = 2;

        current_health = maxHealth;//this cant easily be done in base class, which is dumb and annoying
        base.Start();
        Next(0);
    }

    public override void Update()
    {
        base.Update();
        switch (state)
        {//lazy state machine
            case BossStates.chase: { MoveState(); } return;
            case BossStates.basicShoot: { BasicShoot(); } return;
            case BossStates.circleSpray: { CircleSpray(); } return;
            case BossStates.machineGun: { MachineGun(); } return;
            case BossStates.spawnLandmine: { SpawnSpeeders(); } return;
            case BossStates.spawnSpeeders: { SpawnLandMine(); } return;
            case BossStates.dead: { Dead(); } return;
        }
    }

    public void MoveState()
    {
        if (Vector3.Distance(transform.position, playerObject.transform.position) > 3) maxSpeed = fastSpeed;
        else maxSpeed = slowSpeed;
        MoveToPlayer(maxSpeed);
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        if (Time.time > stateTimer) { Next(attacks[Random.Range(0, attacks.Length)]); }//do a random attack
    }

    private void BasicShoot()
    {//shoot 3 sets of bullets at player, with a gap in each line
        angle = CalculateAngle();
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);//aim at player
        if (Time.time > stateStartTime + basicShootWindup && !hasFired)
        {
            //do shootin
            for (int i = 0; i < 3; i++)
            {
                StartCoroutine(BasicShootFire(i * basicShootGap));
            }
            hasFired = true;
        }

        if (Time.time > stateTimer) { Next(BossStates.chase); }//return to chase state
    }

    private IEnumerator BasicShootFire(float delay)
    {
        yield return new WaitForSeconds(delay);
        int totalshots = 15;
        float shootAngle; int gapShot;
        gapShot = Random.Range(0, totalshots - 2) + 1;
        for (int j = 0; j < totalshots; j++)
        {
            shootAngle = ((basicShootSpread / totalshots) * j) - (basicShootSpread / 2);
            shootAngle += angle;
            if (j != gapShot)
            {
                GameObject _bullet = Shoot(bullet, transform.position, shootAngle);
                _bullet.GetComponent<EnemyBullet>().type = EnemyBullet.bulletType.round;
            }
        }
    }

    private void CircleSpray()
    { // shoot out a circle of bullets a bunch of times
        if (!hasFired)
        {
            for (int i = 0; i < 5; i++)
            {
                StartCoroutine(CircleSprayFire(i * circleSprayRate));
            }
            hasFired = true;
        }
        if (Time.time > stateTimer) { Next(BossStates.chase); }//return to chase state
    }

    private IEnumerator CircleSprayFire(float delay)
    {
        yield return new WaitForSeconds(delay);
        int bulletsTotal; float shootAngle; float randAngle;
        bulletsTotal = Random.Range(circleSprayShotsMin, circleSprayShotsMax);
        randAngle = Random.Range(0, 360);
        for (int j = 0; j < bulletsTotal; j++)
        {
            shootAngle = ((360 / bulletsTotal) * j) + randAngle;
            GameObject _bullet = Shoot(bullet, transform.position, shootAngle);
            _bullet.GetComponent<EnemyBullet>().type = EnemyBullet.bulletType.standard;
        }
    }

    private void MachineGun()
    {//shoot at player a lot
        angle = CalculateAngle();
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);//aim at player
        if (!hasFired)
        {
            int bulletsTotal = (int)(machineGunDuration / machineGunRate);
            for (int i = 0; i < bulletsTotal; i++)
            {
                StartCoroutine(MachineGunFire(i * machineGunRate));
            }
            hasFired = true;
        }
        if (Time.time > stateTimer) { Next(BossStates.chase); }//return to chase state
    }

    private IEnumerator MachineGunFire(float delay)
    {
        yield return new WaitForSeconds(delay);
        int shots = Random.Range(1, 4);
        float shootAngle;
        for (int i = 0; i < shots; i++)
        {
            shootAngle = angle + Random.Range(-machineGunAccuracy, machineGunAccuracy);
            GameObject _bullet = Shoot(bullet, transform.position, shootAngle);
            _bullet.GetComponent<EnemyBullet>().type = EnemyBullet.bulletType.standard;
        }
    }

    private void SpawnSpeeders()
    {//shoot speeder enemies at player
        if (!hasFired)
        {
            int shots = Random.Range(3, 8);
            for (int i = 0; i < shots; i++)
            {
                StartCoroutine(SpawnSpeedersFire(i * speederDelay));
            }
            hasFired = true;
        }
        if (Time.time > stateTimer) { Next(BossStates.chase); }//return to chase state
    }

    private IEnumerator SpawnSpeedersFire(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameObject enemySpawned = Instantiate(speederObject, transform.position, transform.rotation);
        EnemyBase _enemy = enemySpawned.GetComponent<EnemyBase>();
        _enemy.scaleFactor = scaleFactor;
    }

    private void SpawnLandMine()
    { // spawn a bunch of landmine enemies all around map
        if (!hasFired)
        {
            int spawns = Random.Range(10, 16);
            for (int i = 0; i < spawns; i++)
            {
                StartCoroutine(SpawnLandMinesFire(i * landmineDelay));
            }
            hasFired = true;
        }
        if (Time.time > stateTimer) { Next(BossStates.chase); }
    }

    private IEnumerator SpawnLandMinesFire(float delay)
    {
        yield return new WaitForSeconds(delay);
        float pMoveDir = playerObject.transform.rotation.eulerAngles.z;
        float angleOffset = Random.Range(-45, 45);
        pMoveDir += angleOffset;
        Quaternion spawnDir = Quaternion.AngleAxis(pMoveDir, Vector3.forward);

        Vector3 spawnOffset = spawnDir * Vector3.right * EnemySpawner.spawnDistance;
        GameObject enemySpawned = Instantiate(landmineObject, playerObject.transform.position + spawnOffset, new Quaternion());
        EnemyBase _enemy = enemySpawned.GetComponent<EnemyBase>();
        _enemy.scaleFactor = scaleFactor;
    }

    private void Dead()
    {
        if (!hasFired)
        {
            hasFired = true;
            GetComponent<Collider>().enabled = false;
            int explosionCount = 20;
            StartCoroutine(FinalDeath());

            for (int i = 0; i < explosionCount; i++)
            {
                StartCoroutine(SpawnExplosion(bigExplodeTime));
                StartCoroutine(SpawnExplosion(i * deathExplosionRate));
            }
        }
        if (Time.time > stateStartTime + bigExplodeTime && GetComponent<SpriteRenderer>().enabled)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            spawnCoins();
        }
    }

    private IEnumerator SpawnExplosion(float delay)
    {
        
        yield return new WaitForSeconds(delay);
        float offset = Random.Range(0f, 1.3f);
        float _angle = Random.Range(0,360);
        Quaternion _rotation = Quaternion.AngleAxis(_angle, Vector3.forward);

        Vector3 spawnOffset = _rotation * Vector3.right * offset;
        Instantiate(explosion, transform.position+spawnOffset, new Quaternion()); 

    }

    private void Next(BossStates _state)
    {
        state = _state;
        stateStartTime = Time.time;
        hasFired = false;
        StopAllCoroutines();
        switch (_state)
        {//lazy state machine
            case BossStates.chase:
                { //move
                    stateTimer = Time.time + Random.Range(chaseMin, chaseMax);
                    
                }
                return;
            case BossStates.basicShoot:
                {
                    stateTimer = Time.time + basicShootTime;
                    
                    bulletSpeed = basicShootBulletSP;
                }
                return;
            case BossStates.circleSpray:
                {
                    stateTimer = Time.time + circleSprayDuration;
  
                    bulletSpeed = circleSprayBulletSP;
       
                }
                return;
            case BossStates.machineGun:
                {
                    stateTimer = Time.time + machineGunDuration;
       
                    bulletSpeed = machineGunBulletSP;

                }
                return;
            case BossStates.spawnSpeeders:
                {
                    stateTimer = Time.time + speederDuration;
       
                }
                return;
            case BossStates.spawnLandmine:
                {
                    stateTimer = Time.time + landmineDuration;
         
                }
                return;
            case BossStates.dead:
                {
                    stateTimer = Time.time + deathDuration;
     
                }
                return;

        }
    }

    public override IEnumerator destroyActor(bool giveMoney, bool outOfRange = false)
    {

        if (outOfRange) yield return null; //boss cant die from being too far away - this is a little hacky but gets the job done
        if (alive) Next(BossStates.dead);
        alive = false;
        yield return null;
    }

    public IEnumerator FinalDeath()
    {
        yield return new WaitForSeconds(deathDuration);
        gameState.bossWaveWin = true;
        
    }
}
