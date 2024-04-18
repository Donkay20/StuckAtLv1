using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDrainRotate : MonoBehaviour
{
    private Slot slot;
    private readonly float LEVEL_DRAIN_BASE_DURATION = 2;
    private Vector3 mousePosition;
    private Camera mainCamera;
    private float size;
    private float duration;
    void Start() {
        slot = GetComponentInParent<Slot>();
        AttackSlotBonus asb = FindAnyObjectByType<AttackSlotBonus>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        RotateTowardsMouse();

        size = asb.GetSizeBonus(slot);
        transform.localScale = new Vector2(size, size);
        duration = asb.GetDurationBonus(slot, LEVEL_DRAIN_BASE_DURATION);
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

    private void RotateTowardsMouse() {
        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - transform.position;
        transform.up = direction;
    }
}