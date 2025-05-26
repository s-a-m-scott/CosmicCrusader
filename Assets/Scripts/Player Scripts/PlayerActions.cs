using System;
using System.Collections.Generic;
using UnityEngine;


public class PlayerActions : MonoBehaviour
{
    //non upgradeable
    const float acceleration = 0.1f;
    const float bulletSeparation = 0.1f;
    const float friction = 0.05f;
    const float multiBulletSpread = 4;
    public GameObject bulletTemplate;
    public ShipGameState gameState;
    public List<double> stats;
    public AudioSource shootSound;
    public AudioSource hitSound;
    public int currentHealth;
    public bool alive = true;
    Vector3 velocity = new Vector3();
    float currentAngle = 0;
    public bool invincible =false;
    void Start() {
        GameObject managerObject = GameObject.FindWithTag("GameController");
        PlayerStats statStruct = managerObject.GetComponent<GameManager>().currentStats;
        stats = new List<double>();
        for (int i=0;i<statStruct.statValues.Count;i++) {
            stats.Add(statStruct.statValues[i].upgradeLevels[statStruct.statValues[i].currentLevel].value);
        }
        currentHealth = (int)stats[(int)statNames.maxHealth];
    }
    float healTimer = 0;
    float invincibleTimer=0;
    void Update() {
        if (!alive) return;

        MovementCalculation();
        ShootLogic();

        if (healTimer < Time.time && currentHealth < stats[(int)statNames.maxHealth]) {
            currentHealth++;
            healTimer = Time.time + (float)(1 / stats[(int)statNames.regenRate]);
        }

        if (invincibleTimer > Time.time) {
            invincible=true;
            gameObject.GetComponent<Collider>().enabled = false;
            gameObject.GetComponent<SpriteRenderer>().enabled = Mathf.Sin((Time.time)*Mathf.PI*8) > 0; 
        } else {
            invincible = false;
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
            gameObject.GetComponent<Collider>().enabled = true;
        }
    }
    
    void MovementCalculation() {
        Vector3 input_dir = new Vector3(BoolToFloat(Input.GetKey(KeyCode.D)) - BoolToFloat(Input.GetKey(KeyCode.A)),
        BoolToFloat(Input.GetKey(KeyCode.W)) - BoolToFloat(Input.GetKey(KeyCode.S)), 0.0f);
        input_dir.Normalize();

        bool intent_to_move = input_dir != new Vector3();
        
        float lerp_val = ((acceleration * BoolToFloat(intent_to_move)) + (friction * BoolToFloat(!intent_to_move)));
        Vector3 target_velocity = input_dir * (float)stats[(int)statNames.moveSpeed] * BoolToFloat(intent_to_move);

        velocity = Vector3.Lerp(velocity, target_velocity, lerp_val);
        transform.position += velocity * Time.deltaTime;

        if (intent_to_move) {
            float target_dir = (Mathf.Atan2(-input_dir.x, input_dir.y)) * Mathf.Rad2Deg;
            
            //fixing odd turning behaviour with snap to direction
            if (Mathf.Abs(target_dir-currentAngle) < 180) currentAngle = Mathf.Lerp(currentAngle,target_dir, 0.1f);
            else currentAngle = target_dir;
            
        }
		transform.rotation = Quaternion.AngleAxis(currentAngle, Vector3.forward);
    }

    float shootTimer = 0;
    void ShootLogic() {
        Vector3 shootDir = new Vector3(BoolToFloat(Input.GetKey(KeyCode.RightArrow)) - BoolToFloat(Input.GetKey(KeyCode.LeftArrow)),
        BoolToFloat(Input.GetKey(KeyCode.UpArrow)) - BoolToFloat(Input.GetKey(KeyCode.DownArrow)), 0.0f);

        bool intentToShoot = shootDir != new Vector3();

        if (intentToShoot && shootTimer + stats[(int)statNames.fireRate] < Time.time) {
            float startOffset = -((float)stats[(int)statNames.projectiles]-1)/2;//messy but whatever - for multi shot
            for (int i=0;i<stats[(int)statNames.projectiles];i++) {
                float aimAngle = (Mathf.Atan2(-shootDir.x, shootDir.y)) * Mathf.Rad2Deg;
                float accuracy = (float)stats[(int)statNames.accuracy];
                aimAngle += UnityEngine.Random.Range(-accuracy,accuracy) - (multiBulletSpread * startOffset);
                Quaternion aimRotation = Quaternion.AngleAxis(aimAngle, Vector3.forward);

                Vector3 spawnOffset = aimRotation * Vector3.right * startOffset * bulletSeparation;
                startOffset++;
                GameObject bullet = Instantiate(bulletTemplate, transform.position+spawnOffset, aimRotation); 
                PlayerBullet bulletClass = bullet.GetComponent<PlayerBullet>();
                bulletClass.damage = (float)stats[(int)statNames.damage];
            }

            //GetComponent<AudioSource>().Play();
            shootSound.Play();
            shootTimer = Time.time;
        }
    }

    
    void OnCollisionStay(Collision collision)
    {
        if (!alive || invincible) return;//dont take hit if dead or invinc 
        string _tag = collision.gameObject.tag;
        if (_tag == "EnemyShip") {//ship collision
            GetHit(collision.gameObject.GetComponent<EnemyBase>());
        }

        else if (_tag == "EnemyBullet") {//bullet collision
            GetHit(collision.gameObject.GetComponent<EnemyBullet>());
        }
    }

    private void GetHit(EnemyBullet bullet) {
        currentHealth -= (int)bullet.Damage;
        invincibleTimer = Time.time + 1;//1 sec of invinc after get hit
        hitSound.Play();
        StartCoroutine(bullet.destroyActor());
    }

    private void GetHit(EnemyBase enemy) {
        currentHealth -= (int)enemy.Damage;
        invincibleTimer = Time.time + 1;
        hitSound.Play();
        StartCoroutine(enemy.destroyActor(false));
    }

    float BoolToFloat(bool _val) {
        float output; //apparently this isnt a builtin function? c# is wack
        if (_val) output = 1;
        else output = 0;
        return output;
    }
}
