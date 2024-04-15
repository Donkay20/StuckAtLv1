using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head : MonoBehaviour
{
    private Vector2 playerPos;
    [SerializeField] private float step;
    [SerializeField] private float rotationSpeed;

    [SerializeField] private List<GameObject> bodyParts = new List<GameObject>();
    
    [SerializeField] private Enemy enemyScript;

    // Start is called before the first frame update
    void Start()
    {
        enemyScript.SetTarget(FindAnyObjectByType<Character>().gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        //MoveTowardsPlayer();
        FacePlayer();
        MoveBodyParts();
    }
    
    void FixedUpdate()
    {
       
    }

    void MoveTowardsPlayer()
    {
        playerPos = GameObject.Find("Player").transform.position;
        transform.position = Vector2.MoveTowards(transform.position, playerPos, step);
    }

    void FacePlayer()
    {
        playerPos = GameObject.Find("Player").transform.position;
        Vector3 thisPos = transform.position;

        float angle = Mathf.Atan2(playerPos.y - transform.position.y, playerPos.x - transform.position.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    void MoveBodyParts()
    {
        for(int i = 1; i < bodyParts.Count; i++)
        {
            MarkerManager markM = bodyParts[i - 1].GetComponent<MarkerManager>();
            bodyParts[i].transform.position = Vector2.MoveTowards(bodyParts[i].transform.position, markM.markerList[0].position, step);
            bodyParts[i].transform.rotation = markM.markerList[0].rotation;
            markM.markerList.RemoveAt(0);
        }
    }
}
