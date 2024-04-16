using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAttack : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col) {
        if (col.TryGetComponent<Character>(out var player)) {
            int damage;
            if (player.currentHp < 10) {
                damage = 3;
            } else {
                damage = player.currentHp / 10;
            }
            player.TakeDamage(damage);
        }
    }
}