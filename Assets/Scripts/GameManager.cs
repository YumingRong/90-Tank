using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int player;
    public int stage;
    [HideInInspector] public int[,] kill = new int[2, 4];
    [HideInInspector] public int[] playerLife = { 3, 3 };
    [HideInInspector] public int[] playerLevel = { 1, 1 };
    public enum BattleResult { WIN, LOSE};
    [HideInInspector] public BattleResult battleResult;

    int MaxStage = 4;
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
            Destroy(this);
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        playerLevel[0] = playerLevel[1] = 1;
        SceneManager.LoadScene("StartupScene");
        print("player level " + playerLevel[0] + " " + playerLevel[1]);
    }

    public void NextStage()
    {
        stage++;
        if (stage > MaxStage)
            stage = 1;
    }
}
