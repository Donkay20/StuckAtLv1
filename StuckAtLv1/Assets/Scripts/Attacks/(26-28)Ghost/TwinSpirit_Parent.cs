using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwinSpirit_Parent : MonoBehaviour
{
    [SerializeField] private GameObject spirit;
    private Slot slot;
    private readonly float SPIRIT_SPAWN_TIME = 1f;
    private readonly float SPIRIT_SUMMON_DURATION = 3f;
    private float duration;
    private float tickTime;
    void Start() {
        slot = GetComponentInParent<Slot>();
        tickTime = SPIRIT_SPAWN_TIME;
        AttackSlotBonus asb = FindAnyObjectByType<AttackSlotBonus>();
        duration = asb.GetDurationBonus(slot, SPIRIT_SUMMON_DURATION);
    }
    
    void Update() {
        duration -= Time.deltaTime;
        tickTime -= Time.deltaTime;

        if (duration <= 0) {
            Destroy(gameObject);
        }

        if (tickTime <= 0) {
            SpawnGhost();
        }
    }

    private void SpawnGhost() {
        GameObject v = Instantiate(spirit, transform);
        v.GetComponent<TwinSpirit>().AssignSlot(slot);
        tickTime = SPIRIT_SPAWN_TIME;
    }

    public Slot GetSlot() {
        return slot;
    }
}