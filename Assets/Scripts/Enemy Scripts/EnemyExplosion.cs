using UnityEngine;

public class EnemyExplosion : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    float startTime;
    void Start()
    {
        startTime = Time.time;
        Animator anim = gameObject.GetComponent<Animator>();
        anim.speed = 3f;
    }

    // Update is called once per frame
    void Update()
    {
        if (startTime + 0.5f < Time.time) Destroy(this.gameObject);
    }
}
