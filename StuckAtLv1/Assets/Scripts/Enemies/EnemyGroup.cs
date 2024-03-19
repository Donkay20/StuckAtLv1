using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroup : MonoBehaviour
{
    [SerializeField] private float lifetime;
    private float delayTimer = 1f;
    [SerializeField] private GameObject[] enemiesInGroup;
    [SerializeField] private bool Left, Right;
    private Character target;

    private void Start() {
        target = FindAnyObjectByType<Character>();
        EnemyGroupTargetAssignment();

        if (Left) {FindAnyObjectByType<CombatManager>().WarningAnimation("Left");}
        if (Right) {FindAnyObjectByType<CombatManager>().WarningAnimation("Right");}
    }

    void Update() {
        if (lifetime < 0) {
            lifetime -= Time.deltaTime;
        }
        
        if (delayTimer < 0) {
            delayTimer -= Time.deltaTime; //delete if unused
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
