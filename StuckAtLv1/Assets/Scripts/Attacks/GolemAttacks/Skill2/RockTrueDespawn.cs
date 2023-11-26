using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockTrueDespawn : MonoBehaviour
{
    float timer = 0.2f;
    private RockWallSpawner spawn;

    // Start is called before the first frame update
    void Start()
    {
        spawn = GameObject.Find("RockWallSpawner").GetComponent<RockWallSpawner>();
        spawn.Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0) {
            Destroy(gameObject);
        }
    }
}
