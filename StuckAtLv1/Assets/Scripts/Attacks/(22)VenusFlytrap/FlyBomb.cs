using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyBomb : MonoBehaviour
{
    [SerializeField] private GameObject explosion;
    [SerializeField] private CircleCollider2D explosionCollision;
    [SerializeField] private FlyBombExplosion explosionScript;
    [SerializeField] private Sprite explosionSprite;
    private readonly float FLYBOMB_BASE_DURATION = 2f;
    private float duration, maxDuration;
    private float size, maxSize;
    private Slot slot;
    private float timer;
    private readonly float EXPLOSION_MAX_TIMER = 2;
    private bool exploding;
    void Start() {
        slot = FindAnyObjectByType<AttackSpawner>().GetParent();
        AttackSlotBonus asb = FindAnyObjectByType<AttackSlotBonus>();

        maxDuration = asb.GetDurationBonus(slot, FLYBOMB_BASE_DURATION);

        size = asb.GetSizeBonus(slot); maxSize = size * 2;
        transform.localScale = new Vector2(size, size);
    }

    void Update() {
        if (!exploding) {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer/EXPLOSION_MAX_TIMER);
            float tr = Mathf.Lerp(0, maxSize, t);
            explosion.transform.localScale = new Vector2(tr, tr);

            if (timer >= EXPLOSION_MAX_TIMER) {
                exploding = true;
                Explode();
            }
        } else {
            duration += Time.deltaTime;
            float t = Mathf.Clamp01(duration/maxDuration);
            float tr = Mathf.Lerp(0, maxSize, t);
            explosion.transform.localScale = new Vector2(tr, tr);

            if (duration >= maxDuration) {
                Destroy(gameObject);
            }
        }
    }

    private void Explode() {
        GetComponent<SpriteRenderer>().enabled = false;
        explosion.GetComponent<SpriteRenderer>().sprite = explosionSprite;
        explosionCollision.enabled = true;
    }
}