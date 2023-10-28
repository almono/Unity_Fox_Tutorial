using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashCooldownBar : MonoBehaviour
{
    public static DashCooldownBar instance;
    public float cooldownTime;
    private Slider barSlider;

    public Image barImage;
    public Color emptyColor;
    public Color fullColor;

    private void Awake()
    {
        instance = this;
        barSlider = GetComponent<Slider>();
    }

    public void Start()
    {
        barImage.fillAmount = 1;
    }

    public void Update()
    {
        if (cooldownTime > 0)
        {
            cooldownTime -= Time.deltaTime;
            //barImage.fillAmount = Mathf.Clamp01(1 - (cooldownTime));
            barSlider.value = Mathf.Clamp01(1 - (cooldownTime));

            // Interpolate the color between fullColor and emptyColor based on the fill amount.
            barImage.color = Color.Lerp(emptyColor, fullColor, Mathf.Clamp01(1 - (cooldownTime)));
        }
    }

    // You can use a method to trigger the cooldown (e.g., when a skill is used).
    public void StartCooldown(float cooldownDuration)
    {
        cooldownTime = cooldownDuration;
    }
}
