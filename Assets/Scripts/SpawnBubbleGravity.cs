using UnityEngine;
using System.Collections;

public class SpawnBubbleGravity : MonoBehaviour {

    public GameObject BubbleGravity;
    public float spawnTime = 0.5f;            // How long between each spawn.
    public Transform spawnPosition;         // An array of the spawn points this enemy can spawn from.
    [SerializeField]
    private int objectSpawneds = 0;

    // Use this for initialization
    void Start () {
        InvokeRepeating("Spawn", spawnTime, spawnTime);
    }

    void Update(){
        if(objectSpawneds >= 300){
            CancelInvoke();
        }
    }

    void Spawn()
    {
        spawnPosition = gameObject.transform;
        spawnPosition.localPosition = new Vector3(Random.Range(-9, 9), 9.6f, 0);

        objectSpawneds++;
        // Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
        Instantiate(BubbleGravity, spawnPosition.localPosition, Quaternion.identity);
    }
}
