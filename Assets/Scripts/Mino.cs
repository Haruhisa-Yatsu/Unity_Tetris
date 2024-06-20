using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mino : MonoBehaviour
{
    private RectTransform rectTransform;

    /// <summary>
    /// ブロックのプレハブ　Inspectorから指定する
    /// </summary>
    public Block blockPrefab;

    /// <summary>
    /// ミノを構成しているブロックたち
    /// </summary>
    private Block[] blocks = new Block[1];

    private int posX;
    private int posY;

    private readonly int startPosX = 5;
    private readonly int startPosY = -1;

  

    public void CreateBlocks()
    {
        if (blocks[0] != null)
        {
            Destroy(blocks[0].gameObject);
            blocks[0] = null;
        }

        posX = startPosX;
        posY = startPosY;

        var block = Instantiate(blockPrefab, transform);

        block.SetColor(
            new Color(
                Random.Range(0.0f, 1.0f),
                Random.Range(0.0f, 1.0f),
                Random.Range(0.0f, 1.0f)
                )
            );

        blocks[0] = block;
    }


    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        CreateBlocks();
    }

    // Update is called once per frame
    void Update()
    {
        rectTransform.anchoredPosition = new Vector3(posX * 100.0f, posY * 100.0f, 0.0f);
    }
}
