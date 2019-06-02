using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public OurTank player1;
    public EnemyTank enemyTank; 
    int[] EnemyTanks = { 4, 5, 2, 3 };
    Vector2 ourSpawnPoint1 = new Vector2(-0.75f, -3f);
    Vector2 ourSpawnPoint2 = new Vector2(0.75f, -3f);

    // Start is called before the first frame update
    void Start()
    {
        player1.transform.position = ourSpawnPoint1;
        SpawnEnemyTank(1, 1);
        SpawnEnemyTank(2, 2);
        SpawnEnemyTank(3, 3);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnEnemyTank( int type, int position)
    {
        EnemyTank tank = Instantiate(enemyTank);
        StartCoroutine( tank.Born(type, position));
    }
}
