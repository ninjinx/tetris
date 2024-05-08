using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    Spawner spawner;
    Block activeBlock;

    private void Start()
    {
        // FindObjectOftypeは良くない
        spawner = GameObject.FindObjectOfType<Spawner>();

        // スポナークラスからブロック生成関数を読んで変数に格納する
        if (!activeBlock)
        {
            activeBlock = spawner.SpawnBlock();
        }
    }
}
