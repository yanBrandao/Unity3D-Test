using UnityEngine;
using System.Collections;

public class SpawnBombBonus : MonoBehaviour {

    public GameObject BombBonus;
    public float spawnTime = 3f;            // How long between each spawn.
    public Transform spawnPosition;         // An array of the spawn points this enemy can spawn from.

    void Start()
    {
        InvokeRepeating("Spawn", spawnTime, spawnTime);
    }

    void Spawn()
    {
        spawnPosition.localPosition = new Vector3(Random.Range(-7f, 7.5f), Random.Range(2.44f, 13f), 0);
        while (Physics.CheckSphere(spawnPosition.localPosition, 1f)){
            spawnPosition.localPosition = new Vector3(Random.Range(-7f, 7.5f), Random.Range(2.44f, 13f), 0);
        }
        
        
        // Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
        Instantiate(BombBonus, spawnPosition.localPosition, Quaternion.identity);
    }
}
