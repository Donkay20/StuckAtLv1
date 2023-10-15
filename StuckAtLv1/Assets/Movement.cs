using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class Movement : MonoBehaviour
{
    Rigidbody2D body;
    Vector3 movementVector;
    [SerializeField] float speed = 3f;
    private void Awake() {
        body = GetComponent<Rigidbody2D>();
        movementVector = new Vector3();
    }

    void Update()
    {
        movementVector.x = Input.GetAxisRaw("Horizontal");
        movementVector.y = Input.GetAxisRaw("Vertical");
        movementVector *= speed;
        body.velocity = movementVector;
    }
}