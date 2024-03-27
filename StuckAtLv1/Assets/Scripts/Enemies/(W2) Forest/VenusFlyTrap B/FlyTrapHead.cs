using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyTrapHead : MonoBehaviour
{
    private readonly int HEAD_DAMAGE = 5;
    private BuffManager buffManager; 
    private Animator anim;
    void Start() {
        anim = GetComponent<Animator>();
        buffManager = FindAnyObjectByType<BuffManager>();
    }

    public void BiteAttack() {
        anim.SetTrigger("Bite");
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.collider.TryGetComponent<Character>(out var player)) {
            player.TakeDamage(HEAD_DAMAGE);
        }
    }
}
