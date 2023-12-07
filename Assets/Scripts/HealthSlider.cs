using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSlider : MonoBehaviour
{
    [SerializeField] Slider healthSlider;
    [SerializeField] Gradient healthGradient;

    [SerializeField] Image fillImage;
    int maxValue;

    public void SetSliderMaxValue(float value)
    {
        maxValue = (int)value;
        healthSlider.maxValue = maxValue;
        fillImage.color = healthGradient.Evaluate(1f);
    }

    public void ResetSlider()
    {
        healthSlider.value = maxValue;
        fillImage.color = healthGradient.Evaluate(1f);
    }
    public void ChangeCurrentHealthValue(float currentHealth)
    {
        healthSlider.value = currentHealth;
        fillImage.color = healthGradient.Evaluate(healthSlider.normalizedValue);
    }
}
