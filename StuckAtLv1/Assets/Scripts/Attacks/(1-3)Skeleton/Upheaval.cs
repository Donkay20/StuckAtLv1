using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upheaval : MonoBehaviour
{
    private float timeToAttack = 0.75f; private float activeTime;
    private float duration = 1.5f;
    CircleCollider2D atkCollider;
    SpriteRenderer imgRenderer;
    Slot parent;
    private Vector2 mousePosition;
    private float maxSize, initialSize;
    private Camera mainCamera;
    private int damage;
    private bool active;
    [SerializeField] private Sprite upheavalActive;
    void Start() {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        parent = FindAnyObjectByType<AttackSpawner>().GetParent();
        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePosition;
        atkCollider = GetComponent<CircleCollider2D>();
        imgRenderer = GetComponent<SpriteRenderer>();
        transform.localScale = new Vector2(0, 0);

        float scalingFactor = 3 * (1 + parent.GetCommonUpgrade(1)*0.2f + parent.GetRareUpgrade(1)*0.3f + parent.GetLegendaryUpgrade(1)*0.4f);
        maxSize = scalingFactor; initialSize = 0;

        //apply duration bonus
        duration *= 1 + (parent.GetCommonUpgrade(2)*0.2f + parent.GetRareUpgrade(2)*0.4f + parent.GetLegendaryUpgrade(2)*0.6f);
        Debug.Log("duration: " + duration);

        //apply damage bonus
        damage = (int)(5 * (1+(parent.GetCommonUpgrade(0)*0.2f + parent.GetRareUpgrade(0)*0.4f + parent.GetLegendaryUpgrade(0)*0.6f)));
        Debug.Log("damage: " + damage);
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
            duration -= Time.deltaTime;
            if (duration <= 0) {
                Destroy(gameObject);
            }
        }
    }

    private void SetSize(float radius) {
        transform.localScale = new Vector2(radius, radius);
    }

    private void ActivateAttack() {
        atkCollider.enabled = true;
        imgRenderer.sprite = upheavalActive;
    }

    private void OnTriggerEnter2D(Collider2D col) {
        Enemy enemy = col.GetComponent<Enemy>();
        if (enemy != null) {
            enemy.TakeDamage(damage);
        }
    }
}
