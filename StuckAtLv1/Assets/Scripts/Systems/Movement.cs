using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]

public class Movement : MonoBehaviour
{
    Rigidbody2D body;
    Vector3 movementVector;
    Animator anim;
    SpriteRenderer sr;

    private readonly float BASE_SPEED = 5f;
    private float speedModifier; public float SpeedModifier { get => speedModifier; set => speedModifier = value; }
    private float speedDebuff; public float SpeedDebuff { get => speedDebuff; set => speedDebuff = value; }
    
    //Dashing Mechanic
    [SerializeField] float dashForce = 100f;
    [SerializeField] float dashTimerEnd = 0.04f;
    [SerializeField] float dashTimer = 0f;
    private bool isDashing = false;
    [SerializeField] GameObject dashSprite;
    [SerializeField] BoxCollider2D hitbox;
    private Character invincibility;
    [SerializeField] private Image dashCooldownFill;
    [SerializeField] private TextMeshProUGUI dashCooldownText;
    private readonly float BASE_COOLDOWN = 3f;
    private float dashCooldownModifier, externalModifier;
    //todo, add conditions for cooldownmodifier
    private float activeDashCD; public float ActiveDashCD { get => activeDashCD; set => activeDashCD = value; }

    private bool coolingDown;
    
    private void Awake() {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        invincibility = GetComponent<Character>();
        movementVector = new Vector3();
        coolingDown = false;
        dashCooldownFill.fillAmount = 0f;
        speedDebuff = 1;
    }
    private void OnEnable() {
        speedModifier = 0;
        speedDebuff = 1;
    }

    void Update()
    {
        movementVector.x = Input.GetAxisRaw("Horizontal");
        movementVector.y = Input.GetAxisRaw("Vertical");

        if (activeDashCD > 0) {
            coolingDown = true; dashCooldownText.gameObject.SetActive(true);
            activeDashCD -= Time.deltaTime;
            if (activeDashCD > 1) {
                dashCooldownText.text = activeDashCD.ToString("f0");
            } else {
                dashCooldownText.text = activeDashCD.ToString("f1");
            }
            dashCooldownFill.fillAmount = activeDashCD/(BASE_COOLDOWN + dashCooldownModifier + externalModifier);
        } else {
            coolingDown = false; dashCooldownText.gameObject.SetActive(false);
        }

        if (!isDashing) {
            movementVector = movementVector.normalized * BASE_SPEED * (1 + speedModifier) * speedDebuff;
            body.velocity = movementVector;
        }
        
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.LeftShift) && movementVector != Vector3.zero) {
            if (!coolingDown) {
                hitbox.enabled = false;
                isDashing = true;
                Dash(); SetDashCooldown();
            }
        }

        if (isDashing) {
            dashTimer += Time.deltaTime;
            //Debug.Log(dashTimer);

            if(dashTimer >= dashTimerEnd)
            {
                Instantiate(dashSprite, transform.position, transform.rotation);
                dashTimer = 0f;
            }
        }
        RunAnimation();
    }

    void Dash() {
        //Debug.Log("Dashing");
        body.AddForce(movementVector * dashForce, ForceMode2D.Impulse);
        Invoke(nameof(StopDashing), 0.3f);
        invincibility.DashingIFrames();
    }

    void StopDashing() {
        isDashing = false;
        //Debug.Log("Stop Dashing");
        dashTimer = 0f;
        invincibility.StopDashingIFrames();
        hitbox.enabled = true;
    }

    void RunAnimation() {
        if(movementVector.x != 0 || movementVector.y != 0)
        {
            anim.SetBool("Run", true);
        }
        else
        {
            anim.SetBool("Run", false);
        }

        if(movementVector.x > 0)
        {
            sr.flipX = false;
        }
        else if(movementVector.x < 0)
        {
            sr.flipX = true;
        }
    }

    public void SetDashCooldown() {
        activeDashCD = BASE_COOLDOWN + dashCooldownModifier + externalModifier;
    }

    public void SetExternalModifier(float e) {
        externalModifier = e;
    }

    public float GetSpeed() {
        return BASE_SPEED * (1 + speedModifier) * speedDebuff;
    }

    public void BattleEnd() {
        activeDashCD = 0;
        dashCooldownFill.fillAmount = 0;
        dashCooldownText.gameObject.SetActive(false);
        externalModifier = 0;
        coolingDown = false;
    }
}