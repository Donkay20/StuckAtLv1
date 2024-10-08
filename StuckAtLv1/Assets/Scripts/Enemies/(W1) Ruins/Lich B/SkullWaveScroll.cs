using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullWaveScroll : MonoBehaviour
{
    [SerializeField] private bool movement;
    [SerializeField] private GameObject parent;
    private float timer = 15f;
    void Update() {
        
        if (movement) { 
            transform.position += new Vector3(0, -0.1f, 0); 
        } else { 
            transform.position += new Vector3(-0.1f, 0, 0); 
        }

        timer -= Time.deltaTime;
        if (timer <= 0) {
            if (movement) {Destroy(parent);}
            Destroy(gameObject);
        }
    }
}