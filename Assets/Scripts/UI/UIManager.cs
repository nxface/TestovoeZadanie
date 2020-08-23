using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public GameObject ui_mainMenu;
    public GameObject ui_pause;
    public GameObject ui_gameOver;
    public GameObject ui_hud;




    public GameObject[] livesUI;

    public Text hud_score;
    public Text hud_waves;



    public Text gameover_score;
    public Text gameover_waves;
    public Text gameover_highscore;
    public Text gameover_maxwaves;


    public Button[] changeinputs;
    public Button[] changeinputs2;


    public void Chacngeinput(bool ci)
	{
		if (ci == false)
		{
			for (int i = 0; i < changeinputs.Length; i++)
			{
                changeinputs[i].gameObject.SetActive(false);
            }
            for (int i = 0; i < changeinputs2.Length; i++)
            {
                changeinputs2[i].gameObject.SetActive(true);
            }
        }
		else if (ci == true)
		{
            for (int i = 0; i < changeinputs2.Length; i++)
            {
                changeinputs2[i].gameObject.SetActive(false);
            }
            for (int i = 0; i < changeinputs.Length; i++)
            {
                changeinputs[i].gameObject.SetActive(true);
            }
        }
	}
    void DisableAllMenus()
    {
        ui_mainMenu.SetActive(false);

        ui_pause.SetActive(false);
        ui_gameOver.SetActive(false);
        ui_hud.SetActive(false);
    }

    public void ButtonClick(string menuName)
    {
        DisableAllMenus();
        switch (menuName)
        {
            case "MAIN_MENU":
                ui_mainMenu.SetActive(true);
                GameManager.instance.presentState = GameManager.GameState.UI_MENU;
                break;
            case "START":
                ui_hud.SetActive(true);
                GameManager.instance.StartGame();

                break;
            case "GAME_OVER":
                ui_gameOver.SetActive(true);
                gameover_score.text = "Счет : " + GameManager.instance.score.ToString();
                gameover_waves.text = "Волна : " + GameManager.instance.waves.ToString();
                break;
            case "PAUSE":
                ui_pause.SetActive(true);
                GameManager.instance.StopGamePlay();
                GameManager.instance.presentState = GameManager.GameState.PAUSED;
                break;
            case "REPLAY":
                ui_hud.SetActive(true);

                GameManager.instance.ReplayGame();
                break;
            case "RESUME":
                ui_hud.SetActive(true);

                GameManager.instance.ResumeGame();
                break;
            case "EXIT":
                Application.Quit();
                break;
        }
    }





    public void SetLivesUI(int count)
    {
        if (count < 4)
        {
            for (int i = 0; i < livesUI.Length; i++)
            {
                if (i < count)
                    livesUI[i].SetActive(true);
                else
                    livesUI[i].SetActive(false);
            }
        }
    }
    public void ResetLivesUI()
    {
        livesUI[0].SetActive(true);
        livesUI[1].SetActive(true);
        livesUI[2].SetActive(true);
    }


}
