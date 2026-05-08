using UnityEngine;
using System.Collections;

public class PopulationManager : MonoBehaviour
{
    public GameObject personPrefab; // Kéo Prefab người vào đây
    public Transform areaB;         // Kéo điểm đích B vào đây
    public float spawnRate = 2.0f;  // Cứ 2 giây tạo 1 người

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            GameObject newPerson = Instantiate(personPrefab, transform.position, Quaternion.identity);
            newPerson.GetComponent<PeopleAgent>().Setup(areaB);

            yield return new WaitForSeconds(spawnRate);
        }
    }
}