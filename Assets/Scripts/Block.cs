using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Imageクラスを使用するのに必要
using UnityEngine.UI;

public class Block : MonoBehaviour
{
    /// <summary>
    /// Imageコンポーネント
    /// 画像を表示してくれるやつ
    /// </summary>
    public Image _image;

    private Mino.ShapeType _shapeType;

    private Color ShapeColor(Mino.ShapeType shapeType)
    {
        switch (_shapeType)
        {
            case Mino.ShapeType.S:
                return Color.green;
            case Mino.ShapeType.Z:
                return new Color(255.0f / 255.0f, 100.0f / 255.0f, 100.0f / 255.0f);
            case Mino.ShapeType.J:
                return new Color(70.0f / 255.0f, 100.0f / 255.0f, 255.0f / 255.0f); ;
            case Mino.ShapeType.L:
                return new Color(255.0f / 255.0f, 125.0f / 255.0f, 0.0f / 255.0f);
            case Mino.ShapeType.T:
                return Color.magenta;
            case Mino.ShapeType.O:
                return Color.yellow;
            case Mino.ShapeType.I:
                return new Color(75.0f / 255.0f, 255.0f / 255.0f, 255.0f / 255.0f);
            default:
                break;
        }

        return Color.white;
    }

    /// <summary>
    /// 色を設定する
    /// </summary>
    /// <param name="color"></param>
    private void SetColor(Color color)
    {
        _image.color = color;
    }

    /// <summary>
    /// 色を設定する
    /// </summary>
    /// <param name="color"></param>
    public void SetColor(Mino.ShapeType shapeType)
    {
        _shapeType = shapeType;
        SetColor(ShapeColor(shapeType));
    }

    /// <summary>
    /// 色を設定する
    /// </summary>
    /// <param name="color"></param>
    public void SetColor(Block block)
    {
        _shapeType = block._shapeType;
        SetColor(ShapeColor(_shapeType));
    }

    /// <summary>
    /// ブロックの表示設定
    /// </summary>
    /// <param name="b"></param>
    public void SetActive(bool b)
    {
        gameObject.SetActive(b);
    }

    // Start is called before the first frame update
    void Start()
    {
        //SetColor(new Color(1.0f,1.0f,1.0f));
    }

    // Update is called once per frame
    void Update()
    {

    }
}
