using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TextAnimation : MonoBehaviour
{
    [SerializeField] private float _waitColor;
    [SerializeField] private Image _image;

    private float _timeCount;
    private float _colorA = 0f;

    private bool _isColorUp = false;

    private void Start()
    {
        _image = GetComponent<Image>();
    }

    private void Update()
    {
        _timeCount += Time.deltaTime;
        if (_colorA <= 0.5 && !_isColorUp && _waitColor < _timeCount)
        {
            _colorA += 0.04f;
            _image.color = new Color(255, 255, 255, _colorA);
            _timeCount = 0;

            if (_colorA > 0.5) 
            {
                _isColorUp = true;
            }
        }
        else if (_colorA >= 0 && _isColorUp && _waitColor < _timeCount) 
        {
            _colorA -= 0.04f;
            _image.color = new Color(255, 255, 255, _colorA);
            _timeCount = 0;

            if (_colorA == 0) 
            {
                _isColorUp = false;
            }
        }
    }

}
