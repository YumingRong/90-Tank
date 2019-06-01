﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTank : Tank
{
    private float directionChangeInteval;
    private float directionChangeTimer;
    System.Random rnd = new System.Random();

    private int[] healthArray = { 1, 2, 2, 3 };
    private float[] speedArray = { 0.5f, 0.5f, 1f, 0.5f };
    private int[] scoreArray = { 100, 200, 300, 400 };

    public int Type
    {
        set
        {
            health = healthArray[value];
            speed = speedArray[value];
            score = scoreArray[value];
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        m_PlayerNumber = -1;
        moveDirection = Vector2.down;
        m_ChargeTime = 2f;
        directionChangeTimer = 0;
        directionChangeInteval = rnd.Next(3,5);
    }

    private IEnumerator Born(int type)
    {
        Type = type;
        animator.SetInteger("type", type);
        animator.SetInteger("health", health);
        float speed0 = speed;
        speed = 0;
        isInvincible = true;
        yield return new WaitForSeconds(1f);
        isInvincible = false;
        speed = speed0;
    }


    // Update is called once per frame
    void Update()
    {
        m_CurrentChargeTime += Time.deltaTime;
        if (m_CurrentChargeTime >= m_ChargeTime)
        {
            Fire();
            m_CurrentChargeTime = 0f;
        }
        rigidbody2d.position += moveDirection * speed * Time.deltaTime;
        directionChangeTimer += Time.deltaTime;
        if (directionChangeTimer > directionChangeInteval)
        {
            SelectDirection();
            directionChangeInteval = rnd.Next(5);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("tank hit wall");
        SelectDirection();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "WorldLimiter")
        {
            Debug.Log("tank hit ex bound");
            SelectDirection();
        }
    }

    private void SelectDirection()
    {
        Vector2 direction = moveDirection;
        if (transform.position.x < 0)
        {
            Vector2[] directChoice = { Vector2.up, Vector2.right, Vector2.right, Vector2.down, Vector2.down, Vector2.down, Vector2.left };
            direction = directChoice[rnd.Next(6)];
        }
        else
        {
            Vector2[] directChoice = { Vector2.up, Vector2.left, Vector2.left, Vector2.down, Vector2.down, Vector2.down, Vector2.right };
            direction = directChoice[rnd.Next(6)];
        }
        if (direction == Vector2.up)
            rigidbody2d.rotation = 0f;
        else if (direction == Vector2.down)
            rigidbody2d.rotation = 180f;
        else if (direction == Vector2.left)
            rigidbody2d.rotation = 90f;
        else if (direction == Vector2.right)
            rigidbody2d.rotation = -90f;
        moveDirection = direction;

        Vector2 position = rigidbody2d.position;
        position.x = Mathf.RoundToInt(position.x / smallestGrid) * smallestGrid;
        position.y = Mathf.RoundToInt(position.y / smallestGrid) * smallestGrid;
        rigidbody2d.position = position;

        directionChangeTimer = 0;

    }
}
