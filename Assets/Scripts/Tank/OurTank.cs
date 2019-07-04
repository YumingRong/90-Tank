using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class OurTank : Tank
{
    public GameObject shield;
    private float invincibleTime; 
    private string m_VerticalAxisName;
    private string m_HorizontalAxisName;
    private string m_FireButton;                // The input axis that is used for launching shells.
    private Animator shieldAnimator;
    private bool isFixed;   //when tank collides other tank, it cannot move
    private Vector3 position0;      //when tank collides other tank, it restores to the original position
    public int m_level
    {
        set
        {
            float[] chargeTimes = {10f, 1.4f, 1.2f, 1f, 0.8f};
            int[] healths = { 0, 1, 1, 1, 2 };
            m_ChargeTime = chargeTimes[value];
            level = value;
            Health = healths[value];
            animator.SetInteger("level", value);
            animator.SetInteger("health", Health);
        }
        get
        {
            return level;
        }
    }
    private int level;

    public void Born()
    {
        if (gameObject.name == "player1")
            m_PlayerNumber = 1;
        else
            m_PlayerNumber = 2;
        m_VerticalAxisName = "Vertical" + m_PlayerNumber;
        m_HorizontalAxisName = "Horizontal" + m_PlayerNumber;
        m_FireButton = "Fire" + m_PlayerNumber;
        Vector2[] ourSpawnPoint = { new Vector2(-1f, -3f), new Vector2(1f, -3f) };
        transform.position = ourSpawnPoint[m_PlayerNumber - 1];
        m_Dead = false;
        level = 1;
        isFixed = false;
        gameObject.SetActive(true);
        invincibleTime = 3f;
        shieldAnimator.SetBool("invincible", true); 
    }

    // Start is called before the first frame update
    void Start()
    {
        moveDirection = Vector2.up;
        shieldAnimator = shield.GetComponent<Animator>();
    }


    // Update is called once per frame
    void Update()
    {
        if (m_Dead)
            return;
        float distance;

        if (invincibleTime > 0)
        {
            invincibleTime -= Time.deltaTime;
            if (invincibleTime<=0)
            {
                shieldAnimator.SetBool("invincible", false);
                isInvincible = false;
            }
        }

        if (Input.GetButtonDown(m_FireButton))
        {
            if (m_CurrentChargeTime <= 0)
            {
                Fire();
                RaycastHit2D hit = Physics2D.Raycast(fireTransform.position, moveDirection, 7f, LayerMask.GetMask("Tank"));
                distance = hit.distance;
                m_CurrentChargeTime = Mathf.Lerp(0.4f, m_ChargeTime, distance / 7);
            }
        }
        m_CurrentChargeTime -= Time.deltaTime;

        // Store the player's input and make sure the audio for the engine is playing.
        float vertical = Input.GetAxisRaw(m_VerticalAxisName);
        float horizontal = Input.GetAxis(m_HorizontalAxisName);
        //print("vertical " + vertical + "； horizontal " + horizontal);
        // Adjust the position of the tank based on the player's input.
        Vector2 position = rigidbody2d.position;

        if (Mathf.Approximately(horizontal, 0.0f) && Mathf.Approximately(vertical, 0.0f))
        {
            animator.speed = 0;
        }
        else
        {
            if (Mathf.Abs(horizontal) > Mathf.Abs(vertical))
            {
                moveDirection.Set(horizontal, 0);
                if (horizontal < 0)
                {
                    rigidbody2d.rotation = 90f;
                    moveDirection = Vector2.left;
                }
                else
                {
                    rigidbody2d.rotation = -90f;
                    moveDirection = Vector2.right;
                }
                position.y = Mathf.RoundToInt(position.y / smallestGrid) * smallestGrid;

            }
            else
            {
                moveDirection.Set(0, vertical);
                if (vertical > 0)
                {
                    rigidbody2d.rotation = 0f;
                    moveDirection = Vector2.up;
                }

                else
                {
                    rigidbody2d.rotation = 180f;
                    moveDirection = Vector2.down;
                }
                position.x = Mathf.RoundToInt(position.x / smallestGrid) * smallestGrid;
            }
            animator.speed = (moveDirection * speed).magnitude;

            if (!isFixed)
            {
                position0 = position;
                position += moveDirection * speed * Time.deltaTime;
                rigidbody2d.MovePosition(position);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name == "EnemyTank")
        {
            print("Enter collision");
            print("position1 " + transform.position);
            print("position0 " + position0);
            rigidbody2d.position = position0;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.name == "EnemyTank")
        {
            print("Exit collision");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Shell" && !isInvincible)
        {
            Shell shell = collision.GetComponent<Shell>();
            if (shell.shooter < 0)
            {
                if (level == 4)
                    level = 1;
                else
                    Health--;
                // If the current health is at or below zero and it has not yet been registered, call OnDeath.
                if (Health <= 0 && !m_Dead)
                {
                    StartCoroutine(OnDeath());
                }

            }
        }
    }

    private IEnumerator OnDeath()
    {
        // Set the flag so that this function is only called once.
        m_Dead = true;
        yield return new WaitForSeconds(7f / 10f);

        gameObject.SetActive(false);
        BattleManager.GetInstance().OurTankDie(m_PlayerNumber);
    }

    public void OnInvinciblePrize()
    {
        invincibleTime = 5f;
        shieldAnimator.SetBool("invincible", true);
    }

}


