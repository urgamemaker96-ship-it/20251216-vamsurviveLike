using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameInfoHud : MonoBehaviour
{
    [SerializeField] private Slider expSlider;
    [SerializeField] private Slider hpSlider;
    [SerializeField] private TextMeshProUGUI killText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI remianTimeText;

    public void UpdateExpSlider(int exp, int maxExp)
    {
        expSlider.value = exp / (float)maxExp;
    }

    public void UpdateKillText(int kill)
    {
        killText.SetText($"{kill}");
    }

    public void UpdateLevelText(int level)
    {
        levelText.SetText($"Lv.{level + 1}");
    }

    public void UpdateRemainTimeText(float remianTime)
    {
        int min = Mathf.FloorToInt(remianTime / 60);
        int sec = Mathf.FloorToInt(remianTime % 60);
        remianTimeText.SetText($"{min:00}:{sec:00}");
    }

    public void UpdateHpSillder(float hp, float maxHp)
    {
        hpSlider.value = hp / maxHp;
    }
}
