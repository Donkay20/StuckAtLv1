using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnemiaSpread : MonoBehaviour
{
    void Start() {
        Destroy(gameObject, 0.5f);
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.TryGetComponent<Enemy>(out var enemy)) {
            int anemiaDamage;
            if ((int) (enemy.maxHP * 0.05f) < 1) {
                anemiaDamage = 1;
            } else {
                anemiaDamage = (int) (enemy.maxHP * 0.05f);
            } 
            if (anemiaDamage > 50) {
                anemiaDamage = 50;
            }
            enemy.ApplyAnemia(anemiaDamage, 3f);
        }
    }
}