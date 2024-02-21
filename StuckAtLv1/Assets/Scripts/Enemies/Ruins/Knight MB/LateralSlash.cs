using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LateralSlash : MonoBehaviour
{
    private float warningTime = 0f; 
    private float activeTime = 0.3f;
    private readonly int LATERAL_SLASH_DAMAGE = 2;
    private readonly float LATERAL_SLASH_MAX_TIMER = 1.7f;
    private bool attackProcced;
    BoxCollider2D attackCollider;
    SpriteRenderer attackRenderer;
    [SerializeField] private Sprite activeLaterSlashSprite;
    [SerializeField] private Character targetCharacter;
    [SerializeField] private GameObject parent;
    void Start()
    {
        attackCollider = GetComponent<BoxCollider2D>();
        attackRenderer = GetComponent<SpriteRenderer>();
        targetCharacter = FindAnyObjectByType<Character>(); 
        transform.localScale = new Vector2(0, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (warningTime < LATERAL_SLASH_MAX_TIMER && !attackProcced) {
            warningTime += Time.deltaTime;
            float time = Mathf.Clamp01(warningTime/LATERAL_SLASH_MAX_TIMER);
            float currentRadius = Mathf.Lerp(0, 1, time);
            SetSize(currentRadius);
        }

        if (warningTime >= LATERAL_SLASH_MAX_TIMER) {
            ActivateAttack();
            attackProcced = true;
        }

        if (attackProcced) {
            activeTime -= Time.deltaTime;
            if (activeTime <= 0) {
                Destroy(parent);
                Destroy(gameObject);
            }
        }
    }

    private void SetSize(float radius) {
        transform.localScale = new Vector2(radius, 1);
    }

    private void ActivateAttack() {
        attackCollider.enabled = true;
        attackRenderer.sprite = activeLaterSlashSprite;
    }

    private void OnCollisionStay2D(Collision2D col) {
        if (col.gameObject.layer == LayerMask.NameToLayer("Player")) {
            Attack();
            Destroy(gameObject);
        }
    }

    private void Attack() {
        Debug.Log("attacked player");
        if (targetCharacter != null) {
            targetCharacter.TakeDamage(LATERAL_SLASH_DAMAGE);
        } else {
            throw new NullReferenceException("ERROR: Null instance found; Knight's Lateral Slash.");
        }
    }
}
