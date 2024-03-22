using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class AbsorbBullet : MonoBehaviour
{
    private readonly float BASE_BULLET_LIFETIME = 1f;
    private readonly int BASE_BULLET_DAMAGE = 2;
    [SerializeField] float timer = 1f; //if a modifier increase skill time duration, it would call back to the parent slot and acquire the modifier for calculation
    private int damage;
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
        damage = BASE_BULLET_DAMAGE + slotManager.GetPermanentAtkDmg() + slotManager.GetTempAtkDmg();
    }

    private void OnEnable() {
        slotManager = GetComponentInParent<SlotManager>();
        rb = GetComponent<Rigidbody2D>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePosition - transform.position;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * speed;
        damage = BASE_BULLET_DAMAGE + slotManager.GetPermanentAtkDmg() + slotManager.GetTempAtkDmg();
    }

    void Update() { //deactivate the bullet
        timer -= Time.deltaTime;
        if (timer <= 0) {
            timer = BASE_BULLET_LIFETIME;
            BulletPool.Instance.ReturnBullet(gameObject);
        }
    }

    public void UpdateTimer() {
        timer = BASE_BULLET_LIFETIME;
    }

    private void OnTriggerEnter2D(Collider2D col) {     //upon hitting an enemy:                  
        Enemy enemy = col.GetComponent<Enemy>();        //the enemy class will need to be changed to a bare-bones calculation class as every enemy will need it, can stack other classes on different enemies for unique behavior             
        if (enemy != null) {
            switch (enemy.tag) {    //Each enemy will have a unique tag which will identify which one the bullet is hitting.                              
                case "Skeleton1":   //Bone Toss
                    slotManager.AcquireSkill(1);                                      
                    break;

                case "Skeleton2":   //Bone Spikes
                    slotManager.AcquireSkill(2);
                    break;

                case "Skeleton3":   //Upheaval
                    slotManager.AcquireSkill(3);
                    break;

                case "Golem1":      //Rock Throw
                    slotManager.AcquireSkill(4);
                    break;

                case "Golem2":      //Ground Slam
                    slotManager.AcquireSkill(5);
                    break;

                case "Golem3":      //Fissure
                    slotManager.AcquireSkill(6);
                    break;

                case "Spider1":     //Venom Spit
                    slotManager.AcquireSkill(7);
                    break;

                case "Spider2":     //Ensnaring Web
                    slotManager.AcquireSkill(8);
                    break;

                case "Spider3":     //Spider Bite
                    slotManager.AcquireSkill(9);
                    break;
                    
                case "Knight":      //Spinning Sword
                    Knight knight = FindAnyObjectByType<Knight>();
                    if (knight.IsVulnerable()) {slotManager.AcquireSkill(10);}
                    break;

                case "Lich":        //Souls of the Damned
                    Lich lich = FindAnyObjectByType<Lich>();
                    if (lich.IsVulnerable()) {slotManager.AcquireSkill(11);}
                    break;
                
                case "Squirrel1":   //Squirrel Bite
                    slotManager.AcquireSkill(12);
                    break;

                case "Squirrel2":   //Squirrel Tail Swipe
                    slotManager.AcquireSkill(13);
                    break;

                case "Squirrel3":   //Squirrel Ball
                    slotManager.AcquireSkill(14);
                    break;

                case "Tree1":       //Apple Barrage
                    slotManager.AcquireSkill(15);
                    break;

                case "Tree2":       //Tree Branch
                    slotManager.AcquireSkill(16);
                    break;

                case "Tree3":       //Vine Snare
                    slotManager.AcquireSkill(17);
                    break;
                
                case "Wolf1":
                    slotManager.AcquireSkill(18);
                    break;
                
                case "Wolf2":
                    slotManager.AcquireSkill(19);
                    break;
                
                case "Wolf3":
                    slotManager.AcquireSkill(20);
                    break;
            }

            if (slotManager.IsBloodsuckerActive()) {    //legendary 9
                enemy.ApplyAnemia(1,3);
            }

            if (slotManager.IsAvariceActive()) {        //legendary 4
                if (Random.Range(1,101) <= 50) {
                    enemy.CriticalHit();
                    damage *= 2;
                }
                FindAnyObjectByType<Character>().GainMoney(1);
            }
            
            enemy.TakeDamage(damage); 

            if (!slotManager.IsPenetrationActive()) {   //legendary 1
                BulletPool.Instance.ReturnBullet(gameObject);
            }
        }
    }
}
