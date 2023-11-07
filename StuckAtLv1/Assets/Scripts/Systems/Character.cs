using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public int maxHp = 10;
    public int currentHp = 10;
    [SerializeField] StatusBar hpBar;

    public void TakeDamage(int damage) {
        currentHp -= damage;
        if (currentHp <= 0) {
            Debug.Log("Jamp is dead.");
        }
        hpBar.SetState(currentHp, maxHp);
    }

    public void Heal(int amount) {
        if (currentHp <= 0) {
            return;
        }
        currentHp += amount;
        if (currentHp > maxHp) {
            currentHp = maxHp;
        }
    }
}
