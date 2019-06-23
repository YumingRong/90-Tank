using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int player = 2;
    public int stage = 1;
    [HideInInspector] public int[,] kill;
    [HideInInspector] public int[] playerLife = { 3, 3 };
    public enum BattleResult { WIN, LOSE};
    public BattleResult battleResult;

    int MaxStage = 1;
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

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.LoadScene("StartupScene");
    }

    public void PassStage()
    {
        stage++;
        if (stage > MaxStage)
            stage = 1;
    }
}
