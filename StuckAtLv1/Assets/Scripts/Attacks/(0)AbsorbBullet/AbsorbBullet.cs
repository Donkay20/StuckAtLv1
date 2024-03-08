using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AbsorbBullet : MonoBehaviour
{
    private readonly float BASE_BULLET_LIFETIME = 1f;
    [SerializeField] float timer = 1f; //if a modifier increase skill time duration, it would call back to the parent slot and acquire the modifier for calculation
    Rigidbody2D rb;
    private Vector3 mousePosition;
    private Camera mainCamera;
    private SlotManager slotManager;
    public float speed; //if a modifier increase skill speed, it would call back to the parent slot and acquire the modifier for calculation
    [SerializeField] GameObject spawnSiphon;

    void Start() {  //follow the mouse
        slotManager = GetComponentInParent<SlotManager>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody2D>();
        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePosition - transform.position;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * speed;
    }

    private void OnEnable() {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePosition - transform.position;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * speed;
    }

    void Update() { //deactivate the bullet
        timer -= Time.deltaTime;
        if (timer <= 0) {
            timer = BASE_BULLET_LIFETIME;
            BulletPool.Instance.ReturnBullet(gameObject);
            //GetComponentInParent<Slot>().AbsorbBulletAvailable = true;  //need to make sure the absorb bullet is available again
            //Destroy(gameObject);
        }
    }

    public void UpdateTimer() {
        timer = BASE_BULLET_LIFETIME;
    }

    private void OnTriggerEnter2D(Collider2D col) {     //upon hitting an enemy:                  
        Enemy enemy = col.GetComponent<Enemy>();        //the enemy class will need to be changed to a bare-bones calculation class as every enemy will need it, can stack other classes on different enemies for unique behavior             
        if (enemy != null) {
            switch (enemy.tag) {                                        
                /*
                Each enemy will have a unique tag which will identify which one the bullet is hitting.
                //Arguments: skill ID, skill uses, skill cooldown.
                //Case-by-case for each skill; in the terms of this enemy it would return skill 1 with 3 uses, with a 1 second cooldown.
                */
                case "Skeleton1":   //Bone Toss
                    slotManager.AcquireSkill(1);                                   
                    //GetComponentInParent<Slot>().AcquireSkill(1);        
                    break;

                case "Skeleton2":   //Bone Spikes
                    slotManager.AcquireSkill(2);
                    //GetComponentInParent<Slot>().AcquireSkill(2);
                    break;

                case "Skeleton3":   //Upheaval
                    slotManager.AcquireSkill(3);
                    //GetComponentInParent<Slot>().AcquireSkill(3);
                    break;

                case "Golem1":      //Rock Throw
                    slotManager.AcquireSkill(4);
                    //GetComponentInParent<Slot>().AcquireSkill(4);
                    break;

                case "Golem2":      //Ground Slam
                    slotManager.AcquireSkill(5);
                    //GetComponentInParent<Slot>().AcquireSkill(5);
                    break;

                case "Golem3":      //Fissure
                    slotManager.AcquireSkill(6);
                    //GetComponentInParent<Slot>().AcquireSkill(6);
                    break;

                case "Spider1":     //Venom Spit
                    slotManager.AcquireSkill(7);
                    //GetComponentInParent<Slot>().AcquireSkill(7);
                    break;

                case "Spider2":     //Ensnaring Web
                    slotManager.AcquireSkill(8);
                    //GetComponentInParent<Slot>().AcquireSkill(8);
                    break;

                case "Spider3":     //Spider Bite
                    slotManager.AcquireSkill(9);
                    //GetComponentInParent<Slot>().AcquireSkill(9);
                    break;
                    
                case "Knight":      //Spinning Sword
                    Knight knight = FindAnyObjectByType<Knight>();
                    if (knight.IsVulnerable()) {
                        slotManager.AcquireSkill(10);
                        //GetComponentInParent<Slot>().AcquireSkill(10);
                    }
                    break;

                case "Lich":        //Souls of the Damned
                    
                    Lich lich = FindAnyObjectByType<Lich>();
                    if (lich.IsVulnerable()) {
                        slotManager.AcquireSkill(11);
                        //GetComponentInParent<Slot>().AcquireSkill(11);
                    }
                    break;
            }
            //Instantiate(spawnSiphon, transform.position, transform.rotation);
            enemy.TakeDamage(2); //adjust later for stuff
            //GetComponentInParent<Slot>().AbsorbBulletAvailable = true;
            //Destroy(gameObject);
            BulletPool.Instance.ReturnBullet(gameObject);
        }
    }
}
