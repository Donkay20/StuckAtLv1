using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroup : MonoBehaviour
{
    [SerializeField] private float lifetime;
    [SerializeField] private GameObject[] enemiesInGroup;
    [SerializeField] private bool Left, Right, Up, Down;
    private Character target;

    private void Start() {
        target = FindAnyObjectByType<Character>();
        EnemyGroupTargetAssignment();

        if (Left) {FindAnyObjectByType<CombatManager>().WarningAnimation("Left");}
        if (Right) {FindAnyObjectByType<CombatManager>().WarningAnimation("Right");}
        if (Up) {FindAnyObjectByType<CombatManager>().WarningAnimation("Left");}
        if (Down) {FindAnyObjectByType<CombatManager>().WarningAnimation("Right");}
    }

    void Update() {
        if (lifetime > 0) {
            lifetime -= Time.deltaTime;
        }

        if (lifetime <= 0) {
            Destroy(gameObject);
        }

        if (Left) {
            transform.Translate(10 * Time.deltaTime * Vector2.right);
            //shift left
        }

        if (Right) {
            transform.Translate(10 * Time.deltaTime * Vector2.left);
            //shift right
        }

        if (Up) {
            transform.Translate(8 * Time.deltaTime * Vector2.down);
            //shift down
        }

        if (Down) {
            transform.Translate(8 * Time.deltaTime * Vector2.up);
            //shift up
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