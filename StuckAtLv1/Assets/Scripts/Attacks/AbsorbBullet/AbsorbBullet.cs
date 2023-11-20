using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class AbsorbBullet : MonoBehaviour
{
    float timer = 0.5f; //if a modifier increase skill time duration, it would call back to the parent slot and acquire the modifier for calculation
    Rigidbody2D rb;
    private Vector3 mousePosition;
    private Camera mainCamera;
    public float speed; //if a modifier increase skill speed, it would call back to the parent slot and acquire the modifier for calculation

    [SerializeField] GameObject spawnSiphon;

    void Start() {  //follow the mouse
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody2D>();
        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePosition - transform.position;
        Vector3 rotation = transform.position - mousePosition;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * speed;
    }

    void Update()   //destroy the bullet
    {
        timer -= Time.deltaTime;
        if (timer <= 0) {
            GetComponentInParent<Slot>().AbsorbBulletAvailable = true;  //need to make sure the absorb bullet is available again
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D col) {                     //upon hitting an enemy:

        Enemy enemy = col.GetComponent<Enemy>();                        //the enemy class will need to be changed to a bare-bones calculation class as every enemy will need it, can stack other classes on different enemies for unique behavior
        
        if (enemy != null) {
            Instantiate(spawnSiphon, transform.position, transform.rotation);
            switch (enemy.tag) {                                        //each enemy will have a unique tag which will identify which one the bullet is hitting. Alternatively, we can swap to their names if need be
                case "Enemy":
                enemy.TakeDamage(1);                                    //if a modifier increase skill damage, it would call back to the parent slot and acquire the modifier for calculation
                GetComponentInParent<Slot>().AcquireSkill(1, 3);        //case-by-case; in the terms of this enemy it would return skill 1 with 3 uses.
                GetComponentInParent<Slot>().AbsorbBulletAvailable = true;
                Destroy(gameObject);
                break;
                
                case "Golem1":
                enemy.TakeDamage(1);
                GetComponentInParent<Slot>().AcquireSkill(2, 3);
                GetComponentInParent<Slot>().AbsorbBulletAvailable = true;
                Destroy(gameObject);
                break;

                case "Golem2":
                enemy.TakeDamage(1);
                GetComponentInParent<Slot>().AcquireSkill(3, 1);
                GetComponentInParent<Slot>().AbsorbBulletAvailable = true;
                Destroy(gameObject);
                break;
            }
        }
    }
}
