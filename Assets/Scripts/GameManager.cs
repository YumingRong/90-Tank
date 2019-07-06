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

    int MaxStage = 3;
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
        SceneManager.LoadScene("StartupScene");
    }

    public void NextStage()
    {
        stage++;
        if (stage > MaxStage)
            stage = 1;
    }
}
