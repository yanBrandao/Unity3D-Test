using UnityEngine;
using System.Collections;

public class Collectable : MonoBehaviour {

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag.Equals("Bubble")) {
            Destroy(gameObject);
        }
    }
}
