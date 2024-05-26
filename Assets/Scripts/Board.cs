using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    // ボードの状態
    private Transform[,] grid = new Transform[10, 30];

    [SerializeField]
    private Transform emptySprite;

    [SerializeField]
    private int height = 30, width = 10, header = 8;

    private void Start()
    {
        CreateBoard();
    }

    void CreateBoard()
    {
        if (emptySprite)
        {
            for (int y = 0; y < height - header; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Transform clone = Instantiate(
                        emptySprite,
                        new Vector3(x, y, 0),
                        Quaternion.identity
                    );

                    clone.transform.parent = transform;
                }
            }
        }
    }

    // ブロックが移動可能か判定
    public bool CheckPosition(Block block)
    {
        foreach (Transform child in block.transform)
        {
            Vector2 pos = Vector2Int.RoundToInt(child.position);

            // ボードの枠内に存在するか
            if (!IsInsideBorder(pos))
            {
                return false;
            }

            // 移動先にブロックが存在するか
            if (IsExistBlock(pos, block))
            {
                return false;
            }
        }

        return true;
    }

    // ボード内にあるか判定
    bool IsInsideBorder(Vector2 pos)
    {
        return (int)pos.x >= 0 && (int)pos.x < width && (int)pos.y >= 0;
    }

    // 移動先にブロックが存在するか判定
    public bool IsExistBlock(Vector2 pos, Block block)
    {
        return grid[(int)pos.x, (int)pos.y] != null && grid[(int)pos.x, (int)pos.y].parent != block.transform;
    }

    // ブロックの位置を格納
    public void StoreBlockPosition(Block block)
    {
        foreach (Transform child in block.transform)
        {
            Vector2 pos = Vector2Int.RoundToInt(child.position);
            grid[(int)pos.x, (int)pos.y] = child;
        }
    }
}
