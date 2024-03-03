using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulsOfTheDamned : MonoBehaviour
{
    private Slot slot;
    private readonly float SOULS_BASE_DURATION = 5f;
    private float duration;
    private float refreshTimer = 0.05f;
    [SerializeField] private GameObject miniGhost;

    void Start() {
        slot = GetComponentInParent<Slot>();
        AttackSlotBonus asb = FindAnyObjectByType<AttackSlotBonus>();

        duration = asb.GetDurationBonus(slot, SOULS_BASE_DURATION);
    }

    void Update()
    {
        duration -= Time.deltaTime;
        if (duration <= 0) {
            Destroy(gameObject);
        }

        refreshTimer -= Time.deltaTime;
        if (refreshTimer <= 0) {
            Instantiate(miniGhost, slot.transform);
            refreshTimer = 0.05f;
        }
    }
}
