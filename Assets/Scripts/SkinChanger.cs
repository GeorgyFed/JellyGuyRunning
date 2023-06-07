using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SkinChanger : MonoBehaviour
{
    // Скрипт отвечает за покупку и выбор скина персонажа

    public Skin[] info;
    private bool[] StockCheck;

    public Button buyBttn;
    public TextMeshProUGUI priceText;
    public Text coinsText;
    public Transform player;
    public int index;
    public int coins;
    public Text nickName;

    // Переход в главное меню
    public void backToMenu()
    {
        SceneManager.LoadScene(0);
    }

    private void Awake()
    {
        // Сбор данных, колличество монет и выбранный скин
        coins = PlayerPrefs.GetInt("coins");
        index = PlayerPrefs.GetInt("chosenSkin");
        coinsText.text = coins.ToString();

        StockCheck = new bool[6];
        if (PlayerPrefs.HasKey("StockArray"))
            StockCheck = PlayerPrefsX.GetBoolArray("StockArray");
        else
            StockCheck[0] = true;

        info[index].isChosen = true;

        for (int i = 0; i < 6; i++)
        {
            info[i].inStock = StockCheck[i];
            if (i == index)
            {
                player.GetChild(i).gameObject.SetActive(true);
                changeNickName(i);
            }
            else
                player.GetChild(i).gameObject.SetActive(false);
        }

        priceText.text = "CHOSEN";
        buyBttn.interactable = false;
    }

    // Сохранение изменения массива данных
    public void Save()
    {
        PlayerPrefsX.SetBoolArray("StockArray", StockCheck);
    }

    // Перемещение по магазину вправо 
    public void ScrollRight()
    {
        if (index < 5)
        {
            index++;
            changeNickName(index);

            if (info[index].inStock && info[index].isChosen)
            {
                priceText.text = "CHOSEN";
                buyBttn.interactable = false;
            }
            else if (!info[index].inStock)
            {
                priceText.text = info[index].cost.ToString();
                buyBttn.interactable = true;
            }
            else if (info[index].inStock && !info[index].isChosen)
            {
                priceText.text = "CHOOSE";
                buyBttn.interactable = true;
            }

            for (int i = 0; i < player.childCount; i++)
                player.GetChild(i).gameObject.SetActive(false);

            player.GetChild(index).gameObject.SetActive(true);
        }
    }

    // Перемещение по магазину влево
    public void ScrollLeft()
    {
        if (index > 0)
        {
            index--;
            changeNickName(index);

            if (info[index].inStock && info[index].isChosen)
            {
                priceText.text = "CHOSEN";
                buyBttn.interactable = false;
            }
            else if (!info[index].inStock)
            {
                priceText.text = info[index].cost.ToString();
                buyBttn.interactable = true;
            }
            else if (info[index].inStock && !info[index].isChosen)
            {
                priceText.text = "CHOOSE";
                buyBttn.interactable = true;
            }

            for (int i = 0; i < player.childCount; i++)
                player.GetChild(i).gameObject.SetActive(false);

            player.GetChild(index).gameObject.SetActive(true);
        }
    }

    // Купить скин
    public void BuyButtonAction()
    {
        if (buyBttn.interactable && !info[index].inStock)
        {
            if (coins > int.Parse(priceText.text))
            {
                coins -= int.Parse(priceText.text);
                coinsText.text = coins.ToString();
                PlayerPrefs.SetInt("coins", coins);
                StockCheck[index] = true;
                info[index].inStock = true;
                priceText.text = "CHOOSE";
                Save();
            }
        }

        if (buyBttn.interactable && !info[index].isChosen && info[index].inStock)
        {
            PlayerPrefs.SetInt("chosenSkin", index);
            buyBttn.interactable = false;
            priceText.text = "CHOSEN";
        }
    }

    // Название скина
    public void changeNickName(int i)
    {
        if (i == 0)
            nickName.text = "Green jelly Guy";
        else if (i == 1)
            nickName.text = "Blue jelly Guy";
        else if (i == 2)
            nickName.text = "Red jelly Guy";
        else if (i == 3)
            nickName.text = "Turquoise jelly Guy";
        else if (i == 4)
            nickName.text = "Golden jelly Guy (2x coin)";
        else
            nickName.text = "Obsidian Guy (start immortal)";
    }

}

// поле данных для каждого скина
[System.Serializable] 
public class Skin
{
    public int cost;
    public bool inStock;
    public bool isChosen;
}