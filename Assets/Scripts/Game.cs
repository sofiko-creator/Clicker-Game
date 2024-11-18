using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Game : MonoBehaviour
{
    [SerializeField] int Score;

    private int ClickScore = 1;
    private int CountLvl = 0;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI CostText;
    public TextMeshProUGUI AddScoreText;
    public TextMeshProUGUI LvlText;
    public int Cost;
    public Button ButtonUpgrade;

    void Update()
    {
        ScoreText.text = Score.ToString();
        if (Score >= Cost)
        {
            ButtonUpgrade.interactable = true;
        }
        else
        {
            ButtonUpgrade.interactable = false;
        }
    }

    public void OnClickButton()
    {
        Score = Score + ClickScore;
    }

    public void OnClickUpgrade()
    {
        if(Score >= Cost)
        {
            // Уменьшение текущего счёта на стоимость улучшения
            Score -= Cost;

            // Увеличение уровня улучшения
            CountLvl++;

            // Увеличение стоимости улучшения
            Cost = Mathf.CeilToInt(10 * Mathf.Pow(1.15f, CountLvl)); // 15% рост стоимости

            // Увеличение прироста за клик с учётом смещения
            ClickScore = Mathf.CeilToInt(1 * Mathf.Pow(1 + CountLvl, 1.2f));

            // Обновление UI
            CostText.text = "Upgrade " + Cost.ToString();
            AddScoreText.text = "+" + ClickScore.ToString();
            LvlText.text = "LV " + CountLvl.ToString();
        }
    }
}
