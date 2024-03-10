using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnemiaSpread : MonoBehaviour
{
    void Start() {
        Destroy(gameObject, 0.5f);
    }

    private void OnTriggerEnter2D(Collider2D col) {
        Enemy enemy = col.GetComponent<Enemy>();
        if (enemy != null) {
            int anemiaDamage;
            if ((int) (enemy.maxHP * 0.05f) < 1) {
                anemiaDamage = 1;
            } else if ((int) (enemy.maxHP * 0.05f) > 100) {
                anemiaDamage = 100;
            } else {
                anemiaDamage = (int) (enemy.maxHP * 0.05f);
            }
            enemy.ApplyAnemia(anemiaDamage, 5f);
        }
    }
}
