using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upheaval : MonoBehaviour
{
    private readonly int UPHEAVAL_BASE_DMG = 5;
    private readonly float UPHEAVAL_BASE_TIMER = 1.5f;
    CircleCollider2D atkCollider;
    Slot slot;
    Animator anim;
    private Vector2 mousePosition;
    private Camera mainCamera;
    private float maxSize, initialSize = 0;
    private int damage;
    private bool active;
    private float timeToAttack = 0.75f; private float activeTime;
    private float timer;
    
    [SerializeField] private Sprite upheavalActive;
    void Start() {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        slot = FindAnyObjectByType<AttackSpawner>().GetParent();
        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePosition;
        atkCollider = GetComponent<CircleCollider2D>();
        anim = GetComponent<Animator>();
        AttackSlotBonus asb = FindAnyObjectByType<AttackSlotBonus>();

        transform.localScale = new Vector2(0, 0);

        maxSize = asb.GetSizeBonus(slot);
        timer = asb.GetDurationBonus(slot, UPHEAVAL_BASE_TIMER);
        damage = asb.GetDamageBonus(slot, UPHEAVAL_BASE_DMG);
    }

    void Update() {
        if (activeTime < timeToAttack && !active) {
            activeTime += Time.deltaTime;
            float time = Mathf.Clamp01(activeTime/timeToAttack);
            float currentRadius = Mathf.Lerp(initialSize, maxSize, time);
            SetSize(currentRadius);
        }

        if (activeTime >= timeToAttack && !active) {
            ActivateAttack();
            active = true;
        }

        if (activeTime >= timeToAttack && active) {
            timer -= Time.deltaTime;
            if (timer <= 0) {
                Destroy(gameObject);
            }
        }
    }

    private void SetSize(float radius) {
        transform.localScale = new Vector2(radius, radius);
    }

    private void ActivateAttack() {
        atkCollider.enabled = true;
        anim.SetTrigger("active");
    }

    private void OnTriggerEnter2D(Collider2D col) {
        Enemy enemy = col.GetComponent<Enemy>();
        FindAnyObjectByType<OnHitBonus>().ApplyDamageBonus(slot, enemy, damage);
    }
}
