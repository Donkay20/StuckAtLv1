using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class SwordBeam : MonoBehaviour
{
    private readonly int SWORD_BEAM_DMG = 2;
    private readonly float SWORD_BEAM_SPD = 1f;
    private float lifetime = 5f;
    [SerializeField] private Character targetCharacter;
    [SerializeField] private GameObject targetGameObject;
    private Rigidbody2D rb;
    private UnityEngine.Vector3 direction;
    void Start()
    {
        GameObject startingPos = FindAnyObjectByType<Knight>().gameObject;
        transform.position = startingPos.transform.position + transform.forward*3;

        targetGameObject = FindAnyObjectByType<Character>().gameObject;
        targetCharacter = targetGameObject.GetComponent<Character>();

        rb = GetComponent<Rigidbody2D>();

        transform.right = targetGameObject.transform.position - transform.position;
        direction = targetGameObject.transform.position - transform.position;
    }

    void FixedUpdate()
    {
        rb.velocity = direction * SWORD_BEAM_SPD;
    }
    
    void Update() {
        lifetime -= Time.deltaTime;

        if (lifetime <= 0) {
            Destroy(gameObject);
        }
    }

    private void OnCollisionStay2D(Collision2D col) {
        if (col.gameObject == targetGameObject) {
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
