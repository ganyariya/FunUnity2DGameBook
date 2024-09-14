using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private GameObject player;
    [SerializeField]
    private GameObject subScreen;

    [SerializeField]
    private float leftLimit = 0f;
    [SerializeField]
    private float rightLimit = 0f;
    [SerializeField]
    private float topLimit = 0f;
    [SerializeField]
    private float bottomLimit = 0f;

    [SerializeField]
    private bool isForceScrollX = false;
    [SerializeField]
    private float forceScrollSpeedX = 0f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (player == null) return;
        if (subScreen == null) return;

        // player 基準で動かす
        var x = player.transform.position.x;
        var y = player.transform.position.y;
        var z = transform.position.z;

        // プレイヤー関係なくカメラを強制的に動かす
        if (isForceScrollX) x = transform.position.x + (forceScrollSpeedX * Time.deltaTime);

        // 超えてはいけない範囲は出ないようにする
        x = Mathf.Clamp(x, leftLimit, rightLimit);
        y = Mathf.Clamp(y, topLimit, bottomLimit);

        FollowPlayer(x, y, z);
        ScrollSubScreen(x);
    }

    void FollowPlayer(float x, float y, float z)
    {
        // プレイヤー位置を追従するようにカメラを動かす
        transform.position = new Vector3(x, y, z);
    }

    void ScrollSubScreen(float x)
    {
        var y = subScreen.transform.position.y;
        var z = subScreen.transform.position.z;

        subScreen.transform.position = new Vector3(x / 2.0f, y, z);
    }
}
