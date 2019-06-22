using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public int[,] kill;


    OurTank player1, player2;
    GameObject gameoverPanel, scorePanel;
    private int[] playerLife = { 3, 3 };
    [HideInInspector] public int liveEnemy;
    int[] enemyTanks = { 5, 5, 5, 5 };
    int[] enemyQueue = new int[20];
    int totalEnemy = 20;
    int enemyBorn = 0;
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

        player1 = GameObject.Find("player1").GetComponent<OurTank>();
        player2 = GameObject.Find("player2").GetComponent<OurTank>();
        gameoverPanel = GameObject.Find("PanelGameOver");
        scorePanel = GameObject.Find("ScorePanel");
    }

    // Start is called before the first frame update
    void Start()
    {
        gameoverPanel.SetActive(false);
        scorePanel.SetActive(false);
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
        GameObject.DontDestroyOnLoad(gameObject);
        kill = new int[2, 4];
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator GameOver()
    {
        gameoverPanel.SetActive(true);
        player1.m_Dead = true;
        player2.m_Dead = true;
        yield return new WaitForSeconds(1.5f);
        gameoverPanel.SetActive(false);
        scorePanel.SetActive(true);
        yield return new WaitForSeconds(10f);
        SceneManager.LoadScene(0);
    }

    public IEnumerator PassStage()
    {
        yield return new WaitForSeconds(0.5f);

        scorePanel.SetActive(true);
        ShowScore();
        yield return new WaitForSeconds(20f);
        scorePanel.SetActive(false);
    }

    public void ShowScore()
    {
        int[] scoreArray = { 100, 200, 300, 400 }; // score of 4 types of enemy tank
        Text P1T1 = GameObject.Find("P1T1").GetComponent<Text>();
        P1T1.text = kill[0, 0].ToString();
        Text P1S1 = GameObject.Find("P1S1").GetComponent<Text>();
        P1S1.text = (kill[0, 0] * scoreArray[0]).ToString() + " PTS";
        Text P1T2 = GameObject.Find("P1T2").GetComponent<Text>();
        P1T2.text = kill[0, 1].ToString();
        Text P1S2 = GameObject.Find("P1S2").GetComponent<Text>();
        P1S2.text = (kill[0, 1] * scoreArray[1]).ToString() + " PTS";
        Text P1T3 = GameObject.Find("P1T3").GetComponent<Text>();
        P1T3.text = kill[0, 2].ToString();
        Text P1S3 = GameObject.Find("P1S3").GetComponent<Text>();
        P1S3.text = (kill[0, 2] * scoreArray[2]).ToString() + " PTS";
        Text P1T4 = GameObject.Find("P1T4").GetComponent<Text>();
        P1T4.text = kill[0, 3].ToString();
        Text P1S4 = GameObject.Find("P1S4").GetComponent<Text>();
        P1S4.text = (kill[0, 3] * scoreArray[3]).ToString() + " PTS";
        Text P1T = GameObject.Find("P1T").GetComponent<Text>();
        P1T.text = (kill[0, 0] + kill[0, 1] + kill[0, 2] + kill[0, 3]).ToString();
        Text P1S = GameObject.Find("P1S").GetComponent<Text>();
        P1S.text = (kill[0, 0] * scoreArray[0] + kill[0, 1] * scoreArray[1] + kill[0, 2] * scoreArray[2] + kill[0, 3] * scoreArray[3]).ToString() + " PTS";
        Text P2T1 = GameObject.Find("P2T1").GetComponent<Text>();
        P2T1.text = kill[1, 0].ToString();
        Text P2S1 = GameObject.Find("P2S1").GetComponent<Text>();
        P2S1.text = (kill[1, 0] * scoreArray[0]).ToString() + " PTS";
        Text P2T2 = GameObject.Find("P2T2").GetComponent<Text>();
        P2T2.text = kill[1, 1].ToString();
        Text P2S2 = GameObject.Find("P2S2").GetComponent<Text>();
        P2S2.text = (kill[1, 1] * scoreArray[1]).ToString() + " PTS";
        Text P2T3 = GameObject.Find("P2T3").GetComponent<Text>();
        P2T3.text = kill[1, 2].ToString();
        Text P2S3 = GameObject.Find("P2S3").GetComponent<Text>();
        P2S3.text = (kill[1, 2] * scoreArray[2]).ToString() + " PTS";
        Text P2T4 = GameObject.Find("P2T4").GetComponent<Text>();
        P2T4.text = kill[1, 3].ToString();
        Text P2S4 = GameObject.Find("P2S4").GetComponent<Text>();
        P2S4.text = (kill[1, 3] * scoreArray[3]).ToString() + " PTS";
        Text P2T = GameObject.Find("P2T").GetComponent<Text>();
        P2T.text = (kill[1, 0] + kill[1, 1] + kill[1, 2] + kill[1, 3]).ToString();
        Text P2S = GameObject.Find("P2S").GetComponent<Text>();
        P2S.text = (kill[1, 0] * scoreArray[0] + kill[1, 1] * scoreArray[1] + kill[1, 2] * scoreArray[2] + kill[1, 3] * scoreArray[3]).ToString() + " PTS";
    }

    public void SpawnEnemyTank()
    {
        while (liveEnemy < 4 && enemyBorn < totalEnemy)
        {
            //print("Tank No " + enemyBorn + " type " + enemyQueue[enemyBorn] + " born at " + enemyBorn % 3);
            GameObject tankInstance = ObjectPool.GetInstance().GetObject("EnemyTank");
            EnemyTank tank = tankInstance.GetComponent<EnemyTank>();
            StartCoroutine(tank.Born(enemyQueue[enemyBorn], enemyBorn % 3));
            liveEnemy++;
            enemyBorn++;
        }

        if (liveEnemy == 0 && enemyBorn == totalEnemy)
        {
            StartCoroutine(PassStage());
        }
    }

    void SpawnOurTank(int player)
    {
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

    public void OurTankDie(int player)
    {
        playerLife[player - 1]--;
        if (playerLife[0] == 0 && playerLife[1] == 0)
        {
            GameOver();
        }
        else if (playerLife[player - 1] > 0)
        {
            SpawnOurTank(player);
        }
    }
}
