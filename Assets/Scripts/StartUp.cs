using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartUp : MonoBehaviour
{
    private Toggle onePlayer, twoPlayers, construction;
    GameManager gm;


    // Start is called before the first frame update
    void Start()
    {
        onePlayer = GameObject.Find("Toggle1Player").GetComponent<Toggle>();
        twoPlayers = GameObject.Find("Toggle2Players").GetComponent<Toggle>();
        construction = GameObject.Find("ToggleConstruction").GetComponent<Toggle>();
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
            {
                gm.playerLife[0] = 3;
                gm.playerLife[1] = 3;
                gm.player = 2;
                SceneManager.LoadScene("BattleScene");
            }
            else if (onePlayer.isOn)
            {
                gm.playerLife[0] = 3;
                gm.playerLife[1] = 0;
                gm.player = 1;
                SceneManager.LoadScene("BattleScene");
            }
            else if (construction.isOn)
            {
                SceneManager.LoadScene("Construction");
            }
        }

    }
}
