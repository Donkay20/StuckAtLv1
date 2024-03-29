using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    [SerializeField] int health;
    [SerializeField] GameObject damageTextPrefab;
    public void TakeDamage(int damage) {
        health -= damage;

        if (damageTextPrefab) {
            var dmg = Instantiate(damageTextPrefab, transform.position, Quaternion.identity);
            dmg.GetComponentInChildren<DamageNumber>().Setup(damage, "", false, false);
        }
        
        if (health <= 0) {
            Destroy(gameObject);
        }
    }
}
