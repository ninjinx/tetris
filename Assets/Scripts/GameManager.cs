using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    Spawner spawner;
    Block activeBlock;

    private void Start()
    {
        // FindObjectOftype�͗ǂ��Ȃ�
        spawner = GameObject.FindObjectOfType<Spawner>();

        // �X�|�i�[�N���X����u���b�N�����֐���ǂ�ŕϐ��Ɋi�[����
        if (!activeBlock)
        {
            activeBlock = spawner.SpawnBlock();
        }
    }
}
