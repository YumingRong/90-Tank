using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Score : MonoBehaviour
{
    GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.GetInstance();
        ShowScore();
    }

    public void ShowScore()
    {
        print("show score");
        int[,] kill;
        kill = GameManager.GetInstance().kill;
        int[] scoreArray = { 100, 200, 300, 400 }; // score of 4 types of enemy tank
        int[] playerScore = { 0, 0 };

        Text Stage = GameObject.Find("Stage").GetComponent<Text>();
        Stage.text = gm.stage.ToString();
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
        playerScore[0] = kill[0, 0] * scoreArray[0] + kill[0, 1] * scoreArray[1] + kill[0, 2] * scoreArray[2] + kill[0, 3] * scoreArray[3];
        P1S.text = playerScore[0].ToString() + " PTS";
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
        playerScore[1] = kill[1, 0] * scoreArray[0] + kill[1, 1] * scoreArray[1] + kill[1, 2] * scoreArray[2] + kill[1, 3] * scoreArray[3];
        P2S.text = playerScore[1].ToString() + " PTS";
        Text bonusTitle = GameObject.Find("BonusTitle").GetComponent<Text>();
        Text bonus = GameObject.Find("bonus").GetComponent<Text>();
        if (gm.player == 2)
        {
            if (playerScore[0] > playerScore[1])
            {
                bonusTitle.enabled = true;
                bonus.enabled = true;
                bonusTitle.transform.Translate(new Vector3(-333, -277));
                bonus.transform.Translate(new Vector3(-321, -320));
            }
            else if(playerScore[0] < playerScore[1])
            {
                bonusTitle.enabled = true;
                bonus.enabled = true;
                bonusTitle.transform.Translate(new Vector3(246, -277));
                bonus.transform.Translate(new Vector3(258, -320));
            }
            else
            {
                bonusTitle.enabled = false;
                bonus.enabled = false;
            }
        }
        else
        {
            bonusTitle.enabled = false;
            bonus.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Submit"))
        {
            if (gm.battleResult == GameManager.BattleResult.WIN)
            {
                gm.NextStage();

            }
            else
            {
                gm.playerLife[0] = 3;
                if(gm.player == 2)
                    gm.playerLife[1] = 3;
            }
            print("Load new battle");
            SceneManager.LoadScene("BattleScene");
        }
    }

}
