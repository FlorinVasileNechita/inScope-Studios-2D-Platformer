using UnityEngine;
using System.Collections;

public class _Enemy : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Q)) {
            CombatTextManager.Instance.CreateText(transform.position, "-10", Color.red, true);
        }
	}
}