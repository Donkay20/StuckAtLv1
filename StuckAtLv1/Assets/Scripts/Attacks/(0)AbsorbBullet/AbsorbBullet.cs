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
        if (col.TryGetComponent<BreakableWall>(out var wall)) {
            wall.TakeDamage(damage);
        }

        if (col.TryGetComponent<Enemy>(out var enemy)) {
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
                
                case "Wolf1":       //Howl
                    slotManager.AcquireSkill(18);
                    break;
                
                case "Wolf2":       //Summon Wolf
                    slotManager.AcquireSkill(19);
                    break;
                
                case "Wolf3":       //Claw Frenzy
                    slotManager.AcquireSkill(20);
                    break;

                case "DeerNymph":   //Pheromone
                    slotManager.AcquireSkill(21);
                    break;

                case "VenusFlyTrap"://Fly Bomb
                    slotManager.AcquireSkill(22);
                    break;
                
                case "Slime1":      //Artillery
                    slotManager.AcquireSkill(23);
                    break;
                
                case "Slime2":      //Slime Puddle
                    slotManager.AcquireSkill(24);
                    break;
                
                case "Slime3":      //Sticky Slime
                    slotManager.AcquireSkill(25);
                    break;

                case "Ghost1":      //Shadow Ball
                    slotManager.AcquireSkill(26);
                    break;
                
                case "Ghost2":      //Vortex
                    slotManager.AcquireSkill(27);
                    break;
                
                case "Ghost3":      //Spirits
                    slotManager.AcquireSkill(28);
                    break;
                
                case "Imp1":
                    slotManager.AcquireSkill(29);
                    break;
                
                case "Imp2":
                    slotManager.AcquireSkill(30);
                    break;
                
                case "Imp3":
                    slotManager.AcquireSkill(31);
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