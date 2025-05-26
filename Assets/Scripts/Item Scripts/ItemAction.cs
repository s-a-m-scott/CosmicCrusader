using UnityEngine;
using System;
using System.Collections;

public class ItemAction : MonoBehaviour
{
    bool collected = false;
    public GameObject target = null;
    Vector3 velocity;
    public AudioSource audioSource;
    public Sprite[] Coins;
    float startSpeedMax = 3f;
    public int value;
    public float maxLifetime = 10;//seconds
    float spawnTime;
    float startCollectDist = 1.25f;
    float fullCollectDist = 0.25f;

    void Start()
    {
        target = GameObject.FindWithTag("Player");
        //Debug.Log(value);
        gameObject.GetComponent<SpriteRenderer>().sprite = Coins[value-1];
        
        velocity = UnityEngine.Random.insideUnitCircle * startSpeedMax;

        maxLifetime = UnityEngine.Random.Range(maxLifetime * 0.7f,maxLifetime);
        spawnTime = Time.time;
    }

    void Update()
    {
        velocity = Vector3.Lerp(velocity, Vector3.zero, 0.05f);
        transform.position += velocity * Time.deltaTime;
        if (target != null)
        {
            if (Vector3.Distance(target.transform.position, transform.position) < startCollectDist)
            {
                if (!collected) audioSource.Play();
                collected = true;
            }
            if (Vector3.Distance(target.transform.position, transform.position) < fullCollectDist)
            {

                target.GetComponent<PlayerActions>().gameState.manager.currentMoney += value;
                Destroy(this.gameObject);
            }
            if (collected)
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = true;
                transform.position = Vector3.Lerp(transform.position, target.transform.position, 0.1f);
            }
        }
        
        if (Time.time > spawnTime + maxLifetime / 2 && !collected)
            {
                //flash visibility on/off
                gameObject.GetComponent<SpriteRenderer>().enabled = Mathf.Sin((Time.time + maxLifetime) * Mathf.PI * 4) > 0;

                if (Time.time > spawnTime + maxLifetime)
                {
                    Destroy(this.gameObject);
                }
            }
        
    }
}


