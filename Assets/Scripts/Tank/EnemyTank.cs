using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Assets.Scripts;

public class EnemyTank : Tank
{
    public Transform frontLeft, frontRight;
    private Tilemap map;
    [HideInInspector] public static float bulletTime;
    private float directionChangeInteval;
    private float directionChangeTimer;
    private bool hasPrize;

    private readonly int[] healthArray = { 1, 2, 2, 3 };
    private readonly float[] speedArray = { 0.5f, 0.5f, 1f, 0.5f };

    public int Type
    {
        set
        {
            Health = healthArray[value];
            speed = speedArray[value];
            type = value;
        }
        get
        {
            return type;
        }
    }
    private int type;

    // Start is called before the first frame update
    void Start()
    {
        moveDirection = Vector2.down;
        m_ChargeTime = 1.7f;
        bulletTime = 0;
        directionChangeTimer = 0;
        directionChangeInteval = 2;
        map = GameObject.Find("Tilemap").GetComponent<Tilemap>();
    }

    public IEnumerator Born(int type, int number, bool prize)
    {
        Vector2[] enemySpawnPoint = { new Vector2(-3f, 3f), new Vector2(0f, 3f), new Vector2(3f, 3f) };
        Vector2 spawnPoint = enemySpawnPoint[number%3];
        gameObject.SetActive(false);

        bool collide;
        while(true)
        {
            collide = false;
            Tank[] tanks = FindObjectsOfType<Tank>();
            foreach (Tank tank in tanks)
            {
                if (Vector2.Distance(spawnPoint, tank.transform.position) < 0.5f)
                    collide = true;
            }
            if (collide)
                yield return new WaitForSeconds(1f);
            else
                break;
        }

        gameObject.SetActive(true);
        Type = type;
        m_PlayerNumber = -number;
        hasPrize = prize;
        animator.SetInteger("type", type + 1);
        animator.SetBool("prize", prize);
        transform.position = spawnPoint;
        speed = 0;
        invincibleTime = 1f;
        yield return new WaitForSeconds(1f);
        speed = speedArray[type];
    }


    // Update is called once per frame
    void Update()
    {
        if (m_Dead)
            return;
        invincibleTime -= Time.deltaTime;
        if (BattleManager.GetInstance().bulletTime > 0)
            return;
        m_CurrentChargeTime += Time.deltaTime;
        if (m_CurrentChargeTime >= m_ChargeTime)
        {
            Fire(1);
            m_CurrentChargeTime = 0f;
        }
    }

    private void FixedUpdate()
    {
        if (m_Dead)
            return;
        rigidbody2d.position += moveDirection * speed * Time.fixedDeltaTime;
        //print("Delta position " + speed * Time.fixedDeltaTime);
        directionChangeTimer += Time.deltaTime;
        if (directionChangeTimer > directionChangeInteval)
        {
            if (AtGrid(transform.position, smallestGrid))
            {
                SelectDirection(false);
                directionChangeInteval = Random.Range(0.5f, 2f);
            }
        }

    }

    private bool AtGrid(Vector2 v, float grid)
    {
        return (v.x % grid < grid * 0.15 || v.x % grid > grid * 0.85) && (v.y % grid < grid * 0.15 || v.y % grid > grid * 0.85);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name == "Tilemap")
        {
            TileBase tileLeft = map.GetTile(Vector3Int.FloorToInt(frontLeft.position / smallestGrid));
            TileBase tileRight = map.GetTile(Vector3Int.FloorToInt(frontRight.position / smallestGrid));
            if (tileLeft.name == "steelwall" || tileLeft.name == "river" || tileRight.name == "steelwall" || tileRight.name == "river")
            {
                SelectDirection(true);
            }
            //when half brick,it'd be better to change direction
            else if (tileLeft.name == "brickwall" && tileRight.name == "empty" || tileLeft.name == "empty" && tileRight.name == "brickwall")
            {
                print("collide half brick");
                SelectDirection(false);
            }
        }

