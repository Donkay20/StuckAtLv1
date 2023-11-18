using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSiphon : MonoBehaviour
{
    [SerializeField] private float timer = 2f;
    [SerializeField] private GameObject siphon;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(siphon, transform.position, transform.rotation);
        Instantiate(siphon, transform.position, transform.rotation);
        Instantiate(siphon, transform.position, transform.rotation);
        Instantiate(siphon, transform.position, transform.rotation);
        Instantiate(siphon, transform.position, transform.rotation);
        Instantiate(siphon, transform.position, transform.rotation);
        Instantiate(siphon, transform.position, transform.rotation);
        Instantiate(siphon, transform.position, transform.rotation);
        Instantiate(siphon, transform.position, transform.rotation);
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
