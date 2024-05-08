using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    // ïœêî
    [SerializeField]
    private Transform emptySprite;

    [SerializeField]
    private int height = 30, width = 10, header = 8;

    private void Start()
    {
        CreateBoard();
    }

    // ä÷êî
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
}
