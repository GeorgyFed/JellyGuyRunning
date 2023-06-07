using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // Скрипт отвечает за перемещение персонажа, дает ему скорость и увеличивает ее со всеменем
    // Скрипт отвечает за сбор монет и бонусов, подсчет монет, включение и выключение действия бонуса
    // Скрипт отслеживает столкновение м препятствием, вызов панели проигрыша
    // Скрипт останавливает и возобновляет игру при нажатии кнопки паузы
    // Скрипт отвечает за музыку и аудио эффекты
 
    private AudioSource audioSourse;
    public AudioClip money;
    public AudioClip bonus;
    public AudioClip death;
    public AudioClip jump;
    public AudioClip slide;
    public AudioClip breakObj;

    private CharacterController controller;
    private Vector3 dir;
    [SerializeField] private GameObject mainPlayer;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private int coins;
    [SerializeField] private Text coinsText;
    [SerializeField] private Score scoreScript;
    [SerializeField] private float time;
    [SerializeField] private Image timerImage;

    public Toggle togglePause;

    private bool isImmortal;

    private int isSound;
    private int isAudio;

    private float gravity = -30;
    private int lineToMove = 1;
    public float lineDistance = 4;
    private float maxSpeed = 100;
    private float _timeLeft = 0f;
    private float lastSpeed;

    public int indexS;
    private bool[] StockCheck;
    public Transform player;


    void Start()
    {
        audioSourse = mainPlayer.GetComponent<AudioSource>();

        controller = GetComponent<CharacterController>();
        StartCoroutine(SpeedIncrease());
        Time.timeScale = 1;
        StartCoroutine(startStop());

        // Достаем из памяти колличество монет и выводим на экран
        coins = PlayerPrefs.GetInt("coins");
        coinsText.text = coins.ToString();

        isImmortal = false;
        timerImage.enabled = false;

        // Достаем из памяти значения выключателей общей громкости и звуков
        isSound = PlayerPrefs.GetInt("ToggleSound");
        isAudio = PlayerPrefs.GetInt("ToggleAudio");

        // Прверка настройки аудио сопровождения (вкл/выкл)
        if (isAudio == 1)
            audioSourse.volume = 0.1f;
        else
            audioSourse.volume = 0f;

        // Загрузка скина персонажа
        indexS = PlayerPrefs.GetInt("chosenSkin");
        StockCheck = new bool[6];
        if (PlayerPrefs.HasKey("StockArray"))
            StockCheck = PlayerPrefsX.GetBoolArray("StockArray");
        else
            StockCheck[0] = true;

        for (int i = 0; i < 6; i++)
        {
            if (i == indexS)
                player.GetChild(i).gameObject.SetActive(true);
            else
                player.GetChild(i).gameObject.SetActive(false);
        }

        // Персонаж "Obsidian Guy" бессмертен в начале игры
        if (indexS == 5)
        {
            isImmortal = true;
        }
    }

    // Перемещение персонажа
    private void Update()
    {
        if (SwipeController.swipeRight)
        {
            if (lineToMove < 2)
            {
                if (isSound == 1)
                    audioSourse.PlayOneShot(slide);
                lineToMove++;
            }
                
        }

        if (SwipeController.swipeLeft)
        {
            if (lineToMove > 0)
            {
                if (isSound == 1)
                    audioSourse.PlayOneShot(slide);
                lineToMove--;
            }
                
        }

        if (SwipeController.swipeUp)
        {
            if (controller.isGrounded)
                Jump();
        }

        if (SwipeController.swipeDown)
        {
            if (!controller.isGrounded)
                gravity = -600;
        }

        if (controller.isGrounded)
            gravity = -30;

        Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;
        if (lineToMove == 0)
            targetPosition += Vector3.left * lineDistance;
        else if (lineToMove == 2)
            targetPosition += Vector3.right * lineDistance;

        if (transform.position == targetPosition)
            return;
        Vector3 diff = targetPosition - transform.position;
        Vector3 moveDir = diff.normalized * 25 * Time.deltaTime;
        if (moveDir.sqrMagnitude < diff.sqrMagnitude)
            controller.Move(moveDir);
        else
            controller.Move(diff);
    }

    // Функция прыжка 
    private void Jump()
    {
        if (isSound == 1)
            audioSourse.PlayOneShot(jump);
        dir.y = jumpForce;
    }

    // Функция Обновляет скорость, добавляет гравитацию
    void FixedUpdate()
    {
        dir.z = speed;
        dir.y += gravity * Time.fixedDeltaTime;
        controller.Move(dir * Time.fixedDeltaTime);
    }

    // Столкновение с препятствием
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "obstacle")
        {
            // Если персонаж подобрал бонус и бессмертный, он разрушает объект в который врезался
            if (isImmortal)
            {
                if (isSound == 1)
                    audioSourse.PlayOneShot(breakObj);
                Destroy(hit.gameObject);
            }
            // Иначе игра останавливается, всплывает панель проигрыша
            else
            {
            if (isSound == 1)
                audioSourse.PlayOneShot(death);
            losePanel.SetActive(true);
            togglePause.interactable = false;
            int lastRunScore = int.Parse(scoreScript.scoreText.text.ToString());
            PlayerPrefs.SetInt("lastRunScore", lastRunScore);
            Time.timeScale = 0;
            }
        }
    }


    // столкновение с триггером(монеткой или бонусом)
    private void OnTriggerEnter(Collider other)
    {
        // Персонаж подобрал монетку
        if (other.gameObject.tag == "Coin")
        {
            if (isSound == 1)
                audioSourse.PlayOneShot(money);
            coins++;
            PlayerPrefs.SetInt("coins", coins);
            // Скин "Golden jelly Guy" удваивает полученные монетки
            if (indexS == 4)
            {
                coins++;
                PlayerPrefs.SetInt("coins", coins);
            }
            coinsText.text = coins.ToString();
            Destroy(other.gameObject);
        }

        // Персонаж подобрал бонус бессмертия
        if (other.gameObject.tag == "immortal")
        {
            if (isSound == 1)
                audioSourse.PlayOneShot(bonus);
            Destroy(other.gameObject);
            _timeLeft = time;
            timerImage.enabled = true;
             StartCoroutine(StartTimer());
        }
    }

    // Увеличение скорости
    private IEnumerator SpeedIncrease()
    {
        yield return new WaitForSeconds(4);
        if (speed < maxSpeed)
        {
            speed += 1;
            StartCoroutine(SpeedIncrease());
        }
    }

    // Задерка в 1 секунду в начале игры
    private IEnumerator startStop()
    {
        lastSpeed = speed;
        speed = 0;
        yield return new WaitForSeconds(1);
        speed = lastSpeed;
    }

    // Функция дает неуязвимость персонажу и вызывает графический объект(прямоугольник), который показывает время действия бонуса
    private IEnumerator StartTimer()
    {
        if (indexS == 5)
        {
            isImmortal = false;
        }
        else
        {
            isImmortal = true;
        }
        while (_timeLeft > 0)
        {
            _timeLeft -= Time.deltaTime;
            var normalizedValue = Mathf.Clamp(_timeLeft / time, 0.0f, 1.0f);
            timerImage.fillAmount = normalizedValue;
            yield return null;
        }
        if (indexS == 5)
        {
            isImmortal = true;
        }
        else
        {
            isImmortal = false;
        }
        timerImage.enabled = false;
        timerImage.fillAmount = 1;
    }
}