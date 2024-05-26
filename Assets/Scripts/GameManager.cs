using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    private GameObject gameOverPanel; // ゲームオーバーパネル

    // ゲームオーバーか
    private bool isGameOver;

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

        // ゲームオーバーパネルを非表示
        if (gameOverPanel.activeInHierarchy)
        {
            gameOverPanel.SetActive(false);
        }
    }

    private void Update()
    {
        // 有効なブロックが存在かつゲームオーバーでなければ移動
        if (activeBlock && !isGameOver)
        {
            // プレイヤーの入力
            PlayerInput();

            // ブロックの自然落下
            DropBlock();
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

    // 自然落下処理
    public void DropBlock()
    {
        // 自然落下時間を超えているか判定
        if (Time.time > nextDropTimer)
        {
            // 次のブロックが落下する時間を設定
            nextDropTimer = Time.time + dropInterval;

            // ブロックを下に移動
            activeBlock.MoveDown();

            // ブロックが底に達しているか判定
            if (!board.CheckPosition(activeBlock))
            {
                // ブロックの位置をボードに格納
                activeBlock.MoveUp();
                board.StoreBlockPosition(activeBlock);

                if (board.IsOverLimit(activeBlock))
                {
                    // 上限を超えていればゲームオーバー
                    GameOver();
                }
                else
                {
                    // 上限に達していない場合は終了処理
                    BottomBoard();
                }
            }
        }
    }

    // ブロックが底に到達したときの処理
    public void BottomBoard()
    {
        // ボードの行を削除
        board.ClearAllRows();

        // 新規にブロックを生成
        activeBlock = spawner.SpawnBlock();

        // タイマーをリセット
        nextKeyDownTimer = Time.time + keyDownInterval;
        nextKeyLeftRightTimer = Time.time + keyLeftRightInterval;
        nextKeyRotateTimer = Time.time + keyRotateInterval;
    }

    // ゲームオーバー処理
    public void GameOver()
    {
        // ゲームオーバーパネルを表示
        gameOverPanel.SetActive(true);

        // ゲームオーバー
        isGameOver = true;
    }

    // シーンの再読み込みする（ボタン押下で呼ぶ）
    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
