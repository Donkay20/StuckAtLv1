using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraMoney : MonoBehaviour
{
    [SerializeField] private int moneyInside;

    public void DestroyExtraMoney() {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D col) {
        Character c = col.GetComponent<Character>();
        c.GainMoney(moneyInside);
        Destroy(gameObject);
    }
}
