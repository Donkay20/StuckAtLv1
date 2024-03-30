using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SquirrelBall : MonoBehaviour
{
    private readonly int SQUIRRELBALL_BASE_DMG = 3;
    private readonly float SQUIRRELBALL_BASE_DURATION = 3f;
    Slot slot;
    CircleCollider2D ballCollider;
    private float timer;
    private int damage;
    private float size;
    private bool hitCheck;
    private float hitRefreshRate = 0.3f;
    private Character character;
    [SerializeField] private Sprite properSprite;

    void Start() {
        slot = GetComponentInParent<Slot>();
        AttackSlotBonus asb = FindAnyObjectByType<AttackSlotBonus>();
        ballCollider = GetComponent<CircleCollider2D>();
        character = FindAnyObjectByType<Character>();

        damage = asb.GetDamageBonus(slot, SQUIRRELBALL_BASE_DMG);

        size = asb.GetSizeBonus(slot);
        transform.localScale = new Vector2(size, size);

        timer = asb.GetDurationBonus(slot, SQUIRRELBALL_BASE_DURATION);

        character.GainAfterimage(1, false);

        GetComponent<SpriteRenderer>().sprite = properSprite;
    }

    void Update() {
        timer -= Time.deltaTime;
        if (timer <= 0) {
            Destroy(gameObject);
        }

        if (hitRefreshRate > 0 && !hitCheck) {
            hitRefreshRate -= Time.deltaTime;
        }

        if (hitRefreshRate <= 0 && !hitCheck) {
            hitCheck = true;
        }
    }

    private void ResetHitCheck() {
        hitCheck = false;
        hitRefreshRate = 0.3f;
    }

    private void OnTriggerStay2D(Collider2D col) {
        if (hitCheck) {
            Collider2D[] enemyColliders = Physics2D.OverlapCircleAll(transform.position, ballCollider.radius, LayerMask.GetMask("Enemy"));
            Collider2D[] passThroughEnemyColliders = Physics2D.OverlapCircleAll(transform.position, ballCollider.radius, LayerMask.GetMask("PassThroughEnemy"));
            
            foreach (Collider2D c in enemyColliders.Concat(passThroughEnemyColliders)) {
                character.GainAfterimage(0.1f, false);
                Enemy enemy = c.GetComponent<Enemy>();
                FindAnyObjectByType<OnHitBonus>().ApplyDamageBonus(slot, enemy, damage);
            }
            ResetHitCheck();
        }
    }
}