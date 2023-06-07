using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : MonoBehaviour
{
    // ������ ���������� ������
    // ������ ������� �� 6 �������, ������, ����������� �� ���������� ���������, ��� �����������, � ������� ���������� �����, ������� ������ 6 ����
    // ������ ������ ������ ��������� � ����� ������ 0, ������ ������ ���������� �� ������ ������� (36 ���������) ��������� �������

    public GameObject[] roadPrefabs;
    private List<GameObject> activeRoads = new List<GameObject>();
    private float spawnPos = 0;
    private float roadLength = 100;
    private bool newGame = true;


    [SerializeField] private Transform player;
    private int startRoads = 6;

    // ������� ������ 6 �����
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

    // ��������� ����� ������, ������� ���������
    void Update()
    {
        if (player.position.z - 60 > spawnPos - (startRoads * roadLength))
        {
            SpawnRoad(Random.Range(1, roadPrefabs.Length));
            DeleteRoad();
        }
    }

    // ������� ��������� ������ �� ������ ������� �����
    private void SpawnRoad(int roadIndex)
    {
        GameObject nextRoad = Instantiate(roadPrefabs[roadIndex], transform.forward * spawnPos, transform.rotation);
        activeRoads.Add(nextRoad);
        spawnPos += roadLength;
    }

    // ������� �������� ������
    private void DeleteRoad()
    {
        Destroy(activeRoads[0]);
        activeRoads.RemoveAt(0);
    }
}
