using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public OurTank player1, player2;
    int[] EnemyTanks = { 4, 5, 2, 3 };
    Vector2 ourSpawnPoint1 = new Vector2(-0.75f, -3f);
    Vector2 ourSpawnPoint2 = new Vector2(0.75f, -3f);
    Vector2[] enemySpawnPoint = { new Vector2(-3f, 3f), new Vector2(0f, 3f), new Vector2(3f,3f) };

    // Start is called before the first frame update
    void Start()
    {
        player1.transform.position = ourSpawnPoint1;
        player2.transform.position = ourSpawnPoint2;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnEnemyTank(int position, int type)
    {

    }
}
