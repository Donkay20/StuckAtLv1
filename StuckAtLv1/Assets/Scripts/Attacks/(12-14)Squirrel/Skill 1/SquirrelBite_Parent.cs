using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquirrelBite_Parent : MonoBehaviour
{
    private Vector2 mousePosition;
    private Camera mainCamera;
    void Start() {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePosition;
    }

    public void DestroyParent() {
        Destroy(gameObject);
    }
}
