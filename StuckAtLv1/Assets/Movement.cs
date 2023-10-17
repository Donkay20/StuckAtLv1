using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class Movement : MonoBehaviour
{
    Rigidbody2D body;
    Vector3 movementVector;
    Animator anim;
    SpriteRenderer sr;

    [SerializeField] float speed = 3f;


    private void Awake() {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        movementVector = new Vector3();
    }

    void Update()
    {
        movementVector.x = Input.GetAxisRaw("Horizontal");
        movementVector.y = Input.GetAxisRaw("Vertical");
        movementVector *= speed;
        body.velocity = movementVector;

        RunAnimation();
    }

    void RunAnimation()
    {
        if(movementVector.x != 0 || movementVector.y != 0)
        {
            anim.SetBool("Run", true);
        }
        else
        {
            anim.SetBool("Run", false);
        }

        if(movementVector.x > 0)
        {
            sr.flipX = false;
        }
        else if(movementVector.x < 0)
        {
            sr.flipX = true;
        }
    }
}