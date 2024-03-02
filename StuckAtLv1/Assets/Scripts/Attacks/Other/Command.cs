using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Command : MonoBehaviour
{
    private Slot parent;
    private AttackSpawner command;
    [SerializeField] private int id;

    private void Start() {
        parent = GetComponentInParent<Slot>();
        command = FindAnyObjectByType<AttackSpawner>();
        command.SpawnAttack(id, parent);
        Destroy(gameObject);
    }
}