        if (collision.collider.name == "EnemyTank")
        {
            Vector2 line = collision.collider.transform.position - transform.position;
            line.Normalize();
            float dotproduct = Vector2.Dot(moveDirection, line);
            print("Tank " + m_PlayerNumber + " collides. Dot product " + dotproduct + " Move direction "+ moveDirection);
            if (dotproduct > speed * 0.5)     //the knocker should change its direction
            {
                SelectDirection(true);
                print("change direction");
            }
            else  //the knocked should keep its velocity
            {
                print("keep direction");
                rigidbody2d.velocity = moveDirection * speed;
            }
        }
    }


    private void SelectDirection(bool mustChange)
    {
        float[] directChance = { 0.1f, 0.45f, 0.2f, 0.2f };

        if (System.Math.Abs(transform.position.x) < Mathf.Epsilon)
        {
            directChance[0] = 0.1f;
            directChance[1] = 0.45f;
            directChance[2] = 0.2f;
            directChance[3] = 0.2f;
        }
        else if (transform.position.x < 0)
        {
            directChance[2] = 0.15f;
            directChance[3] = 0.25f;
            if (transform.position.x < -2.75f)
                directChance[2] = 0f;
        }
        else if (transform.position.x > 0)
        {
            directChance[2] = 0.25f;
            directChance[3] = 0.15f;
            if (transform.position.x > -2.75f)
                directChance[3] = 0f;
        }
        if (transform.position.y < -2.75f)
            directChance[1] = 0;
        else if (transform.position.y > 2.75f)
            directChance[0] = 0;

        if (mustChange)
        {
            print("must change direction");
            if (moveDirection == Vector2.up)
                directChance[0] = 0f;
            else if (moveDirection == Vector2.down)
                directChance[1] = 0f;
            else if (moveDirection == Vector2.left)
                directChance[2] = 0f;
            else
                directChance[3] = 0f;
        }

        Vector2[] directChoice = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
        moveDirection = directChoice[RandomElement.Choose(directChance)];

        if (moveDirection == Vector2.up)
            rigidbody2d.rotation = 0f;
        else if (moveDirection == Vector2.down)
            rigidbody2d.rotation = 180f;
        else if (moveDirection == Vector2.left)
            rigidbody2d.rotation = 90f;
        else if (moveDirection == Vector2.right)
            rigidbody2d.rotation = -90f;

        Vector2 position = rigidbody2d.position;
        position.x = Mathf.RoundToInt(position.x / smallestGrid) * smallestGrid;
        position.y = Mathf.RoundToInt(position.y / smallestGrid) * smallestGrid;
        rigidbody2d.MovePosition(position);

        directionChangeTimer = 0;

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Shell" && invincibleTime <= 0)
        {
            Shell shell = collision.GetComponent<Shell>();
            if (shell.shooter > 0)
            {
                if (hasPrize)
                {
                    hasPrize = false;
                    GameObject prizeInstance = ObjectPool.GetInstance().GetObject("Prize");
                    Prize prize = prizeInstance.GetComponent<Prize>();
                }
                Health -= shell.damage;
                // If the current health is at or below zero and it has not yet been registered, call OnDeath.
                if (Health <= 0 && !m_Dead)
                {
                    StartCoroutine(OnDeath(shell.shooter));
                }
            }
        }
        else
        {
            Vector2 position = rigidbody2d.position;
            position.x = Mathf.RoundToInt(position.x / smallestGrid) * smallestGrid;
            position.y = Mathf.RoundToInt(position.y / smallestGrid) * smallestGrid;
            rigidbody2d.MovePosition(position);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        rigidbody2d.velocity = new Vector2(0, 0);
    }

    public void Die(int player)       //coroutine cannot be invoked by other class
    {
        StartCoroutine(OnDeath(player));
    }

    private IEnumerator OnDeath(int shooter)
    {
        // Set the flag so that this function is only called once.
        m_Dead = true;
        yield return new WaitForSeconds(0.7f);
        gameManager.kill[shooter - 1, type]++;
        BattleManager.GetInstance().liveEnemy--;
        ObjectPool.GetInstance().RecycleObj(gameObject);
        BattleManager.GetInstance().SpawnEnemyTank();
    }


}
