using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // ������ �������� �� ����������� ���������, ���� ��� �������� � ����������� �� �� ��������
    // ������ �������� �� ���� ����� � �������, ������� �����, ��������� � ���������� �������� ������
    // ������ ����������� ������������ � ������������, ����� ������ ���������
    // ������ ������������� � ������������ ���� ��� ������� ������ �����
    // ������ �������� �� ������ � ����� �������
 
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

        // ������� �� ������ ����������� ����� � ������� �� �����
        coins = PlayerPrefs.GetInt("coins");
        coinsText.text = coins.ToString();

        isImmortal = false;
        timerImage.enabled = false;

        // ������� �� ������ �������� ������������ ����� ��������� � ������
        isSound = PlayerPrefs.GetInt("ToggleSound");
        isAudio = PlayerPrefs.GetInt("ToggleAudio");

        // ������� ��������� ����� ������������� (���/����)
        if (isAudio == 1)
            audioSourse.volume = 0.1f;
        else
            audioSourse.volume = 0f;

        // �������� ����� ���������
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

        // �������� "Obsidian Guy" ���������� � ������ ����
        if (indexS == 5)
        {
            isImmortal = true;
        }
    }

    // ����������� ���������
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

    // ������� ������ 
    private void Jump()
    {
        if (isSound == 1)
            audioSourse.PlayOneShot(jump);
        dir.y = jumpForce;
    }

    // ������� ��������� ��������, ��������� ����������
    void FixedUpdate()
    {
        dir.z = speed;
        dir.y += gravity * Time.fixedDeltaTime;
        controller.Move(dir * Time.fixedDeltaTime);
    }

    // ������������ � ������������
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "obstacle")
        {
            // ���� �������� �������� ����� � �����������, �� ��������� ������ � ������� ��������
            if (isImmortal)
            {
                if (isSound == 1)
                    audioSourse.PlayOneShot(breakObj);
                Destroy(hit.gameObject);
            }
            // ����� ���� ���������������, ��������� ������ ���������
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


    // ������������ � ���������(�������� ��� �������)
    private void OnTriggerEnter(Collider other)
    {
        // �������� �������� �������
        if (other.gameObject.tag == "Coin")
        {
            if (isSound == 1)
                audioSourse.PlayOneShot(money);
            coins++;
            PlayerPrefs.SetInt("coins", coins);
            // ���� "Golden jelly Guy" ��������� ���������� �������
            if (indexS == 4)
            {
                coins++;
                PlayerPrefs.SetInt("coins", coins);
            }
            coinsText.text = coins.ToString();
            Destroy(other.gameObject);
        }

        // �������� �������� ����� ����������
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

    // ���������� ��������
    private IEnumerator SpeedIncrease()
    {
        yield return new WaitForSeconds(4);
        if (speed < maxSpeed)
        {
            speed += 1;
            StartCoroutine(SpeedIncrease());
        }
    }

    // ������� � 1 ������� � ������ ����
    private IEnumerator startStop()
    {
        lastSpeed = speed;
        speed = 0;
        yield return new WaitForSeconds(1);
        speed = lastSpeed;
    }

    // ������� ���� ������������ ��������� � �������� ����������� ������(�������������), ������� ���������� ����� �������� ������
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