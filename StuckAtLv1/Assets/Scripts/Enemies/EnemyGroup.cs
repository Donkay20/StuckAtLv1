using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroup : MonoBehaviour
{
    [SerializeField] private float lifetime;
    [SerializeField] private GameObject[] enemiesInGroup;
    private Character target;

    private void Start() {
        target = FindAnyObjectByType<Character>();
        EnemyGroupTargetAssignment();
    }

    void Update() {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0) {
            Destroy(gameObject);
        }
    }

    public void BattleEnd() {
        Destroy(gameObject);
    }

    private void EnemyGroupTargetAssignment() {
        foreach (GameObject enemy in enemiesInGroup) {
            enemy.GetComponent<Enemy>().SetTarget(target.gameObject);
        }
    }
}
