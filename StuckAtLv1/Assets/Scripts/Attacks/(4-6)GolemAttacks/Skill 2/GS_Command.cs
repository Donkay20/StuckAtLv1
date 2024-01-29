using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GS_Command : MonoBehaviour
{
    private Slot parent;
    private AttackSpawner command;
    private readonly int GROUNDSLAM_ID = 5;

    private void Start() {
        parent = GetComponentInParent<Slot>();
        command = FindAnyObjectByType<AttackSpawner>();
        command.SpawnAttack(GROUNDSLAM_ID, parent);
        Destroy(gameObject);
    }
}
