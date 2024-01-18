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
    
    //Dashing Mechanic
    [SerializeField] float dashForce = 100f;
    [SerializeField] float dashTimerEnd = 0.04f;
    [SerializeField] float dashTimer = 0f;

    private bool isDashing = false;
    [SerializeField] GameObject dashSprite;

    private Character invincibility;


    private void Awake() {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        invincibility = GetComponent<Character>();

        movementVector = new Vector3();
    }

    void Update()
    {
        movementVector.x = Input.GetAxisRaw("Horizontal");
        movementVector.y = Input.GetAxisRaw("Vertical");

        if(!isDashing)
        {
            movementVector = movementVector.normalized * speed;
            body.velocity = movementVector;
        }
        
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.LeftShift) && movementVector != Vector3.zero)
        {
            isDashing = true;
            Dash();
        }

        if(isDashing)
        {
            dashTimer += Time.deltaTime;
            Debug.Log(dashTimer);

            if(dashTimer >= dashTimerEnd)
            {
                Instantiate(dashSprite, transform.position, transform.rotation);
                dashTimer = 0f;
            }
        }
        RunAnimation();
    }

    void Dash()
    {
        Debug.Log("Dashing");
        body.AddForce(movementVector * dashForce, ForceMode2D.Impulse);
        Invoke(nameof(StopDashing), 0.2f);
        invincibility.DashingIFrames();
    }

    void StopDashing(){
        isDashing = false;
        Debug.Log("Stop Dashing");
        dashTimer = 0f;
        invincibility.StopDashingIFrames();
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