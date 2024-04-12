using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeCannon : MonoBehaviour
{
    [SerializeField] private GameObject slimeBullet;
    [SerializeField] private GameObject[] cannonBarrels = new GameObject[5];
    private readonly float SLIME_BULLET_SPEED = 8f;
    private readonly float SLIME_CANNON_RELOAD_TIME = 0.2f;
    private readonly float SLIME_CANNON_BASE_DURATION = 3f;
    private Vector3 mousePosition;
    private Camera mainCamera;
    private Slot slot;
    private Vector2 direction;
    private float reloadTime;
    private float size;
    private float duration;
    void Start() {
        slot = GetComponentInParent<Slot>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        AttackSlotBonus asb = FindAnyObjectByType<AttackSlotBonus>();
        size = asb.GetSizeBonus(slot); transform.localScale = new Vector2(size, size);
        duration = asb.GetDurationBonus(slot, SLIME_CANNON_BASE_DURATION);

        reloadTime = SLIME_CANNON_RELOAD_TIME;
    }

    void Update() {
        RotateTowardsMouse();

        reloadTime -= Time.deltaTime;
        if (reloadTime <= 0) {
            FireBullet();
        }

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

    private void FireBullet() {
        reloadTime = SLIME_CANNON_RELOAD_TIME;
        GameObject b = Instantiate(slimeBullet, cannonBarrels[Random.Range(0,5)].transform.position, Quaternion.identity);
        b.GetComponent<Rigidbody2D>().velocity = direction.normalized * SLIME_BULLET_SPEED;
        b.GetComponent<SlimeBullet>().AssignSlot(slot);
    }
}