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

    /// <summary>
    /// 色を設定する
    /// </summary>
    /// <param name="color"></param>
    public void SetColor(Color color)
    {
        _image.color = color;
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
