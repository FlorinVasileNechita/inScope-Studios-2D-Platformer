using UnityEngine;
using System.Collections;

public class CombatTextManager : MonoBehaviour {
    private static CombatTextManager instance;
    public static CombatTextManager Instance {
        get {
            if (!instance) {
                instance = GameObject.FindObjectOfType<CombatTextManager>();
            }
            return instance;
        }
    }

    public RectTransform canvas;
    public GameObject textPrefab;

    public void CreateText(Vector3 position) {
        GameObject sct = (GameObject)Instantiate(textPrefab, position, Quaternion.identity);
        sct.transform.SetParent(canvas);
        sct.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
    }

}