using UnityEngine;
using System.Collections;
using System;

public class EnemyBullet : MonoBehaviour
{
    public enum bulletType { standard=0, round=1}
    protected float speed = 7;
    public float Speed { get { return speed; } set { speed = value; } }
    protected float lifetime = 5;//needs to be a lot longer than player bullets, as they move slower - on screen longer
    protected float spawnTime = 0;
    protected float damage;
    public float Damage { get { return damage; } set { damage = value; } }
    public bulletType type;
    public Sprite[] sprites;
    Vector3 movement;

    public virtual void Start() {
        //GetComponent<AudioSource>().Play();
        SpriteRenderer rend = this.gameObject.GetComponent<SpriteRenderer>();
        rend.sprite = sprites[(int)type];
        spawnTime = Time.time;
        movement = transform.rotation * Vector3.up * speed;
    }

    public virtual void Update() {
        transform.position += movement * Time.deltaTime;
        if (spawnTime + lifetime < Time.time) { Destroy(this.gameObject); }
    }

    public virtual IEnumerator destroyActor () {
        GetComponent<Collider>().enabled = false;
        Destroy(this.gameObject);
        yield return null;
    } 
}
