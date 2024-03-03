using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneToss : MonoBehaviour
{
    float BONETOSS_BASE_TIMER = 2f;   //if a modifier increase skill time duration, it would call back to the parent slot and acquire the modifier for calculation
    private readonly int BONETOSS_BASE_DMG = 5;
    Rigidbody2D rb;
    Slot slot;
    private Vector3 mousePosition;
    private Camera mainCamera;
    public float timer;
    public float speed;
    private int damage;
    private float size;

    void Start() {  
        //Aim towards the mouse
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody2D>();
        slot = GetComponentInParent<Slot>();
        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePosition - transform.position;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * speed;
        AttackSlotBonus asb = FindAnyObjectByType<AttackSlotBonus>();

        damage = asb.GetDamageBonus(slot, BONETOSS_BASE_DMG);

        size = asb.GetSizeBonus(slot);
        transform.localScale = new Vector2(.5f * size, .5f * size);       //for the bone toss, 0.5f is the base size.

        timer = asb.GetDurationBonus(slot, BONETOSS_BASE_TIMER);
    }

    void Update() {
        timer -= Time.deltaTime;
        if (timer <= 0) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D col) {
        Enemy enemy = col.GetComponent<Enemy>();
        FindAnyObjectByType<OnHitBonus>().ApplyDamageBonus(slot, enemy, damage);
    }
}