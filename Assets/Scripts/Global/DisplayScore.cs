using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayScore : MonoBehaviour
{
    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private Text highScoreText;

    void Start()
    {
        scoreText.text += Score.currentScore;
        highScoreText.text += Score.highScore;
    }
}
