using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public Toggle togglePause;
    [SerializeField] private GameObject pausePanel;


    void Update()
    {
        if (togglePause.isOn == false)
        {
            pauseGame();
        }
    }

    // Функция паузы в игре
    public void pauseGame()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0;
    }

    // Функция отжатия паузы в игре
    public void resumeGame()
    {
        pausePanel.SetActive(false);
        togglePause.isOn = true;
        Time.timeScale = 1;
    }

    // Переход в главное меню через панель паузы
    public void pauseToMenue()
    {
        SceneManager.LoadScene(0);
    }
}
