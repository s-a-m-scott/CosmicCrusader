using UnityEngine;
using System;

public class PlayerBullet : MonoBehaviour
{
    public float speed = 12;
    public float lifetime = 2;
    float spawnTime = 0;
    public float damage;
    Vector3 movement;
    public GameObject damageDisplay;
    void Start() {
        spawnTime = Time.time;
        movement = transform.rotation * Vector3.up * speed;
    }

    void Update() {
        transform.position += movement * Time.deltaTime;

        if (spawnTime + lifetime < Time.time) {
            Destroy(this.gameObject);
        }
    }

    void OnTriggerStay(Collider other) {
        if (other.gameObject.CompareTag("EnemyShip")) {

                StartCoroutine(other.gameObject.GetComponent<EnemyBase>().takeDamage(damage));
                GameObject display = Instantiate(damageDisplay, transform.position, Quaternion.identity);
                display.GetComponent<ItemCollectDisplay>().textDisplay.text = Convert.ToString(damage);

                Destroy(this.gameObject);
        }
    }
}


