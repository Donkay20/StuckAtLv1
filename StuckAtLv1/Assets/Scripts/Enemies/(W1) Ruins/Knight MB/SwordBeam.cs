using System;
using UnityEngine;

public class SwordBeam : MonoBehaviour
{
    private readonly int SWORD_BEAM_DMG = 1;
    private readonly float SWORD_BEAM_SPD = 2f;
    private float lifetime = 5f;
    [SerializeField] private Character targetCharacter;
    [SerializeField] private GameObject targetGameObject;
    private Rigidbody2D rb;
    private UnityEngine.Vector2 direction;
    private GameObject swordPos;
    void Start()
    {
        swordPos = GameObject.FindWithTag("KnightSword");
        transform.position = swordPos.transform.position;

        targetGameObject = FindAnyObjectByType<Character>().gameObject;
        targetCharacter = targetGameObject.GetComponent<Character>();

        rb = GetComponent<Rigidbody2D>();

        transform.right = targetGameObject.transform.position - transform.position;
        direction = (targetGameObject.transform.position - transform.position).normalized;
    }

    void Update() {
        lifetime -= Time.deltaTime;
        rb.velocity = direction * SWORD_BEAM_SPD;
        if (lifetime <= 0) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.layer == LayerMask.NameToLayer("Player")) {
            Attack();
            Destroy(gameObject);
        }
    }

    private void Attack() {
        Debug.Log("attacked player");
        if (targetCharacter != null) {
            targetCharacter.TakeDamage(SWORD_BEAM_DMG);
        } else {
            throw new NullReferenceException("ERROR: Null instance found; Knight's Sword Beam.");
        }
    }
}
