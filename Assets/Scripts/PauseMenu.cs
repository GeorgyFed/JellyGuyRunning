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

    // ������� ����� � ����
    public void pauseGame()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0;
    }

    // ������� ������� ����� � ����
    public void resumeGame()
    {
        pausePanel.SetActive(false);
        togglePause.isOn = true;
        Time.timeScale = 1;
    }

    // ������� � ������� ���� ����� ������ �����
    public void pauseToMenue()
    {
        SceneManager.LoadScene(0);
    }
}
