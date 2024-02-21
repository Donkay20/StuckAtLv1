using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningSlashRotation : MonoBehaviour
{
    [SerializeField] private SpinningSlashSword sword;
    private float duration = 2f;
    private Slot slot;
    void Start() {
        slot = GetComponentInParent<Slot>();
        sword.Activate(slot);
        duration *= 1 + (slot.GetCommonUpgrade(2)*0.2f + slot.GetRareUpgrade(2)*0.4f + slot.GetLegendaryUpgrade(2)*0.6f);
    }

    void Update() {
        duration -= Time.deltaTime;
        transform.Rotate(0, 0, -1.5f);
        if (duration <= 0) {
            Destroy(gameObject);
        }
    }
}
