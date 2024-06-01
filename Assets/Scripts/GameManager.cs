using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // 下方向への射出係数（初期値）
    private float defaultDownForce = -10;

    // 最大の射出係数
    private float maxForce = 10;

    // 下方向への射出係数
    private float downForce = 0;
    // 左方向への射出係数
    private float leftForce = 0;
    // 右方向への射出係数
    private float rightForce = 0;

    // 単位時間ごとの射出係数の増加量
    private float forceIncrease = 20;

    // ブロック生成器
    Spawner spawner;
    // 有効なブロック
    Block activeBlock;
    // ボード
    Board board;

    private float nextKeyDownTimer; // 下移動入力を管理するタイマー
    private float nextKeyLeftRightTimer; // 左右移動入力を管理するタイマー
    private float nextKeyRotateTimer; // 回転入力を管理するタイマー

    // ゲームオーバーか
    private bool isGameOver;
    // ゲームが開始されたか
    private bool isGameStart;
    // ボタンを押しているか
    public bool isButtonDown;
    // スコア
    private int score;

    [SerializeField]
    private GameObject gameOverPanel; // ゲームオーバーパネル
    [SerializeField]
    private GameObject gameStartPanel; // ゲームスタートパネル

    [SerializeField]
    private float keyDownInterval; // 下移動入力のインターバル
    [SerializeField]
    private float keyLeftRightInterval; // 左右移動入力のインターバル
    [SerializeField]
    private float keyRotateInterval; // 回転入力のインターバル

    [SerializeField]
    private float dropInterval = 0.25f; // ブロックが落下すまでのインターバル
    float nextDropTimer; // 次にブロックが落下するまでの時間

    [SerializeField]
    private float spawnInterval = 0.1f; // ブロックが生成されるまでのインターバル
    float nextSpawnTimer; // 次にブロックが生成されるまでの時間

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

        // ステータスの初期化
        isGameStart = false;
        isGameOver = false;
        isButtonDown = false;

        // スコアの初期化
        score = 0;

        // ゲーム開始パネルを表示
        if (!gameStartPanel.activeInHierarchy)
        {
            gameStartPanel.SetActive(true);
        }

        // ゲームオーバーパネルを非表示
        if (gameOverPanel.activeInHierarchy)
        {
            gameOverPanel.SetActive(false);
        }
    }

    private void Update()
    {
        // ゲーム開始中か
        if (isGameStart && !isGameOver)
        {
            // プレイヤー入力
            PlayerInput();
        }
    }

    // ブロックの移動
    void PlayerInput()
    {
        // いずれかのキーが初めて押されたとき
        if (!isButtonDown &&
            Time.time > nextSpawnTimer &&
            (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D)))
        {
            // 次のブロックが生成可能になる時間を設定
            nextSpawnTimer = Time.time + spawnInterval;

            // 新規にブロックを生成
            activeBlock = spawner.SpawnBlock();

            // 重力を無効にする
            activeBlock.GetComponent<Rigidbody2D>().isKinematic = true;

            // ボタンを押している状態にする
            isButtonDown = true;
        }

        // 下方向の力をためる
        if (Input.GetKey(KeyCode.S) && downForce < maxForce && isButtonDown)
        {
            // 下方向への射出係数を時間に応じて増加
            downForce += Time.deltaTime * forceIncrease;
        }

        // 左方向の力をためる
        if (Input.GetKey(KeyCode.A) && leftForce < maxForce && isButtonDown)
        {
            leftForce += Time.deltaTime * forceIncrease;
        }

        // 右方向の力をためる
        if (Input.GetKey(KeyCode.D) && rightForce < maxForce && isButtonDown)
        {
            rightForce += Time.deltaTime * forceIncrease;
        }

        // いずれかのボタンが離されたとき
        if ((Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D)) && isButtonDown)
        {
            // ボタンを離した状態にする
            isButtonDown = false;

            // 重力を有効にする
            activeBlock.GetComponent<Rigidbody2D>().isKinematic = false;

            // 力を加える
            activeBlock.GetComponent<Rigidbody2D>().AddForce(new Vector2(rightForce - leftForce, defaultDownForce - downForce), ForceMode2D.Impulse);

            // 力をリセット
            downForce = 0;
            leftForce = 0;
            rightForce = 0;
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
        // ゲームオーバー状態に更新
        isGameOver = true;

        // スコアを上書き
        gameOverPanel.transform.Find("Score").GetComponent<TMPro.TextMeshProUGUI>().text = score.ToString();

        // ゲームオーバーパネルを表示
        gameOverPanel.SetActive(true);
    }


    // ゲームスタート処理
    public void GameStart()
    {
        // ゲームスタートパネルを非表示
        gameStartPanel.SetActive(false);

        // ブロック生成
        // if (!activeBlock)
        // {
        //     activeBlock = spawner.SpawnBlock();
        // }

        // ゲームスタート
        isGameStart = true;
    }

    // シーンの再読み込みする（ボタン押下で呼ぶ）
    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    // スコアを追加
    public void AddScore(int value)
    {
        score += value;
    }
}
