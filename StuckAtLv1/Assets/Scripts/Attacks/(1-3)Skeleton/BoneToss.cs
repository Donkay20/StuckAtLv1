using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoneToss : MonoBehaviour
{
    float timer = 2f;   //if a modifier increase skill time duration, it would call back to the parent slot and acquire the modifier for calculation
    Rigidbody2D rb;
    Slot parent;
    private Vector3 mousePosition;
    private Camera mainCamera;
    public float speed;
    private int damage;

    void Start() {  //aim towards the mouse
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody2D>();
        parent = GetComponentInParent<Slot>();
        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePosition - transform.position;
        Vector3 rotation = transform.position - mousePosition;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * speed;

        float scalingFactor = 1 + parent.GetCommonUpgrade(1)*0.2f + parent.GetRareUpgrade(1)*0.3f + parent.GetLegendaryUpgrade(1)*0.4f;
        transform.localScale = new Vector2(.5f*scalingFactor, .5f*scalingFactor); //for the bone toss, 0.5f is the base size, idk why, but we will use that as the base in this case.

        //apply duration bonus
        timer *= 1 + (parent.GetCommonUpgrade(2)*0.2f + parent.GetRareUpgrade(2)*0.4f + parent.GetLegendaryUpgrade(2)*0.6f);
        Debug.Log("timer: " + timer);

        //apply damage bonus
        damage = (int)(5 * (1+(parent.GetCommonUpgrade(0)*0.2f + parent.GetRareUpgrade(0)*0.4f + parent.GetLegendaryUpgrade(0)*0.6f)));
        Debug.Log("damage: " + damage);
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D col) {
        Enemy enemy = col.GetComponent<Enemy>();
        if (enemy != null) {
            enemy.TakeDamage(damage);   //if a modifier increases damage, it would call back to the parent slot and acquire the modifier for calculation
        }
    }
}
