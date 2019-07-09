
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;


public class Shell : MonoBehaviour
{
    [HideInInspector] public int shooter;
    public Tile emptyTile;
    public Transform TopLeft;
    public Transform TopRight;
    public int damage;

    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.keepAnimatorControllerStateOnDisable = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //print("Shell hit " + other.name);
        bool explode = false;
        if (other.name == "Tilemap")
        {
            Tilemap map = other.GetComponent<Tilemap>();
            Grid grid = map.GetComponentInParent<Grid>();
            Vector3 cellSize = grid.cellSize;
            Vector3Int roundPosition = Vector3Int.FloorToInt(new Vector3(TopLeft.position.x / cellSize.x, TopLeft.position.y / cellSize.y, 0));
            TileBase tile = map.GetTile(roundPosition);

            if (tile.name == "brickwall")
            {
                map.SetTile(roundPosition, emptyTile);
                explode = true;
            }
            else if (tile.name == "steelwall")
            {
                if (damage == 2)
                    map.SetTile(roundPosition, emptyTile);
                explode = true;
            }
            roundPosition = Vector3Int.FloorToInt(new Vector3(TopRight.position.x / cellSize.x, TopRight.position.y / cellSize.y, 0));
            tile = map.GetTile(roundPosition);

            if (tile.name == "brickwall")
            {
                map.SetTile(roundPosition, emptyTile);
                explode = true;
            }
            else if (tile.name == "steelwall")
            {
                if (damage == 2)
                    map.SetTile(roundPosition, emptyTile);
                explode = true;
            }
        }
        else if (other.name == "EnemyTank" || other.name == "player1" || other.name == "player2")
        {
            Tank targetTank = other.GetComponent<Tank>();
            if (shooter * targetTank.m_PlayerNumber < 0)
                explode = true;
        }
        else if (other.name == "Shell")
        {
            Shell shell = other.GetComponent<Shell>();
            if (shell.shooter * shooter < 0)
                explode = true;
        }
        if (explode)
            StartCoroutine(Explode());

    }


    private IEnumerator Explode()
    {
        animator.SetTrigger("explode");
        GetComponent<Rigidbody2D>().velocity = new Vector3(0f, 0f, 0f);
        yield return new WaitForSeconds(0.35f);
        ObjectPool.GetInstance().RecycleObj(gameObject);
    }
}
