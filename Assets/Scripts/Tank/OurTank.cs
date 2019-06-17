using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class OurTank : Tank
{
    public GameObject shield;

    private string m_VerticalAxisName;
    private string m_HorizontalAxisName;
    private string m_FireButton;                // The input axis that is used for launching shells.
    private Animator shieldAnimator;


    public IEnumerator Born()
    {
        if (gameObject.name == "player1")
            m_PlayerNumber = 1;
        else
            m_PlayerNumber = 2;
        m_VerticalAxisName = "Vertical" + m_PlayerNumber;
        m_HorizontalAxisName = "Horizontal" + m_PlayerNumber;
        m_FireButton = "Fire" + m_PlayerNumber;
        Vector2[] ourSpawnPoint = { new Vector2(-0.75f, -3f), new Vector2(0.75f, -3f) };
        transform.position = ourSpawnPoint[m_PlayerNumber - 1];
        m_Dead = false;
        Health = 1;
        isInvincible = true;
        invincibleTime = 2f;
        gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        isInvincible = false;
        shieldAnimator.SetTrigger("reset");
    }


    // Start is called before the first frame update
    void Start()
    {
        moveDirection = Vector2.up;
        score = 0;

        m_ChargeTime = 1.0f;
        shieldAnimator = shield.GetComponent<Animator>();
    }


    // Update is called once per frame
    void Update()
    {
        if (m_Dead)
            return;
        float distance;
        RaycastHit2D hit1 = Physics2D.Raycast(fireTransform.position, -moveDirection, 0.1f, LayerMask.GetMask("Tank"));
        if (hit1.collider.gameObject != gameObject)
        {
            distance = 0;
        }
        else
        {
            RaycastHit2D hit = Physics2D.Raycast(fireTransform.position, moveDirection, 7f, LayerMask.GetMask("Tank"));
            distance = hit.distance;
        }
        m_ChargeTime = Mathf.Lerp(0.5f, 1.3f, distance / 7);

        if (Input.GetButtonDown(m_FireButton))
        {
            if (m_CurrentChargeTime <= 0)
            {
                Fire();
                m_CurrentChargeTime = m_ChargeTime;
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

            position += moveDirection * speed * Time.deltaTime;
            rigidbody2d.MovePosition(position);
        }
    }
}


