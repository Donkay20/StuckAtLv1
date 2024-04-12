using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimePuddle : MonoBehaviour
{
    private Slot slot;
    private List<Enemy> enemyList = new List<Enemy>();
    private readonly int SLIMEPUDDLE_BASE_DMG = 10;
    private readonly float SLIMEPUDDLE_BASE_DURATION = 4f;
    private int damage;
    private float duration;
    private float size;
    void Start() {
        slot = FindAnyObjectByType<AttackSpawner>().GetParent();
        AttackSlotBonus asb = FindAnyObjectByType<AttackSlotBonus>();

        damage = asb.GetDamageBonus(slot, SLIMEPUDDLE_BASE_DMG);
        size = asb.GetSizeBonus(slot); transform.localScale = new Vector2(size, size);
        duration = asb.GetDurationBonus(slot, SLIMEPUDDLE_BASE_DURATION);
    }

    void Update() {
        duration -= Time.deltaTime;
        if (duration <= 0) {
            foreach(Enemy enemy in enemyList) {
                if (enemy != null) {
                    FindAnyObjectByType<OnHitBonus>().ApplyDamageBonus(slot, enemy, damage);
                }
            }
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.TryGetComponent<Enemy>(out var enemy)) {
            enemy.ApplySlow(0.8f, duration);
            enemyList.Add(enemy);
        }
    }  
}