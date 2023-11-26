using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoeCircle : MonoBehaviour
{
    private Animator anim;
    [SerializeField] int damage;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("AOE Instantiated");
        anim = GetComponent<Animator>();
        anim.SetTrigger("Start");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col) {
        Enemy enemy = col.GetComponent<Enemy>();
        if (enemy != null) {
            enemy.TakeDamage(damage);   //if a modifier increases damage, it would call back to the parent slot and acquire the modifier for calculation
        }
    }

    private void Despawn()
    {
        //Debug.Log("Destory AOE");
        Destroy(gameObject);
    }
}
