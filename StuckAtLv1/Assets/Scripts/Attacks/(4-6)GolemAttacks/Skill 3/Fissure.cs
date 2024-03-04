using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Fissure : MonoBehaviour
{
    private readonly int FISSURE_BASE_DAMAGE = 15;
    private readonly float FISSURE_BASE_DURATION = 3f;
    private int damage;
    private float timer;
    private float size;
    private Slot slot;    

    void Start()
    {
        FaceMouse();
        slot = FindAnyObjectByType<AttackSpawner>().GetParent();
        AttackSlotBonus asb = FindAnyObjectByType<AttackSlotBonus>();
        //set the parent before anything else, by grabbing the parent's relation to the slot

        size = asb.GetSizeBonus(slot);
        transform.localScale = new Vector2(size, size);

        timer = asb.GetDurationBonus(slot, FISSURE_BASE_DURATION);

        damage = asb.GetDamageBonus(slot, FISSURE_BASE_DAMAGE);
    }

    private void Update() {
        timer -= Time.deltaTime;
        if (timer <= 0) {
            Destroy(gameObject);
        }
    }

    private void FaceMouse() {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
        transform.up = direction;
    }

    private void OnTriggerEnter2D(Collider2D col) {
        Enemy enemy = col.GetComponent<Enemy>();
        FindAnyObjectByType<OnHitBonus>().ApplyDamageBonus(slot, enemy, damage);
    }

    private void OnTriggerStay2D(Collider2D col) {
        Enemy enemy = col.GetComponent<Enemy>();
        if (enemy != null) {
            enemy.ApplySlow(0.5f, 0.5f);  
            //As long as the enemy is in the fissure, they will remain slowed.
        }
    }
}
