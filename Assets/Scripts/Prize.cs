using UnityEngine;

public class Prize : MonoBehaviour
{
    public enum Type {star, life, bomb, timer, helmet, shovel };
    public Type type;
    public Sprite[] sprites = new Sprite[6];

    // Start is called before the first frame update
    void Start()
    {
        type = Type.helmet; // (Type)Random.Range(0, 6);
        gameObject.GetComponent<SpriteRenderer>().sprite = sprites[(int)type];
        gameObject.transform.position = new Vector2(Random.Range(-12, 12)* 0.25f, Random.Range(-12, 12) * 0.25f);
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
                myTank.m_level++;
            }
            else if (type == Type.timer)
            {
                EnemyTank.bulletTime = 5f;
            }
            else if (type == Type.shovel)
            {

            }
            ObjectPool.GetInstance().RecycleObj(gameObject);
        }
    }

    //void LoadSpriteByIO()
    //{
    //    //创建文件流
    //    string loadpath = @"J:\My Projects\90Tank\Assets\Sprites\道具\";
    //    FileStream fileStream = new FileStream(loadpath + type.ToString() + ".png", FileMode.Open, FileAccess.Read);
    //    fileStream.Seek(0, SeekOrigin.Begin);
    //    //创建文件长度的缓冲区
    //    byte[] bytes = new byte[fileStream.Length];
    //    //读取文件
    //    fileStream.Read(bytes, 0, (int)fileStream.Length);
    //    //释放文件读取liu
    //    fileStream.Close();
    //    fileStream.Dispose();
    //    fileStream = null;

    //    //创建Texture
    //    int width = 48;
    //    int height = 45;
    //    Texture2D texture2D = new Texture2D(width, height);
    //    texture2D.LoadImage(bytes);

    //    Sprite sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
    //    gameObject.GetComponent<SpriteRenderer>().sprite = sprite;

    //}


}
