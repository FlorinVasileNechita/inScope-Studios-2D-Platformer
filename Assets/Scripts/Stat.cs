using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Stat {
    [SerializeField] private BarScript bar;
    [SerializeField] private float maxValue;

    public float MaxValue {
        get { return maxValue; }
        set {
            bar.MaxValue = value;
            this.maxValue = value; 
        }
    }

    [SerializeField] private float currentValue;
    public float CurrentValue {
        get { return currentValue; }
        set {
            currentValue = Mathf.Clamp(value, 0, MaxValue);
            bar.Value = currentValue;
        }
    }

    public void Initialize() {
        this.MaxValue = maxValue;
        this.CurrentValue = currentValue;
    }
}