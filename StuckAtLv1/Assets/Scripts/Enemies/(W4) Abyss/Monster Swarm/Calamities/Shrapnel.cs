using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shrapnel : MonoBehaviour
{
    private readonly int SHRAPNEL_BASE_DAMAGE = 3;
    private void OnTriggerEnter2D(Collider2D col) {
        if (col.TryGetComponent<Character>(out var player)) {
            BuffManager b = FindAnyObjectByType<BuffManager>();
            b.AddDebuff("bleed", 0.5f, 5f);
            if (player.currentHp > 10) {
                player.TakeDamage(player.currentHp / 5);
            } else {
                player.TakeDamage(SHRAPNEL_BASE_DAMAGE);
            }
        }
    }
}