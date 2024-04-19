using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderBiteParent : MonoBehaviour
{
    private Slot slot;
    private readonly float SPIDERBITE_BASE_DURATION = .5f;
    private float timer;
    private float size;
    
    void Start() {
        slot = FindAnyObjectByType<AttackSpawner>().GetParent();
        AttackSlotBonus asb = FindAnyObjectByType<AttackSlotBonus>();

        timer = asb.GetDurationBonus(slot, SPIDERBITE_BASE_DURATION);

        size = asb.GetSizeBonus(slot); 
        transform.localScale = new Vector2(size, size);
    }

    void Update() {
        timer -= Time.deltaTime;
        if (timer <= 0) {
            Destroy(gameObject);
        }
    }

    public Slot GetSlot() {
        return slot;
    }
}
