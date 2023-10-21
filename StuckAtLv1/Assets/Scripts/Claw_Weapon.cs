using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Claw_Weapon : MonoBehaviour
{
    [SerializeField] float timeToAttack = 4f;
    float timer = 4f;

    [SerializeField] GameObject claw;
    [SerializeField] Vector2 clawAttackSize = new Vector2(4f, 4f);

    [SerializeField] int clawDamage = 1;

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0f) {
            Attack();
        }
    }

    private void Attack() {
        timer = timeToAttack;
        claw.SetActive(true);
        Collider2D[] colliders = Physics2D.OverlapBoxAll(claw.transform.position, clawAttackSize, 0f);
        ApplyDamage(colliders);
    }

    private void ApplyDamage(Collider2D[] colliders)
    {
        for (int i = 0; i < colliders.Length; i++) {
            Enemy e = colliders[i].GetComponent<Enemy>();
            if (e != null) {
                colliders[i].GetComponent<Enemy>().TakeDamage(clawDamage);
            }
        }
    }
}
