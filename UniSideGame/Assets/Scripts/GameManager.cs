using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private const int REMAIN_TIME_COEFFICIENT = 10;

    public GameObject mainImage;

    public Sprite gameOverSprite;
    public Sprite gameClearSprite;

    public GameObject buttonListPanel;

    public GameObject restartButton;
    public GameObject nextButton;

    Image titleImage;

    /// <summary>
    /// Canvas prefab 内に timeBar, timeText がいるため、そのまま prefab 自体にドラッグドロップで接続を設定できる
    /// しかし、Player は Canvas prefab 配下にいないため、 getTag で参照を実行時に設定する
    /// </summary>
    [SerializeField]
    private GameObject timeBar;
    [SerializeField]
    private Text timeText;
    [SerializeField]
    private TimeController timeController;

    /// <summary>
    /// ゲーム全体で共通している合計スコア」
    /// 
    /// </summary>
    public static int gameTotalScore = 0;

    private PlayerController playerController;

    [SerializeField]
    private Text scoreText;

    void Awake()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
    }


    void Start()
    {
        // ゲーム開始時は「mainImage」に最初に設定している gameStart sprite を表示する
        buttonListPanel.SetActive(false);
        Invoke("InactiveImage", 1.0f); // 1 秒後に mainImage を false にする

        // 制限時間がないなら隠す
        if (timeController != null && timeController.getMaxTimeLimit() == 0) timeBar.SetActive(false);
    }

    void Update()
    {
        if (PlayerController.gameState == GameState.GameClear)
        {
            mainImage.SetActive(true);
            buttonListPanel.SetActive(true);
            restartButton.GetComponent<Button>().interactable = false; // クリアしたので RESTART は無効化する
            mainImage.GetComponent<Image>().sprite = gameClearSprite; // ゲームクリアスプライトを設定する
            PlayerController.gameState = GameState.GameEnd;
            timeController.StopTimer();

            int remainTime = (int)timeController.getDisplayTime();
            UpdateGameScore(remainTime);
        }

        if (PlayerController.gameState == GameState.GameOver)
        {
            mainImage.SetActive(true);
            buttonListPanel.SetActive(true);
            nextButton.GetComponent<Button>().interactable = false;
            mainImage.GetComponent<Image>().sprite = gameOverSprite;
            PlayerController.gameState = GameState.GameEnd;
            timeController.StopTimer();
        }

        if (PlayerController.gameState == GameState.Playing)
        {
            if (timeController == null) return;
            if (timeController.getMaxTimeLimit() == 0f) return;

            var displayTime = (int)timeController.getDisplayTime();
            timeText.text = displayTime.ToString();
            if (displayTime == 0)
            {
                playerController.GameOver();
            }
        }

        UpdateScoreText();
    }

    void InactiveImage()
    {
        mainImage.SetActive(false);
    }

    void UpdateScoreText()
    {
        var score = gameTotalScore + playerController.GetTotalScore();
        scoreText.text = score.ToString();
    }

    void UpdateGameScore(int remainTime)
    {
        gameTotalScore += REMAIN_TIME_COEFFICIENT * remainTime;
        gameTotalScore += playerController.GetTotalScore();
        playerController.ResetTotalScore();
    }
}
