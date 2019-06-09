using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public OurTank ourTank;
    public EnemyTank enemyTank; 
    int[] enemyTanks = { 5, 5, 5, 5 };
    int[] enemyQueue;
    int enemyBorn = 0;
    [HideInInspector] public int liveEnemy = 0;

    // Start is called before the first frame update
    void Start()
    {
        FormQueue();
        SpawnEnemyTank();
        SpawnOurTank(1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void SpawnEnemyTank()
    {
        while (liveEnemy < 4 && enemyBorn <20)
        {
            print("Tank " + enemyQueue[enemyBorn] + " born at " + enemyBorn % 3);
            EnemyTank tank = Instantiate(enemyTank);
            StartCoroutine(  tank.Born(enemyQueue[enemyBorn], enemyBorn % 3));
            liveEnemy++;
            enemyBorn++;
        }

    }

    void SpawnOurTank(int player)
    {
        OurTank tank = Instantiate(ourTank);
        StartCoroutine(tank.Born(player));
    }

    void FormQueue()
    {
        enemyQueue = new int[20];

        int cursor = 0;
        for (int i = 0; i<enemyTanks.Length; i++)
        {
            for (int j=0; j<enemyTanks[i];j++)
                enemyQueue[cursor++] = i;
        }
        Shuffle(enemyQueue);
        //print("Enemy queue " + enemyQueue);
    }


    void Shuffle(int[] deck)
    {
        for (int i = 0; i < deck.Length; i++)
        {
            int temp = deck[i];
            int randomIndex = Random.Range(0, deck.Length);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
    }

}
