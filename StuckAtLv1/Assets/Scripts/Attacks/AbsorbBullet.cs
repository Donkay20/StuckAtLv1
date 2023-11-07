using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AbsorbBullet : MonoBehaviour
{
    float timer = 0.5f;
    Rigidbody2D rb;
    private Vector3 mousePosition;
    private Camera mainCamera;
    public float speed;

    void Start() {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody2D>();
        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePosition - transform.position;
        Vector3 rotation = transform.position - mousePosition;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * speed;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0) {
            GetComponentInParent<Slot>().AbsorbBulletAvailable = true;
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D col) {
        Enemy enemy = col.GetComponent<Enemy>();
        if (enemy != null) {
            enemy.TakeDamage(1);
            GetComponentInParent<Slot>().AcquireSkill(1, 3);
            GetComponentInParent<Slot>().AbsorbBulletAvailable = true;
            Destroy(gameObject);
        }
        /*
        switch(col.name) {
            case "Skeleton": //1
                Enemy enemy = col.GetComponent<Enemy>();
                GetComponentInParent<Slot>().AcquireSkill(1);
                Debug.Log("acquired skeleton dna");
                enemy.TakeDamage(1);
                GetComponentInParent<Slot>().AbsorbBulletAvailable = true;
                Destroy(gameObject);
            break;
        }
        */
    }
}
