using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageVisualizer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI DamageText;
    [SerializeField] private TextMeshProUGUI HealText;

    public void ShowDamageText(int damage)
    {
        DamageText.gameObject.SetActive(true);
        DamageText.text = damage <= 0 ? "MISS" : $"-{damage.ToString()}";
    }

    public void ShowHealText(int heal)
    {
        HealText.gameObject.SetActive(true);
        HealText.text = $"+{heal.ToString()}";
    }
}
