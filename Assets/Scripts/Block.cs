using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    // 回転可能か
    [SerializeField]
    private bool canRotate = true;

    // 衝突中の同じ種類のオブジェクト一覧 
    private List<GameObject> collisionObjects = new List<GameObject>();

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

    // 衝突時の判定
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 同じプレハブ同士が衝突したときリストに存在しない場合
        if (collision.gameObject.name == gameObject.name && !collisionObjects.Contains(collision.gameObject))
        {
            collisionObjects.Add(collision.gameObject);
        }

        // 衝突中の同じ種類のオブジェクト一覧が2つ以上のとき
        if (collisionObjects.Count >= 2)
        {
            // 全てのオブジェクトを削除
            foreach (GameObject obj in collisionObjects.ToArray())
            {
                Destroy(obj);
            }

            // 自身も削除
            Destroy(gameObject);
        }
    }

    // 分離時の判定
    private void OnCollisionExit2D(Collision2D collision)
    {
        // 同じプレハブ同士が分離したときにリストに存在する場合
        if (collision.gameObject.name == gameObject.name && collisionObjects.Contains(collision.gameObject))
        {
            // 衝突中のオブジェクト一覧から削除
            collisionObjects.Remove(collision.gameObject);
        }
    }
}
