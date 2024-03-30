using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquirrelTailRotate : MonoBehaviour
{
    [SerializeField] private SquirrelTail tail;
    private readonly float SQUIRREL_TAIL_BASE_DURATION = 4f;
    private readonly float SQUIRREL_TAIL_ROTATION_SPEED = 10f;
    private readonly float SWAP_TIMER = 0.3f;
    private float duration;
    private float swap = 0.15f;
    private bool direction;
    private Slot slot;

    [SerializeField] Sprite rightTail, leftTail;
    private SpriteRenderer sr;
    
    void Start() {
        RotateTowardsMouse();
        slot = GetComponentInParent<Slot>();
        AttackSlotBonus asb = FindAnyObjectByType<AttackSlotBonus>();
        tail.Activate(slot);
        duration = asb.GetDurationBonus(slot, SQUIRREL_TAIL_BASE_DURATION);

        sr = tail.gameObject.GetComponent<SpriteRenderer>();
    }

    void Update() {
        duration -= Time.deltaTime;
        swap -= Time.deltaTime;

        if (duration <= 0) {
            Destroy(gameObject);
        }

        if (swap > 0) {
            if (direction) {
                transform.Rotate(0, 0, SQUIRREL_TAIL_ROTATION_SPEED);
            } else {
                transform.Rotate(0, 0, -SQUIRREL_TAIL_ROTATION_SPEED);
            }
        }

        if (swap <= 0) {
            if (direction) {
                direction = false;
                sr.sprite = rightTail;
            } else {
                direction = true;
                sr.sprite = leftTail;
            }
            swap = SWAP_TIMER;
        }
    }

    private void RotateTowardsMouse() {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
        transform.up = direction;
    }
}