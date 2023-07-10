using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitUI_HpBar : UnitUI
{
    private const float COLOR_GREEN = 1.0f / 3.0f;
    private const float COLOR_RED = 0f;

    private CD_GameObject _unit;
    private Renderer _renderer;

    public float HpRate = 1f;

    private Vector3 _scale;
    private Vector3 _position;
    private Vector3 _offset;

    protected void Awake()
    {
        if (_unit == null)
        {
            this.transform.parent.TryGetComponent(out _unit);
        }

        TryGetComponent(out _renderer);

        _scale = this.transform.localScale;
        _position = this.transform.localPosition;
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();

        CalculateHpRate();

        SetScale();
        SetColor();
    }

    private void CalculateHpRate()
    {
        HpRate = (float)_unit.Hp / _unit.MaxHp;
    }

    private void SetScale()
    {
        Vector3 scale = _scale;
        scale.x *= HpRate;  // Set scale
        this.transform.localScale = scale;

        _offset.x = HpRate; // Set position
        this.transform.localPosition = _position - (_offset * 0.5f) + Vector3.right * 0.5f;
    }

    private void SetColor()
    {
        // rate가 0~1일 때, 색을 red~green으로 설정한다.
        // Red와 Green의 가시율 증가를 위해 rate를 제곱하여 구간을 보정하였다.
        // HSV으로 색채 사이값을 자연스럽게 보간함.
        _renderer.material.color = Color.HSVToRGB(Mathf.Lerp(COLOR_RED, COLOR_GREEN, Mathf.Pow(HpRate, 2)), 1f, 1f);
    }
}
