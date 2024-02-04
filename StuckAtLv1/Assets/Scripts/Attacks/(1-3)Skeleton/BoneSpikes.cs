using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneSpikes : MonoBehaviour
{
    private float timer = 3f;
    private float slowdownTime;
    Rigidbody2D rb;
    Slot parent;
    private Vector3 mousePosition;
    private Camera mainCamera;
    private float speed = 5; private float baseSpeed = 5;
    private int damage;
    void Start() {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody2D>();
        parent = GetComponentInParent<Slot>();
        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePosition - transform.position;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * speed;

        float scalingFactor = 1 + parent.GetCommonUpgrade(1)*0.2f + parent.GetRareUpgrade(1)*0.3f + parent.GetLegendaryUpgrade(1)*0.4f;
        transform.localScale = new Vector2(.5f*scalingFactor, .5f*scalingFactor); //for the bone toss, 0.5f is the base size, idk why, but we will use that as the base in this case.

        //apply duration bonus
        timer *= 1 + (parent.GetCommonUpgrade(2)*0.2f + parent.GetRareUpgrade(2)*0.4f + parent.GetLegendaryUpgrade(2)*0.6f);
        slowdownTime = timer / 2;
        Debug.Log("timer: " + timer);

        //apply damage bonus
        damage = (int)(3 * (1+(parent.GetCommonUpgrade(0)*0.2f + parent.GetRareUpgrade(0)*0.4f + parent.GetLegendaryUpgrade(0)*0.6f)));
        Debug.Log("damage: " + damage);
    }

    void Update() {
        timer -= Time.deltaTime;
        if (timer <= 0) {
            Destroy(gameObject);
        }

        if (timer <= slowdownTime) {
            speed = baseSpeed*(timer/slowdownTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D col) {
        Enemy enemy = col.GetComponent<Enemy>();
        if (enemy != null) {
            enemy.TakeDamage(damage);   //if a modifier increases damage, it would call back to the parent slot and acquire the modifier for calculation
        }
    }
}
