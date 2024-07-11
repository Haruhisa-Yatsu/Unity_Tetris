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
    public readonly int[,] _shape_Z =
    {
        {1, 0 },
        {0, -1 },
        {-1, -1 }
    };
    public readonly int[,] _shape_J =
    {
        {-1, 0 },
        {1, 0 },
        {-1, -1 }
    };
    public readonly int[,] _shape_L =
    {
        {-1, 0 },
        {1, 0 },
        {1, -1 }
    };
    public readonly int[,] _shape_T =
    {
        {-1, 0 },
        {1, 0 },
        {0, -1 }
    };
    public readonly int[,] _shape_O =
    {
        {-1, 0 },
        {0, -1 },
        {-1, -1 }
    };
    public readonly int[,] _shape_I =
    {
        {-1, 0 },
        {-2, 0 },
        {1, 0 }
    };

    public int[,] GetShapeData(ShapeType shapeType)
    {
        switch (shapeType)
        {
            case ShapeType.S:
                return _shape_S;
            case ShapeType.Z:
                return _shape_Z;
            case ShapeType.J:
                return _shape_J;
            case ShapeType.L:
                return _shape_L;
            case ShapeType.T:
                return _shape_T;
            case ShapeType.O:
                return _shape_O;
            case ShapeType.I:
                return _shape_I;
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

    /// <summary>
    /// 横位置
    /// </summary>
    private int _posX;

    /// <summary>
    /// 縦位置
    /// </summary>
    private int _posY;

    /// <summary>
    /// 形状データ
    /// </summary>
    private int[,] _shapeData;

    /// <summary>
    /// ミノの初期位置
    /// </summary>
    private readonly int _startPosX = 5;

    /// <summary>
    /// ミノの初期位置
    /// </summary>
    private readonly int _startPosY = -1;

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

    private ShapeType _currentShape;

    public void CreateBlocks()
    {
        //すでに生成されているブロックを削除
        for (int i = 0; i < 4; i++)
        {
            if (_blocks[i] != null)
            {
                Destroy(_blocks[i].gameObject);
                _blocks[i] = null;
            }
        }

        // 初期位置の設定
        _posX = _startPosX;
        _posY = _startPosY;

        _currentShape = (ShapeType)Random.Range(0, 7);
        // 形状データの取得
        _shapeData = GetShapeData(_currentShape);

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
                    if (MoveCheck(1))
                    {
                        _posX += 1;
                    }
                }

                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    if (MoveCheck(-1))
                    {
                        _posX += -1;
                    }
                }

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Rotate();
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

                for(int i = 0; i < Board.BOARD_HEIGHT; i++)
                {
                    if (_board.CheckLine(i))
                    {
                        _board.DeleteLine(i);
                    }
                }

                _state = State.Initialize;

                break;
            default:
                break;
        }

        _rectTransform.anchoredPosition = new Vector3(_posX * Board.BLOCK_SIZE, _posY * Board.BLOCK_SIZE, 0.0f);
    }

    private void Rotate()
    {
        if(_currentShape == ShapeType.O)
        {
            return;
        }

        int[,] tempShape = new int[4 - 1, 2];

        for (int i = 0; i < 4 - 1; i++)
        {
            var x = _shapeData[i, 0];
            var y = _shapeData[i, 1];

            tempShape[i, 0] = -y;
            tempShape[i, 1] = x;
        }

        bool check = false;
        for (int i = 0; i < 4 - 1; i++)
        {
            var b = _board.GetBlock(
                _posX + tempShape[i, 0],
                -(_posY - tempShape[i, 1])
                );
            if (b == null || b.gameObject.activeSelf)
            {
                check = true;
                break;
            }
        }

        if (check)
        {
            return;
        }

        for (int i = 1; i < 4; i++)
        {
            _shapeData[i - 1, 0] = tempShape[i - 1, 0];
            _shapeData[i - 1, 1] = tempShape[i - 1, 1];

            _blocks[i].transform.localPosition =
                new Vector3(
                    _shapeData[i - 1, 0],
                    -_shapeData[i - 1, 1],
                    0)
                * Board.BLOCK_SIZE;
        }
    }

    /// <summary>
    /// ミノを1行分下に移動させる
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
        for (int i = 0; i < 4; i++)
        {
            if (i == 0)
            {
                if (_posY <= -Board.BOARD_HEIGHT + 1)
                {
                    return true;
                }

                if (_board.GetBlock(_posX, -_posY + 1).gameObject.activeSelf)
                {
                    return true;
                }
            }
            else
            {
                if (_posY - _shapeData[i - 1, 1] <= -Board.BOARD_HEIGHT + 1)
                {
                    return true;
                }

                if (_board.GetBlock(
                    _posX + _shapeData[i - 1, 0],
                    -(_posY - _shapeData[i - 1, 1]) + 1
                    ).gameObject.activeSelf)
                {
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// ミノの移動判定
    /// </summary>
    /// <returns></returns>
    private bool MoveCheck(int moveX)
    {
        for (int i = 0; i < 4; i++)
        {
            if (i == 0)
            {
                if (_posX + moveX < 0)
                {
                    return false;
                }
                if (_posX + moveX >= Board.BOARD_WIDTH)
                {
                    return false;
                }

                if (_board.GetBlock(_posX + moveX, -_posY).gameObject.activeSelf)
                {
                    return false;
                }
            }
            else
            {
                if (_posX + moveX + _shapeData[i - 1, 0] < 0)
                {
                    return false;
                }
                if (_posX + moveX + _shapeData[i - 1, 0] >= Board.BOARD_WIDTH)
                {
                    return false;
                }

                if (_board.GetBlock(
                    _posX + moveX + _shapeData[i - 1, 0],
                    -(_posY - _shapeData[i - 1, 1])
                    ).gameObject.activeSelf)
                {
                    return false;
                }
            }
        }
        return true;
    }
}
