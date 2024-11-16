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

    private Save SaveObj = new Save();

    private void Awake()
    {
        if (PlayerPrefs.HasKey("SaveObj"))
        {
          //  SaveObj = JsonUtility.FromJson<Save>(PlayerPrefs.GetString("SaveObj"));
           // Score = SaveObj.Score;
           // Cost = SaveObj.Cost;
           // ClickScore = SaveObj.ClickScore;
          //  CountLvl = SaveObj.CountLvl;
          //  CostText.text = "Upgrade " + Cost.ToString();
          //  AddScoreText.text = "+" + ClickScore.ToString();
          //  LvlText.text = "LV " + CountLvl.ToString();
        }
    }

    // Update is called once per frame
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
            Score -= Cost;
            Cost *= 2;
            ClickScore *= 2;
            CostText.text = "Upgrade " + Cost.ToString();
            AddScoreText.text = "+" + ClickScore.ToString();
            CountLvl++;
            LvlText.text = "LV " + CountLvl.ToString();
        }
    }
    
    private void OnApplicationQuit()
    {
        SaveObj.Score = Score;
        SaveObj.Cost = Cost;    
        SaveObj.ClickScore = ClickScore;
        SaveObj.CountLvl = CountLvl;

        PlayerPrefs.SetString("SaveObj", JsonUtility.ToJson(SaveObj));
    }

}

[Serializable]
public class Save
{
    public int Score;
    public int ClickScore;
    public int CountLvl;
    public int Cost;
}
