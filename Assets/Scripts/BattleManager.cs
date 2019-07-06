using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    [HideInInspector] public int liveEnemy;
    [HideInInspector] public float bulletTime;
    OurTank[] ourTank = new OurTank[2];
    Image gameoverImage;
    const int totalEnemy = 20;
    int[] enemyTanks = { 5, 5, 5, 5 };
    int[] enemyQueue = new int[totalEnemy];

    int prizePerBattle = 3;
    bool[] prizeQueue = new bool[totalEnemy];
    int enemyBorn;
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
        ObjectPool.GetInstance().Clear();
        ourTank[0] = GameObject.Find("player1").GetComponent<OurTank>();
        ourTank[1] = GameObject.Find("player2").GetComponent<OurTank>();
        gameoverImage = GameObject.Find("ImageGameOver").GetComponent<Image>();
    }

    // Start is called before the first frame update
    void Start()
    {
        print("New battle");
        gameoverImage.enabled = false;
        enemyBorn = 0;
        FormEnemyQueue();
        SpawnEnemyTank();
        for (int i = 0; i < 2; i++)
        {
            if (gm.playerLife[i] > 0)
                ourTank[i].Born();
            else
                ourTank[i].gameObject.SetActive(false);

        }
        System.Array.Clear(gm.kill, 0, gm.kill.Length);
    }

    private void Update()
    {
        bulletTime -= Time.deltaTime;
    }

    public IEnumerator GameOver()
    {
        print("Game over!");
        gameoverImage.enabled = true;
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
            StartCoroutine(tank.Born(enemyQueue[enemyBorn], enemyBorn, prizeQueue[enemyBorn]));
            liveEnemy++;
            enemyBorn++;
        }

        if (liveEnemy == 0 && enemyBorn == totalEnemy)
        {
            gm.battleResult = GameManager.BattleResult.WIN;
            gm.playerLevel[0] = ourTank[0].level;
            gm.playerLevel[1] = ourTank[1].level;
            SceneManager.LoadScene("ScoreScene");
        }
    }


    void FormEnemyQueue()
    {
        int cursor = 0;
        for (int i = 0; i < enemyTanks.Length; i++)
        {
            for (int j = 0; j < enemyTanks[i]; j++)
                enemyQueue[cursor++] = i;
        }
        RandomElement.Shuffle(enemyQueue);

        for (int i = 0; i < prizePerBattle; i++)
            prizeQueue[i] = true;
        for (int i = prizePerBattle; i < totalEnemy; i++)
            prizeQueue[i] = false;
        RandomElement.Shuffle(prizeQueue);
    }


    public void OurTankDie(int player)
    {
        gm.playerLife[player - 1]--;
        if (gm.playerLife[0] == 0 && gm.playerLife[1] == 0)
        {
            StartCoroutine(GameOver());
        }
        else if (gm.playerLife[player - 1] > 0)
        {
            ourTank[player - 1].Born();
            ourTank[player - 1].level = 1;
        }
    }

}
