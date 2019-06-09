using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public OurTank ourTank;
    public EnemyTank enemyTank;
    int[] enemyTanks = { 5, 5, 5, 5 };
    int[] enemyQueue = new int[20];
    int enemyBorn = 0;
    [HideInInspector] public int liveEnemy;
    private static GameManager instance;

    public static GameManager GetInstance()
    {
        if (instance == null)
        {
            instance = GameObject.FindObjectOfType<GameManager>();
        }
        return instance;
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;

        if (this != instance)
        {
            Debug.LogError("singleton:" + this.ToString() + " exists, remove it");
            GameObject.Destroy(this);
        }

    }

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

    public void GameOver()
    {

    }


    public void SpawnEnemyTank()
    {
        while (liveEnemy < 3 && enemyBorn < 20)
        {
            //print("Tank No " + enemyBorn + " type "  + enemyQueue[enemyBorn] +" born at " + enemyBorn % 3);
            GameObject tankInstance = ObjectPool.GetInstance().GetObject("EnemyTank");
            EnemyTank tank = tankInstance.GetComponent<EnemyTank>();
            StartCoroutine(tank.Born(enemyQueue[enemyBorn], enemyBorn % 3));
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
        int cursor = 0;
        for (int i = 0; i < enemyTanks.Length; i++)
        {
            for (int j = 0; j < enemyTanks[i]; j++)
                enemyQueue[cursor++] = i;
        }
        Shuffle(enemyQueue);

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

    void OnDestroy()
    {
        instance = null;
    }
}
