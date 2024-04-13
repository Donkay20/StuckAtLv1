using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MagicCircle : MonoBehaviour
{
    [Header("Outer")]
    [SerializeField] private PolygonCollider2D outerCollider;
    [Header("Inner")]
    [SerializeField] private CircleCollider2D innerCollider;
    private Slot slot;
    private readonly float MAGIC_CIRCLE_BASE_DURATION = 5f;
    private float size;
    private float duration;
    void Start() {
        slot = FindAnyObjectByType<AttackSpawner>().GetParent();
        AttackSlotBonus asb = FindAnyObjectByType<AttackSlotBonus>();

        size = asb.GetSizeBonus(slot); transform.localScale = new Vector2(size, size);
        duration = asb.GetDurationBonus(slot, MAGIC_CIRCLE_BASE_DURATION);
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

    public void IntroAnimationFinished() {
        innerCollider.enabled = false;
        outerCollider.enabled = true;
    }
}