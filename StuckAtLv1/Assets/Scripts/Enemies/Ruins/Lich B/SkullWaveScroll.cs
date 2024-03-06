using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullWaveScroll : MonoBehaviour
{
    [SerializeField] private bool movement;
    [SerializeField] private GameObject parent;
    private float timer = 9f;
    void Update() {
        if (movement) {transform.Translate(0, -0.04f ,0);}

        if (!movement) {transform.Translate(-0.04f, 0, 0);}

        timer -= Time.deltaTime;
        if (timer <= 0) {
            if (movement) {Destroy(parent);}
            Destroy(gameObject);
        }
    }
}
