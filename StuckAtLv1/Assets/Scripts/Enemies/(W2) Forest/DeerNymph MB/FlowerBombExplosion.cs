using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerBombExplosion : MonoBehaviour
{
    private readonly int EXPLOSION_DAMAGE = 3;
    private void OnTriggerEnter2D(Collider2D col) {
        Character player = col.GetComponent<Character>();
        if (player != null) {
            player.TakeDamage(EXPLOSION_DAMAGE);
        }
    }
}
