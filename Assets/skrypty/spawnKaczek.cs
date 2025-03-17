using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class spawnKaczek : MonoBehaviour
{
    [System.Serializable]
    public class SpawnableObject
    {
        public GameObject prefab;
        public int maxCount;
        public int pointValue;
    }

    [System.Serializable]
    public class SpawnArea
    {
        public Transform spawnBlock; // Blok, w którym modele będą się pojawiać
        public int maxObjects;       // Maksymalna liczba obiektów w tym obszarze
        public List<GameObject> spawnedObjects = new List<GameObject>(); // Obiekty na planszy
    }

    public List<SpawnableObject> spawnablePrefabs; // Lista obiektów do spawnowania
    public float spawnInterval = 5f;       // Czas pomiędzy spawnowaniem modeli
    public List<SpawnArea> spawnAreas;     // Lista obszarów spawnowania z limitami
    public int startMax = 5;               // Globalny maksymalny limit obiektów

    private int totalSpawnedObjects = 0;   // Licznik obecnych obiektów w grze
    private Dictionary<GameObject, int> objectPoints = new Dictionary<GameObject, int>(); // Słownik przechowujący punkty za obiekty
    
    public static int kill = 0; //zmienna do zabic

    private void Start()
    {
        InvokeRepeating("WaitAndSpawnn", 0f, 5f);
    }

    void WaitAndSpawnn()
    {
        StartCoroutine(WaitAndSpawn());
    }

    private void Update()
    {
        if (totalSpawnedObjects < startMax)
        {
            SpawnModel();
        }
    }

    private void SpawnModel()
    {
        foreach (var area in spawnAreas)
        {
            if (area.spawnedObjects.Count < area.maxObjects)
            {
                // Wybierz losowy prefab z listy
                SpawnableObject selectedObject = GetRandomSpawnableObject();
                if (selectedObject == null) continue;

                Vector3 randomPosition = new Vector3(
                    Random.Range(area.spawnBlock.position.x - area.spawnBlock.localScale.x / 2, area.spawnBlock.position.x + area.spawnBlock.localScale.x / 2),
                    area.spawnBlock.position.y + selectedObject.prefab.GetComponent<Collider>().bounds.size.y + area.spawnBlock.GetComponent<BoxCollider>().bounds.extents.y,
                    Random.Range(area.spawnBlock.position.z - area.spawnBlock.localScale.z / 2, area.spawnBlock.position.z + area.spawnBlock.localScale.z / 2)
                );

                float checkRadius = selectedObject.prefab.GetComponent<Collider>().bounds.extents.magnitude;
                Collider[] colliders = Physics.OverlapSphere(randomPosition, checkRadius);

                bool isCollisionFree = true;

                foreach (var collider in colliders)
                {
                    if (collider.gameObject != selectedObject.prefab)
                    {
                        isCollisionFree = false;
                        break;
                    }
                }

                if (isCollisionFree)
                {
                    Quaternion randomRotation = Quaternion.Euler(
                        Random.Range(0f, 0f),
                        Random.Range(0f, 360f),
                        Random.Range(0f, 0f)
                    );

                    GameObject newModel = Instantiate(selectedObject.prefab, randomPosition, randomRotation);
                    area.spawnedObjects.Add(newModel);
                    objectPoints[newModel] = selectedObject.pointValue; // Zapisz punkty za ten obiekt
                    totalSpawnedObjects++;
                }
            }
        }
    }

    private SpawnableObject GetRandomSpawnableObject()
    {
        int totalMaxObjects = 0;
        Dictionary<SpawnableObject, float> spawnChances = new Dictionary<SpawnableObject, float>();

        // Oblicz całkowitą maksymalną liczbę obiektów
        foreach (var obj in spawnablePrefabs)
        {
            totalMaxObjects += obj.maxCount;
        }

        // Oblicz szanse spawnu dla każdego obiektu
        foreach (var obj in spawnablePrefabs)
        {
            int currentCount = CountObjectsOfType(obj.prefab);
            int remainingCount = obj.maxCount - currentCount;
            
            if (remainingCount > 0)
            {
                float spawnChance = (float)remainingCount / totalMaxObjects;
                spawnChances.Add(obj, spawnChance);
            }
        }

        if (spawnChances.Count == 0) return null;

        // Wybierz obiekt na podstawie obliczonych szans
        float randomValue = Random.value;
        float currentSum = 0f;

        foreach (var pair in spawnChances)
        {
            currentSum += pair.Value;
            if (randomValue <= currentSum)
            {
                return pair.Key;
            }
        }

        return spawnChances.Keys.ElementAt(0);
    }

    private int CountObjectsOfType(GameObject prefab)
    {
        int count = 0;
        foreach (var area in spawnAreas)
        {
            foreach (var obj in area.spawnedObjects)
            {
                if (obj != null && obj.name.Contains(prefab.name))
                {
                    count++;
                }
            }
        }
        return count;
    }

    public int GetPointsForObject(GameObject obj)
    {
        if (objectPoints.ContainsKey(obj))
        {
            return objectPoints[obj];
        }
        return 0;
    }

    private IEnumerator WaitAndSpawn()
    {
        yield return new WaitForSeconds(spawnInterval);
        SpawnModel();
    }

    public void DestroyModel(GameObject modelToDestroy)//
    {
        foreach (var area in spawnAreas)
        {
            if (area.spawnedObjects.Contains(modelToDestroy))
            {
                area.spawnedObjects.Remove(modelToDestroy);
                totalSpawnedObjects--;

                if (objectPoints.ContainsKey(modelToDestroy))
                {
                    int points = objectPoints[modelToDestroy];
                    pkt.PKT += points;
                    objectPoints.Remove(modelToDestroy);
                    kill++;
                }

                SpawnModel();
                break;
            }
        }

        Destroy(modelToDestroy);
    }
}
