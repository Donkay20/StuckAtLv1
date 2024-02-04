using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class AttackSpawner : MonoBehaviour
{
    [SerializeField] private Character player;
    [SerializeField] private GameObject[] attacks;
    Slot parent;

    /*
    Possible arguments to pass:
    2: Bone Spikes
    5: Ground Slam
    6: Fissure
    */
    
    public void SpawnAttack(int id, Slot s) {
        parent = s;
        Instantiate(attacks[id], player.transform.position, Quaternion.identity, transform);
    }

    public Slot GetParent() {
        return parent;
    }
}
