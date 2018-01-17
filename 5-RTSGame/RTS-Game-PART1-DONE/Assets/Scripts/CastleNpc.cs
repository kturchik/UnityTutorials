using UnityEngine;
using System.Collections;

public class CastleNpc : MonoBehaviour {
    // Parameters
    public float spawnInterval = 3.0f;

    // Use this for initialization
    void Start () {
        InvokeRepeating("SpawnHelp", spawnInterval, spawnInterval);
    }
    
    void SpawnHelp() {
        // this doesn't work directly with InvokeRepeating..
        GetComponent<UnitSpawner>().Spawn();
    }
}
