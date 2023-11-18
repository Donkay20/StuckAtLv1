using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSiphon : MonoBehaviour
{
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
        
    }
}
