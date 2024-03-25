using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornSpike : MonoBehaviour
{
    [SerializeField] private Sprite thornSpikeActiveSprite;
    private float timer;
    private readonly float TIME_TO_ATTACK = 0.5f;
    private readonly int THORN_DAMAGE = 3;
    private float activeTime = 3f;
    private CircleCollider2D atkCollider;
    private bool active;
    private Character character;
    void Start() {
        atkCollider = GetComponent<CircleCollider2D>();
        character = FindAnyObjectByType<Character>();
        transform.position = character.gameObject.transform.position;
    }

    void Update() {
        if (!active) {
            timer += Time.deltaTime;
            float time = Mathf.Clamp01(timer/TIME_TO_ATTACK);
            SetSize(time);
        } else {
            activeTime -= Time.deltaTime;
        }

        if (timer >= TIME_TO_ATTACK && !active) {
            Attack();
            active = true;
        }

        if (activeTime <= 0) {
            Destroy(gameObject);
        }
    }

    private void Attack() {
        atkCollider.enabled = true;
        GetComponent<SpriteRenderer>().sprite = thornSpikeActiveSprite;
    }

    private void SetSize(float radius) {
        transform.localScale = new Vector2(radius, radius);
    }

    private void OnTriggerEnter2D(Collider2D col) {
        Character player = col.GetComponent<Character>();
        if (player != null) {
            player.TakeDamage(THORN_DAMAGE);
        }
    }
}
