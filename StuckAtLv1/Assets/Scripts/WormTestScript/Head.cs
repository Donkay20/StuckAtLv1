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
        
    }
    
    void FixedUpdate()
    {
        MoveBodyParts();
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
            
            Rigidbody2D body = bodyParts[i].GetComponent<Rigidbody2D>();
            Vector3 force = (markM.markerList[0].position - bodyParts[i].transform.position).normalized;
            Enemy enemyScript = this.GetComponent<Enemy>();
            
            if(enemyScript.GetAlteredSpeedTimer() > 0) {
                if(enemyScript.GetBaseSpeed() > 0) {
                    body.velocity = force * (enemyScript.GetAlteredSpeed() + 0.5f);
                }
            }
            else {
                if (enemyScript.GetBaseSpeed() > 0) {
                    body.velocity = force * (enemyScript.GetBaseSpeed() + 0.5f);
                }    
            }

            

            //bodyParts[i].transform.position = Vector2.MoveTowards(bodyParts[i].transform.position, markM.markerList[0].position, step);

            //Rotate bodyparts
            float angle = Mathf.Atan2(markM.markerList[0].position.y - bodyParts[i].transform.position.y, markM.markerList[0].position.x - bodyParts[i].transform.position.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
            bodyParts[i].transform.rotation = Quaternion.RotateTowards(bodyParts[i].transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            markM.markerList.RemoveAt(0);
        }
    }
}
