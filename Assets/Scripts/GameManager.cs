using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public EnemyTank enemyTank;
    public OurTank player1, player2;
    public GameObject gameoverPanel;

    public Canvas canvas;
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
        canvas.enabled = true;
        gameoverPanel.SetActive(false);

        //int player = GameObject.Find("Choice").GetComponent<StartUp>().selection;
        int player = 2;
        FormQueue();
        SpawnEnemyTank();
        SpawnOurTank(1);
        if (player == 2)
            SpawnOurTank(2);
        else
        {
            player2.gameObject.SetActive(false);
        }
        canvas.enabled = false;
        GameObject.DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GameOver()
    {
        canvas.enabled = true;
        player1.m_Dead = true;
    }


    public void SpawnEnemyTank()
    {
        print("spawn enemy tank");
        while (liveEnemy < 1 && enemyBorn < 20)
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
        print("spawn our tank");
        if (player == 1)
            StartCoroutine(player1.Born());
        else
            StartCoroutine(player2.Born());
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
