using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BarScript : MonoBehaviour {

    [SerializeField] private float fillAmount;
    [SerializeField] private Image content;

    public float MaxValue { get; set; }
    public float Value {
        set {
            fillAmount = Map(value, 0, MaxValue, 0, 1);
        }

    }

	void Start () {
	
	}
	
	void Update () {
        UpdateBar();
	}

    private void UpdateBar() {
        content.fillAmount = fillAmount;
    }

    private float Map(float value, float inMin, float inMax, float outMin, float outMax) {
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
        //(80 - 0) * (1 - 0) / (100 - 0) + 0 = 0.8
        //(78 - 0) * (1 - 0) / (230 - 0) + 0 = 0.3
    }
}