using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    // ������ �������� �� ��������� ����� ��������
    // ������ ���������� �� ����� ���� ��� ��������

    [SerializeField] Text coinsText;
    [SerializeField] private GameObject settingsPanel;
    private int sc;
    bool soundCheck;
    public Toggle toggleSound; 
    public Toggle toggleAudio;

    private void Start()
    {
        // ��������� �������� �����������
        sc = PlayerPrefs.GetInt("ToggleSound");
        if (sc == 1)
            soundCheck = true;
        else
            soundCheck = false;
        toggleSound.isOn = soundCheck;

        // ��������� �������� �����������
        sc = PlayerPrefs.GetInt("ToggleAudio");
        if (sc == 1)
            soundCheck = true;
        else
            soundCheck = false;
        toggleAudio.isOn = soundCheck;

        int coins = PlayerPrefs.GetInt("coins");
        coinsText.text = coins.ToString();
    }

    // ������� �� ����� ����
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    // ������� � �������
    public void goToStore()
    {
        SceneManager.LoadScene(2);
    }

    // ������� ������ ��������
    public void settingsMenu()
    {
        settingsPanel.SetActive(true);
    }

    // ������� ������ ��������
    public void closeSettingsMenu()
    {
        settingsPanel.SetActive(false);
    }

    // ������ �������� �����������
    public void booleanSoundSettings()
    {
        sc = toggleAudio.isOn ? 1 : 0;
        if (sc == 0)
        {
            PlayerPrefs.SetInt("ToggleSound", sc);
            soundCheck = false;
            toggleSound.isOn = soundCheck;
        }
        else
        {
            sc = toggleSound.isOn ? 1 : 0;
            PlayerPrefs.SetInt("ToggleSound", sc);
        }
    }
    // ��������� �������� �����������
    public void booleanAudioSettings()
    {
        sc = toggleAudio.isOn ? 1 : 0;
        PlayerPrefs.SetInt("ToggleAudio", sc);
        if (sc == 0)
        {
            PlayerPrefs.SetInt("ToggleSound", sc);
            soundCheck = false;
            toggleSound.isOn = soundCheck;
        }
        else
        {
            PlayerPrefs.SetInt("ToggleSound", sc);
            soundCheck = true;
            toggleSound.isOn = soundCheck;
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}