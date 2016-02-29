using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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

    public float speed;
    public Vector3 direction;

    public RectTransform canvas;
    public GameObject textPrefab;

    public void CreateText(Vector3 position, string text, Color color) {
        GameObject sct = (GameObject)Instantiate(textPrefab, position, Quaternion.identity);

        sct.transform.SetParent(canvas);
        sct.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        sct.GetComponent<CombatText>().Initialize(speed, direction);
        sct.GetComponent<Text>().text = text;
        sct.GetComponent<Text>().color = color;
    }

}