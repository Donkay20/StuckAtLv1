using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatWave : MonoBehaviour
{
    [SerializeField] GameObject[] bats;
    
    // The bat wave needs to destroy one random bat to allow Jamp to pass through. 
    // This method is called every time one is spawned by evil jamp.
    public void DestroyBat(int victim) {
        Destroy(bats[victim]);
    }
}