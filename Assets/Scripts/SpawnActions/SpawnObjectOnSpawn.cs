using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[UnityEngine.CreateAssetMenu(fileName = "New Spawn Object Action", menuName = "Card Spawn Actions/SpawnObject")]
public class SpawnObjectOnSpawn : CardActionOnSpawn
{
    public GameObject objectPrefab = null;
    public string receiverName = "";
    public string triggerName = "Play";
    public override void Act()
    {
        GameEvents.current.SpawnObject(objectPrefab);

        GameEvents.current.Execute(SpawnOnNextFrame());
    }

    public IEnumerator SpawnOnNextFrame()
    {
        yield return new WaitForFixedUpdate();
        GameEvents.current.PlayAnimation(receiverName, triggerName);
    }
}

