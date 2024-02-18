using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningSlashSword : MonoBehaviour
{
    private Slot slot;
    private int damage;

    public void Activate(Slot s) {
        slot = s;
        damage = (int) (25 * (1+(slot.GetCommonUpgrade(0)*0.2f + slot.GetRareUpgrade(0)*0.4f + slot.GetLegendaryUpgrade(0)*0.6f)));
        float scalingFactor = 1 + slot.GetCommonUpgrade(1)*0.2f + slot.GetRareUpgrade(1)*0.3f + slot.GetLegendaryUpgrade(1)*0.4f;
        transform.localScale = new Vector2(scalingFactor, scalingFactor);
    }

    private void OnTriggerEnter2D(Collider2D col) {
        Enemy enemy = col.GetComponent<Enemy>();
        if (enemy != null) {
            enemy.TakeDamage(damage);   //if a modifier increases damage, it would call back to the parent slot and acquire the modifier for calculation
        }
    }
}
