using UnityEngine;
using System.Collections;

public class CombatText : MonoBehaviour {
    private float speed;
    private float fadeTime;
    private Vector3 direction;
    
    
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        float translation = speed * Time.deltaTime;
        transform.Translate(direction * translation);
    }

    public void Initialize(float speed, Vector3 direction) {
        this.speed = speed;
        this.direction = direction;
    }
}