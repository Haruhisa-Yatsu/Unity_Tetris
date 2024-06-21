using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mino : MonoBehaviour
{
    private RectTransform _rectTransform;

    /// <summary>
    /// ブロックのプレハブ　Inspectorから指定する
    /// </summary>
    public Block _blockPrefab;

    /// <summary>
    /// ミノを構成しているブロックたち
    /// </summary>
    private Block[] _blocks = new Block[1];

    private int _posX;
    private int _posY;

    private readonly int _startPosX = 5;
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


    public void CreateBlocks()
    {
        if (_blocks[0] != null)
        {
            Destroy(_blocks[0].gameObject);
            _blocks[0] = null;
        }

        _posX = _startPosX;
        _posY = _startPosY;

        var block = Instantiate(_blockPrefab, transform);

        block.SetColor(
            new Color(
                Random.Range(0.0f, 1.0f),
                Random.Range(0.0f, 1.0f),
                Random.Range(0.0f, 1.0f)
                )
            );

        _blocks[0] = block;
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

                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    _posX++;
                }

                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    _posX--;
                }

                break;
            case State.Landing:
                _state = State.Initialize;

                break;
            default:
                break;
        }

        _rectTransform.anchoredPosition = new Vector3(_posX * 100.0f, _posY * 100.0f, 0.0f);
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
        if(_posY <= -20 + 1)
        {
            return true;
        }

        return false;
    }
}
