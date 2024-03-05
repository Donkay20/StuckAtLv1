using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Siphon : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 direction;
    private float force;
    private bool moveTowardsPlayer = false;
    private float moveTowardsSpeed;
    private float moveTimer;

    private Transform playerPos;

    // Start is called before the first frame update
    void Start()
    {
        direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        force = Random.Range(-20f, 20f);
        moveTowardsSpeed = Random.Range(0.05f, 0.1f);
        playerPos = GameObject.Find("Player").transform;

        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(direction.normalized * force, ForceMode2D.Impulse);
        
        moveTimer = Random.Range(0.3f, 0.5f);
        Invoke(nameof(Move), moveTimer);
    }

    // Update is called once per frame
    void Update()
    {
        if(moveTowardsPlayer)
        {
            transform.position = Vector2.MoveTowards(transform.position, playerPos.position, 0.5f);
        }
    }

    void Move()
    {
        moveTowardsPlayer = true;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
