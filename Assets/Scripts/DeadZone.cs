using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    // リミット時間
    float limitTime = 5.0f;

    [SerializeField]
    private GameManager gameManager; // ゲームマネージャー

    // ブロックが3秒間トリガーに触れていたらゲームオーバー
    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("残り" + limitTime + "秒");
        Debug.Log(collision.transform.parent.tag);

        // ボタンを押してない状態で親のゲームオブジェクトがBlockタグの場合
        if (!gameManager.isButtonDown && collision.transform.parent.tag == "Block")
        {
            limitTime -= Time.deltaTime;

            if (limitTime <= 0)
            {
                gameManager.GameOver();
            }
        }
    }

    // ブロックがトリガーから離れたらリミット時間をリセット
    private void OnTriggerExit2D(Collider2D collision)
    {
        // 親のゲームオブジェクトがBlockタグの場合
        if (collision.transform.parent.tag == "Block")
        {
            limitTime = 3.0f;
        }
    }
}
