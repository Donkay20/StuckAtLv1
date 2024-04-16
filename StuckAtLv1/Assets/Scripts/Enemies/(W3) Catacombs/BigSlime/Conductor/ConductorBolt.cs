using System.Linq;
using UnityEngine;

public class ConductorBolt : MonoBehaviour
{
    private readonly int BOLT_DAMAGE = 200;
    private bool hitCheck;
    private float hitRefreshRate = 0.25f;
    private BoxCollider2D boltCollider;

    void Start() {
        boltCollider = GetComponent<BoxCollider2D>();
    }

    void Update() {
        if (hitRefreshRate > 0 && !hitCheck) {
            hitRefreshRate -= Time.deltaTime;
        }

        if (hitRefreshRate <= 0 && !hitCheck) {
            hitCheck = true;
        }
    }

    /*
    private void OnEnable() {
        anim.SetTrigger("activate");
    }
    */

    private void ResetHitCheck() {
        hitCheck = false;
        hitRefreshRate = 0.25f;
    }

    private void OnTriggerStay2D(Collider2D col) {
        if (hitCheck) {
            Vector2 boxSize = boltCollider.size;
            Vector2 boxCenter = transform.position;

            Collider2D[] enemyColliders = Physics2D.OverlapBoxAll(boxCenter, boxSize, 0, LayerMask.GetMask("Enemy"));
            Collider2D[] passThroughEnemyColliders = Physics2D.OverlapBoxAll(boxCenter, boxSize, 0, LayerMask.GetMask("PassThroughEnemy"));
            
            foreach (Collider2D c in enemyColliders.Concat(passThroughEnemyColliders)) {
                if (c.gameObject.CompareTag("BigSlime")) {
                    Enemy boss = c.gameObject.GetComponent<Enemy>(); 
                    BigSlime slime = c.gameObject.GetComponent<BigSlime>();

                    slime.Electrified(); 
                    boss.TakeDamage(BOLT_DAMAGE); 
                    boss.ApplyStun(0.3f);
                } else {
                    Enemy shitter = c.gameObject.GetComponent<Enemy>();
                    shitter.TakeDamage(BOLT_DAMAGE);
                }
            }
            ResetHitCheck();
        }
    }
}