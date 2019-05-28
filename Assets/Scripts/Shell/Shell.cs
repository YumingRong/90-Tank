
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;


public class Shell: MonoBehaviour
{

    [HideInInspector] public int shooter;
    [HideInInspector] public int damage;
    public Tile emptyTile;
    public Transform TopLeft;
    public Transform TopRight;

    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.keepAnimatorControllerStateOnDisable = true;
    }

    void Update()
    {
        //destroy the projectile when it reach a distance of 5.0f from the origin
        if (transform.position.magnitude > 5.0f)
            Destroy(gameObject);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Tilemap")
        {
            Tilemap map = other.GetComponent<Tilemap>();
            if (map != null)
            {
                Grid grid = map.GetComponentInParent<Grid>();
                Vector3 cellSize = grid.cellSize;
                Vector3Int roundPosition = Vector3Int.FloorToInt(new Vector3 (TopLeft.position.x /cellSize.x, TopLeft.position.y/cellSize.y, 0));
                TileBase tile = map.GetTile(roundPosition);
                print(roundPosition);

                if (tile.name == "brickwall")
                {
                    map.SetTile(roundPosition, emptyTile);
                }

                roundPosition = Vector3Int.FloorToInt(new Vector3(TopRight.position.x / cellSize.x, TopRight.position.y / cellSize.y, 0));
                tile = map.GetTile(roundPosition);
                print(roundPosition);

                if (tile.name == "brickwall")
                {
                    map.SetTile(roundPosition, emptyTile);
                }
            }
        }   
        else
        {
            // ... and find their rigidbody.
            //Rigidbody2D targetRigidbody = other.GetComponent<Rigidbody2D>();
            Rigidbody2D targetRigidbody = other.attachedRigidbody;
            // Find the TankHealth script associated with the rigidbody.
            Tank targetTank = targetRigidbody.GetComponent<Tank>();
            // Deal this damage to the tank.
            if (targetTank != null)
            {
                print("tank:" + targetTank.m_PlayerNumber);
                if (targetTank.m_PlayerNumber != shooter)
                {
                    targetTank.TakeDamage(damage);
                }
                else
                {
                    return;
                }

            }
        }


        Debug.Log("Shell explode:" + animator.GetCurrentAnimatorStateInfo(0).IsName("Base.Explode"));
        // Explode the shell.
        StartCoroutine(Explode());
    }


    private IEnumerator Explode()
    {
        animator.SetBool("exploding", true);
        GetComponent<Rigidbody2D>().velocity = new Vector3(0f, 0f, 0f);
        yield return new WaitForSeconds(0.3f);
        animator.SetBool("exploding", false);
        transform.position = new Vector3(100, 100); //because shell is in exploding animation for double time, I have to do this nonsense job!
        yield return new WaitForSeconds(0.3f);
        ObjectPool.GetInstance().RecycleObj(gameObject);
    }
}
