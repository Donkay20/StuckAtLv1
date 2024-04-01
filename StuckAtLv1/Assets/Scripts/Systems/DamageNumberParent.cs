using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageNumberParent : MonoBehaviour
{
    private float lifetime = 1f;
    private void Update() {
        lifetime -= Time.deltaTime;
        
        if (lifetime <= 0) {
            Destroy(gameObject);
        }
    }
    public void BattleOver() {
        Destroy(gameObject);
    }
}
