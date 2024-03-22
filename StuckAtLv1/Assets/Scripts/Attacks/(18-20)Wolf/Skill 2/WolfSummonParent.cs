using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class WolfSummonParent : MonoBehaviour
{
    [SerializeField] private GameObject wolfSummon;
    [SerializeField] private GameObject spawnPoint1, spawnPoint2;
    private readonly float WOLF_SUMMON_GLOBAL_DURATION = 10f;
    private float duration;
    Slot slot;
    void Start() {
        slot = FindAnyObjectByType<AttackSpawner>().GetParent();
        AttackSlotBonus asb = FindAnyObjectByType<AttackSlotBonus>();
        duration = asb.GetDurationBonus(slot, WOLF_SUMMON_GLOBAL_DURATION);

        Instantiate(wolfSummon, spawnPoint1.transform.position, Quaternion.identity, transform);
        Instantiate(wolfSummon, spawnPoint2.transform.position, Quaternion.identity, transform);
    }

    void Update() {
        duration -= Time.deltaTime;
        if (duration <= 0) {
            Destroy(gameObject);
        }
    }

    public Slot GetSlot() {
        return slot;
    }
}
