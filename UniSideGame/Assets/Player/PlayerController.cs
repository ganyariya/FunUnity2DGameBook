using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float horizontalSpeed;

    private Rigidbody2D rBody;

    private float horizontalAxisInput;

    private bool isJumpTriggered;
    [SerializeField]
    private LayerMask groundLayerMask;
    [SerializeField]
    private float jumpSpeed;

    private Animator animator;
    private string playingAnimationName;
    private string prevAnimationName;

    /// <summary>
    /// static は unity 起動時から通して 1 つのクラスにずっと紐づく
    /// 書き換えられる
    /// </summary>
    public static string gameState = GameState.Playing;

    private int totalScore = 0;


    void Start()
    {
        rBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playingAnimationName = PlayerAnimationCategory.PlayerStop;
        prevAnimationName = PlayerAnimationCategory.PlayerStop;
        gameState = GameState.Playing;
    }

    void Update()
    {
        if (gameState != GameState.Playing) return;

        // キャラの向きを変える
        horizontalAxisInput = Input.GetAxisRaw("Horizontal");
        if (horizontalAxisInput > 0) transform.localScale = new Vector2(1, 1);
        if (horizontalAxisInput < 0) transform.localScale = new Vector2(-1, 1);

        if (Input.GetButtonDown("Jump")) Jump();
    }

    void FixedUpdate()
    {
        if (gameState != GameState.Playing) return;

        // 円を発射して他のゲームオブジェクトに衝突するかをチェックする
        // プレイヤーの足元に 0.2f 反映の円を作成し, 0.0f の距離を飛ばす、つまりその場に留める
        bool onGround = Physics2D.CircleCast(
            transform.position,
            0.2f,
            Vector2.down,
            0.0f,
            groundLayerMask
        );

        if (onGround || horizontalAxisInput != 0)
        {
            rBody.velocity = new Vector2(horizontalAxisInput * horizontalSpeed, rBody.velocity.y);
        }
        if (onGround && isJumpTriggered)
        {
            rBody.AddForce(new Vector2(0, jumpSpeed), ForceMode2D.Impulse);
            isJumpTriggered = false;
        }

        UpdateAnimation(onGround);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Goal")
        {
            Goal();
        }
        else if (collision.gameObject.tag == "Dead")
        {
            GameOver();
        }
        else if (collision.gameObject.tag == "ScoreItem")
        {
            ItemData itemData = collision.gameObject.GetComponent<ItemData>();
            AcquireScore(itemData);
        }
    }

    private void UpdateAnimation(bool onGround)
    {
        if (onGround) playingAnimationName = horizontalAxisInput == 0 ? PlayerAnimationCategory.PlayerStop : PlayerAnimationCategory.PlayerMove;
        else playingAnimationName = PlayerAnimationCategory.PlayerJump;

        if (playingAnimationName != prevAnimationName)
        {
            prevAnimationName = playingAnimationName;
            animator.Play(playingAnimationName);
        }
    }

    private void Jump()
    {
        isJumpTriggered = true;
    }

    private void Goal()
    {
        animator.Play(PlayerAnimationCategory.PlayerGoal);
        gameState = GameState.GameClear;
        GameStop();
    }

    public void GameOver()
    {
        animator.Play(PlayerAnimationCategory.PlayerGameOver);
        gameState = GameState.GameOver;
        GameStop();

        // 当たり判定を消す
        GetComponent<CapsuleCollider2D>().enabled = false;
        rBody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
    }

    private void GameStop()
    {
        rBody.velocity = Vector2.zero;
    }

    private void AcquireScore(ItemData itemData)
    {
        totalScore += itemData.GetValue();
        itemData.DestroyObject();
    }
}
