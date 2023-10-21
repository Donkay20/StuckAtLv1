using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeScript : MonoBehaviour
{
    public Transform playerTransform;
    
    private SpriteRenderer rend;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y > playerTransform.position.y + .5f)
        {
            rend.sortingOrder = 1;
        }
        else
        {
            rend.sortingOrder = 10;
        }
    }
}
