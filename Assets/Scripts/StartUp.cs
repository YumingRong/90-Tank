using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartUp : MonoBehaviour
{
    private Toggle onePlayer, twoPlayers;
    GameManager gm;


    // Start is called before the first frame update
    void Start()
    {
        onePlayer = GameObject.Find("Toggle1Player").GetComponent<Toggle>();
        twoPlayers = GameObject.Find("Toggle2Players").GetComponent<Toggle>();
        onePlayer.Select();
        gm = GameManager.GetInstance();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Submit"))
        {
            gm.playerLife[0] = 3;
            if (twoPlayers.isOn)
                gm.playerLife[1] = 3;
            else
                gm.playerLife[1] = 0;
            SceneManager.LoadScene("BattleScene");
        }

    }
}
