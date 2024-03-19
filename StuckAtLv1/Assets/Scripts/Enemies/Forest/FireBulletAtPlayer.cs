using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBulletAtPlayer : MonoBehaviour
{
    [SerializeField] private GameObject enemyProjectile;
    [SerializeField] private float rateOfFire;
    [SerializeField] private float timer;
    void Start() {
        timer = rateOfFire / 2;
    }

    void Update() {
        if (timer > 0) {
            timer -= Time.deltaTime;
        }
        
        if (timer <= 0) {
            Instantiate(enemyProjectile, transform.position, Quaternion.identity, transform.parent.transform);
            timer = rateOfFire;
        }
    }
}
