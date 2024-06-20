using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Image�N���X���g�p����̂ɕK�v
using UnityEngine.UI;

public class Block : MonoBehaviour
{
    /// <summary>
    /// Image�R���|�[�l���g
    /// �摜��\�����Ă������
    /// </summary>
    public Image _image;

    /// <summary>
    /// �F��ݒ肷��
    /// </summary>
    /// <param name="color"></param>
    public void SetColor(Color color)
    {
        _image.color = color;
    }

    // Start is called before the first frame update
    void Start()
    {
        SetColor(new Color(1.0f,1.0f,1.0f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
