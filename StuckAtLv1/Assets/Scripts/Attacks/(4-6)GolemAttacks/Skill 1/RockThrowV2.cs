using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockThrowV2 : MonoBehaviour
{
    private readonly float ROCKTHROW_BASE_TIMER = 1f;
    private readonly int ROCKTHROW_BASE_DMG = 10;
    Rigidbody2D rb;
    Slot slot;
    private Vector3 mousePosition;
    private Camera mainCamera;
    [SerializeField] private GameObject aoeAttack;
    public float speed;
    private int damage;
    private float timer;
    private float size;

    void Start() {  //aim towards the mouse
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody2D>();
        slot = GetComponentInParent<Slot>();
        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePosition - transform.position;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * speed;
        AttackSlotBonus asb = FindAnyObjectByType<AttackSlotBonus>();

        size = asb.GetSizeBonus(slot);
        transform.localScale = new Vector2(size, size);

        timer = asb.GetDurationBonus(slot, ROCKTHROW_BASE_TIMER);

        damage = asb.GetDamageBonus(slot, ROCKTHROW_BASE_DMG);
    }

    void Update() {
        timer -= Time.deltaTime;
        if (timer <= 0) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D col) {
        Enemy enemy = col.GetComponent<Enemy>();
        if (enemy != null) {
            FindAnyObjectByType<OnHitBonus>().ApplyDamageBonus(slot, enemy, damage);
            Instantiate(aoeAttack, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
