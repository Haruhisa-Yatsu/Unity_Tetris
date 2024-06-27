using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mino : MonoBehaviour
{
    public enum ShapeType
    {
        S,
        Z,

        J,
        L,

        T,
        O,
        I
    }

    public readonly int[,] _shape_S =
    {
        {-1, 0 },
        {0, -1 },
        {1, -1 }
    };

    public int[,] GetShapeData(ShapeType shapeType)
    {
        switch (shapeType)
        {
            case ShapeType.S:
                return _shape_S;
            case ShapeType.Z:
                break;
            case ShapeType.J:
                break;
            case ShapeType.L:
                break;
            case ShapeType.T:
                break;
            case ShapeType.O:
                break;
            case ShapeType.I:
                break;
            default:
                break;
        }
        return null;
    }


    private RectTransform _rectTransform;

    /// <summary>
    /// ボード Inspectorから指定
    /// </summary>
    public Board _board;

    /// <summary>
    /// ブロックのプレハブ　Inspectorから指定する
    /// </summary>
    public Block _blockPrefab;

    /// <summary>
    /// ミノを構成しているブロックたち
    /// </summary>
    private Block[] _blocks = new Block[4];

    private int _posX;
    private int _posY;
    private int[,] _shapeData;
    private readonly int _startPosX = 5;
    private readonly int _startPosY = 0;

    /// <summary>
    /// 自然落下する秒数
    /// </summary>
    private float _fallTime = 1.0f;

    /// <summary>
    /// 自然落下のカウンタ
    /// </summary>
    private float _fallCount = 0.0f;


    public enum State
    {
        // 初期化
        Initialize,
        // 落下中
        Fall,
        // 着地
        Landing
    }
    private State _state = State.Initialize;


    public void CreateBlocks()
    {
        for (int i = 0; i < 4; i++)
        {
            if (_blocks[i] != null)
            {
                Destroy(_blocks[i].gameObject);
                _blocks[i] = null;
            }
        }

        _posX = _startPosX;
        _posY = _startPosY;

        _shapeData = GetShapeData(ShapeType.S);

        for (int i = 0; i < 4; i++)
        {
            var block = Instantiate(_blockPrefab, transform);

            if (i == 0)
            {
                block.transform.localPosition = new Vector3(0, 0, 0);
            }
            else
            {
                block.transform.localPosition =
                    new Vector3(
                        _shapeData[i - 1, 0],
                        -_shapeData[i - 1, 1],
                        0)
                    * Board.BLOCK_SIZE;
            }

            _blocks[i] = block;

            //block.SetColor(
            //    new Color(
            //        Random.Range(0.0f, 1.0f),
            //        Random.Range(0.0f, 1.0f),
            //        Random.Range(0.0f, 1.0f)
            //        )
            //    );


        }
    }


    // Start is called before the first frame update
    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();

    }

    // Update is called once per frame
    void Update()
    {
        switch (_state)
        {
            case State.Initialize:
                CreateBlocks();
                _state = State.Fall;

                break;
            case State.Fall:

                _fallCount += Time.deltaTime;

                if (Input.GetKeyDown(KeyCode.DownArrow) || _fallCount > _fallTime)
                {
                    _fallCount = 0.0f;
                    if (LandingCheck())
                    {
                        _state = State.Landing;
                    }
                    else
                    {
                        Fall();
                    }
                }

                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    while (!LandingCheck())
                    {
                        Fall();
                    }

                    _state = State.Landing;
                }

                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    _posX++;

                    if (_posX >= Board.BOARD_WIDTH)
                    {
                        _posX = Board.BOARD_WIDTH - 1;
                    }
                }

                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    _posX--;

                    if (_posX <= -1)
                    {
                        _posX = 0;
                    }
                }

                break;
            case State.Landing:

                for (int i = 0; i < 4; i++)
                {
                    Block block = null;
                    if (i == 0)
                    {
                        block = _board.GetBlock(_posX, -_posY);
                    }
                    else
                    {
                        block = _board.GetBlock(
                            _posX + _shapeData[i - 1, 0],
                            -(_posY - _shapeData[i - 1, 1])
                            );
                    }

                    block.SetActive(true);
                }

                _state = State.Initialize;

                break;
            default:
                break;
        }

        _rectTransform.anchoredPosition = new Vector3(_posX * Board.BLOCK_SIZE, _posY * Board.BLOCK_SIZE, 0.0f);
    }

    /// <summary>
    /// ミノを1列分下に移動させる
    /// </summary>
    private void Fall()
    {
        _posY--;
    }

    /// <summary>
    /// ミノの着地判定
    /// </summary>
    /// <returns></returns>
    private bool LandingCheck()
    {


        if (_posY <= -Board.BOARD_HEIGHT + 1)
        {
            return true;
        }

        if (_board.GetBlock(_posX, -_posY + 1).gameObject.activeSelf)
        {
            return true;
        }



        return false;
    }
}
