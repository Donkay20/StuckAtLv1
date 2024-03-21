using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RottenAppleParent : MonoBehaviour
{
    private readonly float ROTTEN_APPLE_GLOBAL_DURATION = 3f;
    private readonly float ROTTEN_APPLE_SPEED = 6f;
    private Slot slot;
    private float duration;
    private Vector3 mousePosition;
    private Camera mainCamera;
    Rigidbody2D rb;

    void Start() {
        RotateTowardsMouse();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody2D>();
        slot = FindAnyObjectByType<AttackSpawner>().GetParent();
        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - transform.position;
        rb.velocity = direction.normalized * ROTTEN_APPLE_SPEED;
        AttackSlotBonus asb = FindAnyObjectByType<AttackSlotBonus>();

        duration = asb.GetDurationBonus(slot, ROTTEN_APPLE_GLOBAL_DURATION);
    }

    void Update() {
        duration -= Time.deltaTime;
        if (duration <= 0) {
            Destroy(gameObject);
        }
    }

    private void RotateTowardsMouse() {
        Vector2 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
        transform.up = direction;
    }
}
