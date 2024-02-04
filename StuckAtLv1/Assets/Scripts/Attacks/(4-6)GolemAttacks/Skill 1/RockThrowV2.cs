using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockThrowV2 : MonoBehaviour
{
    float timer = 1f;   //if a modifier increase skill time duration, it would call back to the parent slot and acquire the modifier for calculation
    Rigidbody2D rb;
    RectTransform scale;
    Slot parent;
    private Vector3 mousePosition;
    private Camera mainCamera;
    public float speed;
    private int damage;
    [SerializeField] private GameObject aoeAttack;

    void Start() {  //aim towards the mouse
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody2D>();
        scale = GetComponent<RectTransform>();
        parent = GetComponentInParent<Slot>();
        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePosition - transform.position;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * speed;

        float scalingFactor = 1 + parent.GetCommonUpgrade(1)*0.2f + parent.GetRareUpgrade(1)*0.3f + parent.GetLegendaryUpgrade(1)*0.4f;
        transform.localScale = new Vector2(scalingFactor, scalingFactor);
        Debug.Log("size: " + scale.sizeDelta);

        //apply duration bonus
        timer *= 1f + (parent.GetCommonUpgrade(2)*0.2f + parent.GetRareUpgrade(2)*0.4f + parent.GetLegendaryUpgrade(2)*0.6f);
        Debug.Log("timer: " + timer);

        //apply damage bonus
        damage = (int)(10 * (1+(parent.GetCommonUpgrade(0)*0.2f + parent.GetRareUpgrade(0)*0.4f + parent.GetLegendaryUpgrade(0)*0.6f)));
        Debug.Log("damage: " + damage);
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D col) {
        Enemy enemy = col.GetComponent<Enemy>();
        if (enemy != null) {
            enemy.TakeDamage(damage);   //if a modifier increases damage, it would call back to the parent slot and acquire the modifier for calculation
            //rb.velocity = Vector2.zero;
            Instantiate(aoeAttack, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
