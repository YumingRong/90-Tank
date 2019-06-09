using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public OurTank ourTank;
    public EnemyTank enemyTank; 
    int[] EnemyTanks = { 4, 5, 2, 3 };

    // Start is called before the first frame update
    void Start()
    {
        SpawnOurTank(1);
        //SpawnEnemyTank(1, 1);
        SpawnEnemyTank(2, 2);
        //SpawnEnemyTank(3, 3);

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

    void SpawnOurTank(int player)
    {
        OurTank tank = Instantiate(ourTank);
        StartCoroutine(tank.Born(player));
    }
}
