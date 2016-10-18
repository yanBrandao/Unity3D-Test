using UnityEngine;
using System.Collections;

public class SpawnBubbleGravity : MonoBehaviour {

    public GameObject BubbleGravity;
    public float spawnTime = 0.5f; 
    public Transform spawnPosition; 
    [SerializeField]
    private int objectSpawneds = 0;

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
        Instantiate(BubbleGravity, spawnPosition.localPosition, Quaternion.identity);
    }
}
