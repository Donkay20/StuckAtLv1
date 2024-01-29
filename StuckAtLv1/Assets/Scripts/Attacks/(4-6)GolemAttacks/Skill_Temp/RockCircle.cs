using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockCircle : MonoBehaviour
{
    [SerializeField] float timer = 10f;
    // Start is called before the first frame update
    void Start()
    {
        
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
