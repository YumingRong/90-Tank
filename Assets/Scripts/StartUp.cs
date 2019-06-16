using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartUp : MonoBehaviour
{
    [HideInInspector] public int selection = 1;
    private Toggle onePlayer, twoPlayers;


    // Start is called before the first frame update
    void Start()
    {
        GameObject.DontDestroyOnLoad(gameObject);
        onePlayer = GameObject.Find("Toggle1Player").GetComponent<Toggle>();
        twoPlayers = GameObject.Find("Toggle2Players").GetComponent<Toggle>();
        onePlayer.Select();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Submit"))
        {
            if (onePlayer.isOn)
                selection = 1;
            else if (twoPlayers.isOn)
                selection = 2;
            SceneManager.LoadScene("BattleScene");
        }

    }
}
