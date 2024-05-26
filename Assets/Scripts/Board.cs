using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
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

    // ブロックがボード内にあるか判定
    public bool CheckPosition(Block block)
    {
        foreach (Transform child in block.transform)
        {
            Vector2 pos = Vector2Int.RoundToInt(child.position);

            if (!IsInsideBorder(pos))
            {
                return false;
            }
        }

        return true;
    }

    // ボード内にあるか判定
    bool IsInsideBorder(Vector2 pos)
    {
        return ((int)pos.x >= 0 && (int)pos.x < width && (int)pos.y >= 0);
    }
}
