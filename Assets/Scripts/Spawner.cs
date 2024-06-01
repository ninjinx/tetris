using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // �z��
    [SerializeField]
    Block[] Blocks;

    // block�������_���ɑI��
    Block GetRandomBlock()
    {
        int i = Random.Range(0, Blocks.Length);

        if (Blocks[i])
        {
            return Blocks[i];
        }
        else
        {
            return null;
        }
    }

    public Block SpawnBlock()
    {
        // ブロックを生成
        Block block = Instantiate(GetRandomBlock(), transform.position, Quaternion.Euler(0, 0, Random.Range(0, 4) * 90));


        if (block)
        {
            return block;
        }
        else
        {
            return null;
        }
    }
}
