using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChaosFissure : MonoBehaviour
{
    private readonly int FISSURE_BASE_DAMAGE = 2;
    private bool hitCheck;
    private float hitRefreshRate = 0.5f;

    void Update() {
        if (hitRefreshRate > 0 && !hitCheck) {
            hitRefreshRate -= Time.deltaTime;
        }

        if (hitRefreshRate <= 0 && !hitCheck) {
            hitCheck = true;
        }
    }

    private void ResetHitCheck() {
        hitCheck = false;
        hitRefreshRate = 0.5f;
    }

    private void OnTriggerStay2D(Collider2D col) {
        if (hitCheck) {
            if (col.TryGetComponent<Character>(out var player)) {
                BuffManager b = FindAnyObjectByType<BuffManager>();
                b.AddDebuff("slow", 0.5f, 0.5f);
                player.TakeDamage(FISSURE_BASE_DAMAGE);
            }
            ResetHitCheck();
        }
    }
}