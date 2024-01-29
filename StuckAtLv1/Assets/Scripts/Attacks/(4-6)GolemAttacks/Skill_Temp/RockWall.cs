using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockWall : MonoBehaviour
{
    private Animator anim;

    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
        anim.SetTrigger("Start");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
