using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    Spawner spawner;
    Block activeBlock;

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
        // 有効なブロックがあれば移動
        if (activeBlock)
        {
            activeBlock.MoveDown();
        }
    }
}
