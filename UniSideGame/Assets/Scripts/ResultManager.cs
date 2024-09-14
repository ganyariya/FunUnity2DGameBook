using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    public Text scoreText;
    void Start()
    {
        scoreText.text = GameManager.gameTotalScore.ToString();
    }
}
