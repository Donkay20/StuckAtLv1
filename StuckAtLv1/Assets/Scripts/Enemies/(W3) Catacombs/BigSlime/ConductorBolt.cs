using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ConductorBolt : MonoBehaviour
{
    private readonly int BOLT_DAMAGE = 30;
    private bool hitCheck;
    private float hitRefreshRate = 0.25f;

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
        hitRefreshRate = 0.25f;
    }

    private void OnTriggerStay2D(Collider2D col) {
        if (hitCheck) {
            if (col.gameObject.CompareTag("BigSlime")) {
                Enemy boss = col.gameObject.GetComponent<Enemy>(); BigSlime slime = col.gameObject.GetComponent<BigSlime>();

                slime.Electrified(); 
                boss.TakeDamage(BOLT_DAMAGE); 
                boss.ApplyStun(0.3f);
            }
            ResetHitCheck();
        }
    }
}