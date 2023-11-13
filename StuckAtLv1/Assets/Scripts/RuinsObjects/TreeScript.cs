using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeScript : MonoBehaviour
{
    private Transform playerTransform;
    
    private SpriteRenderer rend;
    [SerializeField] float yOffset = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        playerTransform = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y > playerTransform.position.y + yOffset)
        {
            rend.sortingOrder = 2;
        }
        else
        {
            rend.sortingOrder = 10;
        }
    }
}
