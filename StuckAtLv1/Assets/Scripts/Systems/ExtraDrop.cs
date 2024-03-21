using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraDrop : MonoBehaviour
{
    [SerializeField] private int moneyInside, healthInside, afterimageTimeInside;
    [SerializeField] private bool money, health, afterimage;

    public void DestroyExtraDrops() {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D col) {
        Character c = col.GetComponent<Character>();
        if (c != null) {
            if (money) {c.GainMoney(moneyInside);}
            if (health) {c.Heal(healthInside);}
            Destroy(gameObject);
        }
    }
}