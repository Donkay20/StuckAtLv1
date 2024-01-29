using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class AttackSpawner : MonoBehaviour
{
    [SerializeField] private Character player;
    [SerializeField] private GameObject[] attacks;
    Slot parent;

    public void SpawnAttack(int id, Slot s) {
        parent = s;
        switch (id) {
            case 5: //Ground Slam
                Instantiate(attacks[id], player.transform.position, Quaternion.identity, transform);
            break;
        }
    }

    public Slot GetParent() {
        return parent;
    }
}
