using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    // 回転可能か
    [SerializeField]
    private bool canRotate = true;

    // 移動
    public void Move(Vector3 moveDirection)
    {
        transform.position += moveDirection;
    }

    // 左に移動
    public void MoveLeft()
    {
        Move(new Vector3(-1, 0, 0));
    }

    // 右に移動
    public void MoveRight()
    {
        Move(new Vector3(1, 0, 0));
    }

    // 下に移動
    public void MoveDown()
    {
        Move(new Vector3(0, -1, 0));
    }

    // 上に移動
    public void MoveUp()
    {
        Move(new Vector3(0, 1, 0));
    }

    // 右に回転
    public void RotateRight()
    {
        if (canRotate)
        {
            transform.Rotate(0, 0, -90);
        }
    }

    // 左に回転
    public void RotateLeft()
    {
        if (canRotate)
        {
            transform.Rotate(0, 0, 90);
        }
    }
}
