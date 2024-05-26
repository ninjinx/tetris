using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // ブロック生成器
    Spawner spawner;
    // 有効なブロック
    Block activeBlock;
    // ボード
    Board board;

    private float nextKeyDownTimer; // 下移動入力を管理するタイマー
    private float nextKeyLeftRightTimer; // 左右移動入力を管理するタイマー
    private float nextKeyRotateTimer; // 回転入力を管理するタイマー


    [SerializeField]
    private float keyDownInterval; // 下移動入力のインターバル
    [SerializeField]
    private float keyLeftRightInterval; // 左右移動入力のインターバル
    [SerializeField]
    private float keyRotateInterval; // 回転入力のインターバル

    [SerializeField]
    private float dropInterval = 0.25f; // ブロックが落下すまでのインターバル
    float nextDropTimer; // 次にブロックが落下するまでの時間

    private void Start()
    {
        // FindObjectOftype
        spawner = GameObject.FindObjectOfType<Spawner>();

        // ボードを取得
        board = GameObject.FindObjectOfType<Board>();

        // 生成器の位置を修正
        spawner.transform.position = Vector3Int.RoundToInt(spawner.transform.position);

        // タイマーの初期化
        nextKeyDownTimer = Time.time + keyDownInterval;
        nextKeyLeftRightTimer = Time.time + keyLeftRightInterval;
        nextKeyRotateTimer = Time.time + keyRotateInterval;

        // ブロック生成
        if (!activeBlock)
        {
            activeBlock = spawner.SpawnBlock();
        }
    }

    private void Update()
    {
        // 有効なブロックがあれば移動
        if (activeBlock)
        {
            // プレイヤーの入力を受け付ける
            PlayerInput();

            // 自然落下時間を超えているか判定
            if (Time.time > nextDropTimer)
            {
                // 次のブロックが落下する時間を設定
                nextDropTimer = Time.time + dropInterval;

                // 自然落下処理を実行
                activeBlock.MoveDown();
                if (!board.CheckPosition(activeBlock))
                {
                    BottomBoard();
                }
            }
        }
    }

    // ブロックの移動
    void PlayerInput()
    {
        // 右移動
        if (Input.GetKey(KeyCode.D) && (Time.time > nextKeyLeftRightTimer)
         || Input.GetKeyDown(KeyCode.D))
        {
            activeBlock.MoveRight();
            nextKeyLeftRightTimer = Time.time + keyLeftRightInterval;

            if (!board.CheckPosition(activeBlock))
            {
                activeBlock.MoveLeft();
            }
        }

        // 左移動
        if (Input.GetKey(KeyCode.A) && (Time.time > nextKeyLeftRightTimer)
         || Input.GetKeyDown(KeyCode.A))
        {
            activeBlock.MoveLeft();
            nextKeyLeftRightTimer = Time.time + keyLeftRightInterval;

            if (!board.CheckPosition(activeBlock))
            {
                activeBlock.MoveRight();
            }
        }

        // 下移動
        if (Input.GetKey(KeyCode.S) && (Time.time > nextKeyDownTimer)
         || Input.GetKeyDown(KeyCode.S))
        {
            activeBlock.MoveDown();
            nextKeyDownTimer = Time.time + keyDownInterval;

            if (!board.CheckPosition(activeBlock))
            {
                activeBlock.MoveUp();
            }
        }

        // 右回転
        if (Input.GetKey(KeyCode.E) && (Time.time > nextKeyRotateTimer)
         || Input.GetKeyDown(KeyCode.E))
        {
            activeBlock.RotateRight();
            nextKeyRotateTimer = Time.time + keyRotateInterval;

            if (!board.CheckPosition(activeBlock))
            {
                activeBlock.RotateLeft();
            }
        }

        // 左回転 
        if (Input.GetKey(KeyCode.Q) && (Time.time > nextKeyRotateTimer)
         || Input.GetKeyDown(KeyCode.Q))
        {
            activeBlock.RotateLeft();
            nextKeyRotateTimer = Time.time + keyRotateInterval;

            if (!board.CheckPosition(activeBlock))
            {
                activeBlock.RotateRight();
            }
        }
    }


    // ブロックが底に到達したときの処理
    public void BottomBoard()
    {
        // ブロックの位置をボードに格納
        activeBlock.MoveUp();
        board.StoreBlockPosition(activeBlock);

        // 新規にブロックを生成
        activeBlock = spawner.SpawnBlock();

        // タイマーをリセット
        nextKeyDownTimer = Time.time + keyDownInterval;
        nextKeyLeftRightTimer = Time.time + keyLeftRightInterval;
        nextKeyRotateTimer = Time.time + keyRotateInterval;
    }
}
