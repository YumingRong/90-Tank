using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    [HideInInspector] public int liveEnemy;
    OurTank[] ourTank = new OurTank[2];
    GameObject gameoverPanel, scorePanel;
    int[] enemyTanks = { 5, 5, 5, 5 };
    int[] enemyQueue = new int[20];
    int totalEnemy = 20;
    int enemyBorn = 0;
    GameManager gm;
    private static BattleManager instance;

    public static BattleManager GetInstance()
    {
        if (instance == null)
        {
            instance = GameObject.FindObjectOfType<BattleManager>();
        }
        return instance;
    }

    private void Awake()
    {
        gm = GameManager.GetInstance();
    }

    // Start is called before the first frame update
    void Start()
    {
        ourTank[0] = GameObject.Find("player1").GetComponent<OurTank>();
        ourTank[1] = GameObject.Find("player2").GetComponent<OurTank>();
        gameoverPanel = GameObject.Find("PanelGameOver");
        gameoverPanel.SetActive(false);
        FormQueue();
        SpawnEnemyTank();
        for (int i= 0; i<2;i++)
        {
            if (gm.playerLife[i] > 0)
                StartCoroutine(ourTank[i].Born());
            else
                ourTank[i].gameObject.SetActive(false);

        }
        gm.kill = new int[2, 4];

    }

    public IEnumerator GameOver()
    {
        print("Game over!");
        gameoverPanel.SetActive(true);
        ourTank[0].m_Dead = true;
        ourTank[1].m_Dead = true;
        gm.battleResult = GameManager.BattleResult.LOSE;
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("ScoreScene");

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
            gm.battleResult = GameManager.BattleResult.WIN;
            gm.PassStage();
            SceneManager.LoadScene("ScoreScene");
        }
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

    public void OurTankDie(int player)
    {
        gm.playerLife[player - 1]--;
        //print("Player 1 life " + gm.playerLife[0]);
        //print("Player 2 life " + gm.playerLife[1]);
        if (gm.playerLife[0] == 0 && gm.playerLife[1] == 0)
        {
            StartCoroutine(GameOver());
        }
        else if (gm.playerLife[player - 1] > 0)
        {
            StartCoroutine(ourTank[player - 1].Born());
        }
    }

}
