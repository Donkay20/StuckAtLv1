using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathParticle : MonoBehaviour
{
    public float deathTime;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        EndLife();
    }

    void EndLife()
    {
        Destroy(this.gameObject, deathTime);
    }
}
