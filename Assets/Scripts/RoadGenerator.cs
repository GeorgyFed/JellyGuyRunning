using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : MonoBehaviour
{
    // Скрипт генерирует дорогу
    // Дорога состоит из 6 панелей, панель, находящаяся за персонажем удаляются, для оптимизации, а спереди появляется новая, панелей всегда 6 штук
    // Первая панель всегда стартовая и имеет индекс 0, другая панель выбирается из списка панелей (36 вариантов) случайным образом

    public GameObject[] roadPrefabs;
    private List<GameObject> activeRoads = new List<GameObject>();
    private float spawnPos = 0;
    private float roadLength = 100;
    private bool newGame = true;


    [SerializeField] private Transform player;
    private int startRoads = 6;

    // Создает первые 6 дорог
    void Start()
    {
        for (int i = 0; i < startRoads; i++)
        {
            if (i == 0 && newGame == true)
            {
                SpawnRoad(0);
                newGame = false;
            }
            SpawnRoad(Random.Range(1, roadPrefabs.Length));
        }
    }

    // Добавляет новую панель, удаляет последнюю
    void Update()
    {
        if (player.position.z - 60 > spawnPos - (startRoads * roadLength))
        {
            SpawnRoad(Random.Range(1, roadPrefabs.Length));
            DeleteRoad();
        }
    }

    // Функция добавляет панель из списка готовых дорог
    private void SpawnRoad(int roadIndex)
    {
        GameObject nextRoad = Instantiate(roadPrefabs[roadIndex], transform.forward * spawnPos, transform.rotation);
        activeRoads.Add(nextRoad);
        spawnPos += roadLength;
    }

    // Функция удаления дороги
    private void DeleteRoad()
    {
        Destroy(activeRoads[0]);
        activeRoads.RemoveAt(0);
    }
}
