using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpLaserParent : MonoBehaviour
{
    private Vector3 mousePosition;
    private Camera mainCamera;
    private Slot slot;
    private Vector2 direction;
    private readonly float IMP_BEAM_BASE_DURATION = 3f;
    private float duration;
    private float size;
    void Start() {
        slot = GetComponentInParent<Slot>();
        AttackSlotBonus asb = FindAnyObjectByType<AttackSlotBonus>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        size = asb.GetSizeBonus(slot); transform.localScale = new Vector2(size, size);
        duration = asb.GetDurationBonus(slot, IMP_BEAM_BASE_DURATION);
    }

    void Update() {
        RotateTowardsMouse();

        duration -= Time.deltaTime;
        if (duration <= 0) {
            Destroy(gameObject);
        }
    }

    private void RotateTowardsMouse() {
        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        direction = mousePosition - transform.position;
        transform.up = direction;
    }

    public Slot GetSlot() {
        return slot;
    }
}