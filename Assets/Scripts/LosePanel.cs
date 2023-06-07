using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LosePanel : MonoBehaviour
{
    // Скрипт панели проигрыша обновляет данные рекорда, паказывает наилучший результат
    // Скрипт панели проигрыша содержит функции перехода между сценами, можно перезапустить уровень или выйти в меню

    [SerializeField] Text recordText;

    private void Start()
    {
        // Обновляет рекорд

        int lastRunScore = PlayerPrefs.GetInt("lastRunScore");
        int recordScore = PlayerPrefs.GetInt("recordScore");

        if (lastRunScore > recordScore)
        {
            recordScore = lastRunScore;
            PlayerPrefs.SetInt("recordScore", recordScore);
            recordText.text = recordScore.ToString();
        }
        else
        {
            recordText.text = recordScore.ToString();
        }
    }

    // Загрузка сцены игры
    public void RestartLevel()
    {
        SceneManager.LoadScene(1);
    }

    // Загрузка сцены главного меню
    public void ToMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }
}
