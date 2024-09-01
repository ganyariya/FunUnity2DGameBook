using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject mainImage;

    public Sprite gameOverSprite;
    public Sprite gameClearSprite;

    public GameObject buttonListPanel;

    public GameObject restartButton;
    public GameObject nextButton;

    Image titleImage;

    void Start()
    {
        // ゲーム開始時は「mainImage」に最初に設定している gameStart sprite を表示する
        buttonListPanel.SetActive(false);
        Invoke("InactiveImage", 1.0f); // 1 秒後に mainImage を false にする
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
        }

        if (PlayerController.gameState == GameState.GameOver)
        {
            mainImage.SetActive(true);
            buttonListPanel.SetActive(true);
            nextButton.GetComponent<Button>().interactable = false;
            mainImage.GetComponent<Image>().sprite = gameOverSprite;
            PlayerController.gameState = GameState.GameEnd;
        }
    }

    void InactiveImage()
    {
        mainImage.SetActive(false);
    }
}
