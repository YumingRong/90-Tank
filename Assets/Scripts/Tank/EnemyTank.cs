using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyTank : Tank
{
    private float directionChangeInteval;
    private float directionChangeTimer;

    private int[] healthArray = { 1, 2, 2, 3 };
    private float[] speedArray = { 0.5f, 0.5f, 1f, 0.5f };
    private int[] scoreArray = { 100, 200, 300, 400 };

    public int Type
    {
        set
        {
            Health = healthArray[value];
            speed = speedArray[value];
            score = scoreArray[value];
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
        m_PlayerNumber = -1;
        moveDirection = Vector2.down;
        m_ChargeTime = 1.8f;
        directionChangeTimer = 0;
        directionChangeInteval = Random.Range(2, 5);
    }

    public IEnumerator Born(int type, int position)
    {
        Type = type;
        animator.SetInteger("type", type + 1);

        Vector2[] enemySpawnPoint = { new Vector2(-3f, 3f), new Vector2(0f, 3f), new Vector2(3f, 3f) };
        transform.position = enemySpawnPoint[position];
        //speed = 0;
        isInvincible = true;
        invincibleTime = 1f;
        yield return new WaitForSeconds(1f);
        isInvincible = false;
        //speed = speedArray[type];
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
        //print("Delta position " + speed * Time.deltaTime);
        directionChangeTimer += Time.deltaTime;
        float gridsize = smallestGrid * 2;
        if (directionChangeTimer > directionChangeInteval && (transform.position.x % gridsize)< smallestGrid /4 && (transform.position.y % gridsize) < smallestGrid/4)
        {
            SelectDirection(false);
            directionChangeInteval = Random.Range(1, 3);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name == "Tilemap")
        {
            Tilemap map = collision.collider.GetComponent<Tilemap>();
            Vector3 position = moveDirection * smallestGrid;
            position += collision.otherCollider.transform.position;
            position /= smallestGrid;
            TileBase tile = map.GetTile(Vector3Int.FloorToInt(position));
            if (tile.name == "steelwall" || tile.name == "river")
            {
                SelectDirection(true);
            }
        }
    }


    private void SelectDirection(bool mustChange)
    {
        float[] directChance = { 0.1f, 0.45f, 0.2f, 0.2f };
        if (transform.position.y <= -3f)
            directChance[1] = 0;
        else if (transform.position.y >= 3f)
            directChance[0] = 0;
        if (transform.position.x < 0)
        {
            directChance[2] = 0.15f;
            directChance[3] = 0.3f;
            if (transform.position.x <= -3f)
                directChance[2] = 0f;
        }
        else if (transform.position.x > 0)
        {
            directChance[2] = 0.3f;
            directChance[3] = 0.15f;
            if (transform.position.x >= -3f)
                directChance[3] = 0f;
        }

        if (mustChange)
        {
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
        moveDirection = directChoice[Choose(directChance)];

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
        rigidbody2d.position = position;

        directionChangeTimer = 0;

    }

    private int Choose(float[] probs)
    {

        float total = 0;

        foreach (float elem in probs)
        {
            total += elem;
        }

        float randomPoint = Random.value * total;

        for (int i = 0; i < probs.Length; i++)
        {
            if (randomPoint < probs[i])
            {
                return i;
            }
            else
            {
                randomPoint -= probs[i];
            }
        }
        return probs.Length - 1;
    }

}
