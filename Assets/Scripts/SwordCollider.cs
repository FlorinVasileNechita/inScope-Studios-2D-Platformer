using UnityEngine;
using System.Collections;

public class SwordCollider : MonoBehaviour {
    [SerializeField]
    private string targetTag;

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == targetTag) {
            GetComponent<Collider2D>().enabled = false;
        }
    }
}
