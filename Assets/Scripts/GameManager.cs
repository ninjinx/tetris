using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // ブロック生成器
    Spawner spawner;
    // 有効なブロック
    Block activeBlock;

    [SerializeField]
    private float dropInterval = 0.25f; // ブロックが落下すまでのインターバル
    float nextDropTimer; // 次にブロックが落下するまでの時間

    private void Start()
    {
        // FindObjectOftype
        spawner = GameObject.FindObjectOfType<Spawner>();

        // ブロック生成
        if (!activeBlock)
        {
            activeBlock = spawner.SpawnBlock();
        }
    }

    private void Update()
    {
        // 次のブロックが落下する時間を超えているか判定
        if (Time.time > nextDropTimer)
        {
            // 次のブロックが落下する時間を設定
            nextDropTimer = Time.time + dropInterval;

            // 有効なブロックがあれば移動
            if (activeBlock)
            {
                activeBlock.MoveDown();
            }
        }
    }
}
