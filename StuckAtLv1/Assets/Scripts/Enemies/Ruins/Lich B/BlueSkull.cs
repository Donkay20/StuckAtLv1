using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class BlueSkull : MonoBehaviour
{
    private readonly int BLUE_SKULL_DMG = 2;
    [SerializeField] private Character targetCharacter;
    [SerializeField] private GameObject targetGameObject;
    void Start()
    {
        targetGameObject = FindAnyObjectByType<Character>().gameObject;
        targetCharacter = targetGameObject.GetComponent<Character>();
    }

    private void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.layer == LayerMask.NameToLayer("Player")) {
            Attack();
            Destroy(gameObject);
        }
    }

    private void Attack() {
        Debug.Log("attacked player");
        if (targetCharacter != null) {
            targetCharacter.TakeDamage(BLUE_SKULL_DMG);
        } else {
            throw new NullReferenceException("ERROR: Null instance found; Knight's Sword Beam.");
        }
    }
}
