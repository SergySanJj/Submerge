using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject container;

    void Start()
    {
        GameEvents.current.onSpawnObject += SpawnObject;
    }

    private void OnDestroy()
    {
        GameEvents.current.onSpawnObject -= SpawnObject;
    }

    public void SpawnObject(GameObject objectPrefab)
    {
        if (objectPrefab != null)
        {
            var instance = Instantiate(objectPrefab);
            instance.transform.SetParent(container.transform, false);
        }
    }

}
