using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderBite : MonoBehaviour
{
    [SerializeField] private SpiderBiteParent parent;
    private Slot slot;
    private AttackSlotBonus asb;
    private readonly float SPIDER_BITE_SPEED = 10f;
    private readonly int SPIDERBITE_BASE_DMG = 5;
    private readonly float SPIDERBITE_BASE_ANEMIA_DURATION = 5f;
    
    private int damage;
    

    void Start() {
        slot = parent.GetSlot();
        asb = FindAnyObjectByType<AttackSlotBonus>();

        damage = asb.GetDamageBonus(slot, SPIDERBITE_BASE_DMG);
    }

    void Update() {
        transform.Translate(SPIDER_BITE_SPEED * Time.deltaTime * Vector2.up);
    }

    private void OnTriggerEnter2D(Collider2D col) { //applies anemia by default
        if (col.TryGetComponent<Enemy>(out var enemy)) {
            enemy.ApplyAnemia(damage / 2, asb.GetDurationBonus(slot, SPIDERBITE_BASE_ANEMIA_DURATION));
            FindAnyObjectByType<OnHitBonus>().ApplyDamageBonus(slot, enemy, damage);
        }
    }
}
