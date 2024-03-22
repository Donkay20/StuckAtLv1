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
    3: Upheaval
    5: Ground Slam
    6: Fissure
    8: Ensnaring Web
    12: Squirrel Bite
    15: Apple Shotgun
    16: Tree Branch
    17: Vine Snare
    */
    
    public void SpawnAttack(int id, Slot s) {
        parent = s;
        Instantiate(attacks[id], player.transform.position, Quaternion.identity, transform);
    }

    public Slot GetParent() {
        return parent;
    }
}
