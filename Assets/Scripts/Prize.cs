using UnityEngine;


public class Prize : MonoBehaviour
{
    public enum Type {star, life, bomb, timer, helmet, shovel };
    public Type type;
    public Sprite[] sprites = new Sprite[6];

    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnEnable()
    {
        type = Type.star;// (Type)Random.Range(0, 6);
        gameObject.GetComponent<SpriteRenderer>().sprite = sprites[(int)type];
        gameObject.transform.position = new Vector2(Random.Range(-13, 13) * 0.25f, Random.Range(-11, 11) * 0.25f);

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "player1" || collision.name == "player2")
        {
            OurTank myTank = collision.GetComponent<OurTank>();
            if (type == Type.bomb)
            {
                EnemyTank[] tanks = FindObjectsOfType<EnemyTank>();
                foreach (EnemyTank tank in tanks)
                {
                    if (!tank.m_Dead)
                    {
                        tank.Health = 0;
                        tank.Die(myTank.m_PlayerNumber);
                    }
                }
            }
            else if(type == Type.life)
            {
                GameManager.GetInstance().playerLife[myTank.m_PlayerNumber - 1]++;
            }
            else if(type == Type.helmet)
            {
                myTank.OnInvinciblePrize();
            }
            else if(type == Type.star)
            {
                myTank.level++;
            }
            else if (type == Type.timer)
            {
                BattleManager.GetInstance().bulletTime = 8f;
            }
            else if (type == Type.shovel)
            {
                BattleManager.GetInstance().GetComponent<MapLoader>().OnPrizeShovel();
            }
            ObjectPool.GetInstance().RecycleObj(gameObject);
        }
    }

}
