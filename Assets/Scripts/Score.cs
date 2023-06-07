using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    // Скрипт записывает и постоянно обновляет очки, полученные за игру
    // Очки  это растояние которое персонаж пробезал за одну игру

    [SerializeField] public Transform player;
    [SerializeField] public Text scoreText;


    private void Update()
    {
        scoreText.text = ((int)(player.position.z)).ToString();
    }
}
