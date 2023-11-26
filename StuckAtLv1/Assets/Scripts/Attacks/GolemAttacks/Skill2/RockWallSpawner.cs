using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockWallSpawner : MonoBehaviour
{
    [SerializeField] GameObject rockWall;
    private Camera mainCamera;
    private Vector2 mousePos;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
    }

    public void Spawn()
    {
        Instantiate(rockWall, mousePos, Quaternion.identity);
    }
}
