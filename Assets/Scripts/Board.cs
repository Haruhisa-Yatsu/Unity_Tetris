using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public Transform _minoRoot;

    public Mino _mino;

    public Block _blockPrefab;

    /// <summary>
    /// 盤面の横幅
    /// </summary>
    public const int BOARD_WIDTH = 10;

    /// <summary>
    /// 盤面の縦幅
    /// </summary>
    public const int BOARD_HEIGHT = 20;

    /// <summary>
    /// ブロック一つの大きさ
    /// </summary>
    public const float BLOCK_SIZE = 100.0f;

    private Block[,] _boardData = new Block[BOARD_HEIGHT, BOARD_WIDTH];

    /// <summary>
    /// 指定された座標のブロックを返す
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public Block GetBlock(int x, int y)
    {
        if (x < 0)
        {
            return null;
        }

        if(x >= BOARD_WIDTH)
        {
            return null;
        }

        if(y < 0)
        {
            return null;
        }

        if(y >= BOARD_HEIGHT)
        {
            return null;
        }

        return _boardData[y, x];
    }


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < BOARD_HEIGHT; i++)
        {
            for (int j = 0; j < BOARD_WIDTH; j++)
            {
                var block = Instantiate(_blockPrefab, _minoRoot);
                block.GetComponent<RectTransform>().anchoredPosition = new Vector3(j * BLOCK_SIZE, -i * BLOCK_SIZE);
                block.SetActive(false);


                _boardData[i, j] = block;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
