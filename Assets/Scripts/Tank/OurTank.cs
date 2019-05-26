using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class  OurTank: Tank
{
    private float smallestGrid = 0.0625f;
    private string m_VerticalAxisName;
    private string m_HorizontalAxisName;

    // ==== ANIMATION =====
    Animator animator;
    private string m_FireButton;                // The input axis that is used for launching shells.


    // Start is called before the first frame update
    void Start()
    {
        moveDirection = Vector2.up;
        m_ChargeTime = 1.0f;
        m_VerticalAxisName = "Vertical" + m_PlayerNumber;
        m_HorizontalAxisName = "Horizontal" + m_PlayerNumber;

        animator = GetComponent<Animator>();
        animator.speed = 0;

        // The fire axis is based on the player number.
        m_FireButton = "Fire" + m_PlayerNumber;

        rigidbody2d = GetComponent<Rigidbody2D>();
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown(m_FireButton))
        {
            if (m_CurrentChargeTime <= 0)
            {
                Fire();
                m_CurrentChargeTime = m_ChargeTime;
            }
        }
        m_CurrentChargeTime -= Time.deltaTime;
    }


    void FixedUpdate()
    {
        // Store the player's input and make sure the audio for the engine is playing.
        float vertical = Input.GetAxis(m_VerticalAxisName);
        float horizontal = Input.GetAxis(m_HorizontalAxisName);

        // Adjust the position of the tank based on the player's input.
        Vector2 move = new Vector2(horizontal, vertical);
        Vector2 position = rigidbody2d.position;

        if (Mathf.Approximately(move.x, 0.0f) && Mathf.Approximately(move.y, 0.0f))
        {
            animator.speed = 0;
            position.x = Mathf.RoundToInt(position.x / smallestGrid) * smallestGrid;
            position.y = Mathf.RoundToInt(position.y / smallestGrid) * smallestGrid;
            rigidbody2d.position = position;
        }
        else
        {
            if (Mathf.Abs(move.x) > Mathf.Abs(move.y))
            {
                moveDirection.Set(move.x, 0);
                if (move.x < 0)
                    rigidbody2d.rotation = 90f;
                else
                    rigidbody2d.rotation = -90f;
            }
            else
            {
                moveDirection.Set(0, move.y);
                if (move.y > 0)
                    rigidbody2d.rotation = 0;
                else
                    rigidbody2d.rotation = 180f;
            }
            moveDirection.Normalize();

            animator.speed = (move * speed).magnitude;

            position += move * speed * Time.deltaTime;
            rigidbody2d.MovePosition(position);
        }
    }

}
