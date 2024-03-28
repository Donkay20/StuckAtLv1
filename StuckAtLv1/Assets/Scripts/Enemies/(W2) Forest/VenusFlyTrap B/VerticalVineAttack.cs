using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalVineAttack : MonoBehaviour
{
    private readonly float MAGNITUDE = 7f;
    private readonly int VINE_DAMAGE = 3;
    private BuffManager buffManager;
    private Rigidbody2D rb;
    [SerializeField] private GameObject parent;
    void Start() {
        buffManager = FindAnyObjectByType<BuffManager>();

        rb = GetComponent<Rigidbody2D>();
        rb.velocity = -transform.up * MAGNITUDE;
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.TryGetComponent<Character>(out var player)) {
            player.TakeDamage(VINE_DAMAGE);
            buffManager.AddDebuff("slow", 0.5f, 1);
            buffManager.AddDebuff("anemia", 0.2f, 3);
        }

        if (col.gameObject.layer == LayerMask.NameToLayer("Wall")) {
            Destroy(parent);
        }
    }
}
