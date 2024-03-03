using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Web : MonoBehaviour
{
    [SerializeField] private Sprite fullWeb;
    Rigidbody2D rb;
    private Vector3 mousePosition;
    private Camera mainCamera;
    private Slot slot;
    private readonly float WEB_BALL_BASE_TIMER = 1f;
    private readonly float WEB_ACTIVE_BASE_TIMER = 2f;
    private readonly int WEB_BASE_DAMAGE = 2;
    private float speed = 5;
    private float ballTimer; private float webTimer;
    private float initialSize; private float expandedSize;
    private bool skillHit;
    private int damage;
    void Start() {
        skillHit = false;
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody2D>();
        slot = FindAnyObjectByType<AttackSpawner>().GetParent();
        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - transform.position;
        rb.velocity = direction.normalized * speed;
        AttackSlotBonus asb = FindAnyObjectByType<AttackSlotBonus>();

        initialSize = asb.GetSizeBonus(slot);
        expandedSize = initialSize * 5; 
        transform.localScale = new Vector2(initialSize, initialSize);

        ballTimer = asb.GetDurationBonus(slot, WEB_BALL_BASE_TIMER);
        webTimer = asb.GetDurationBonus(slot, WEB_ACTIVE_BASE_TIMER);

        damage = asb.GetDamageBonus(slot, WEB_BASE_DAMAGE);
    }

    void Update() {
        if (!skillHit) {
            ballTimer -= Time.deltaTime;
            if (ballTimer <= 0) {
                Destroy(gameObject);
            }
        }

        if (skillHit) {
            webTimer -= Time.deltaTime;
            if(webTimer <= 0) {
                Destroy(gameObject);
            }
        }
    }

    private void TransformIntoWeb() {
        Debug.Log("transformed into web");
        GetComponent<SpriteRenderer>().sprite = fullWeb;
        transform.localScale = new Vector2(expandedSize, expandedSize);
        skillHit = true;
        rb.velocity = Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D col) {
        Enemy enemy = col.GetComponent<Enemy>();
        if (enemy != null) {
            if(!skillHit) {TransformIntoWeb();}
            enemy.ApplyStun(1f);
        }
        FindAnyObjectByType<OnHitBonus>().ApplyDamageBonus(slot, enemy, damage);
    }
}
