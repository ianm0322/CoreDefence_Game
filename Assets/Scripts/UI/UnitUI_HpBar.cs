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

    private Coroutine _fadeInOutCoroutine;

    private int TEST_STATE = 0; // 0=APPEARENCE, 1=DISAPPEARENCE

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

        if (IsInsideOfView())
        {
            _renderer.enabled = true;
            DrawHpBar();
        }
        else
        {
            _renderer.enabled = false;
        }
    }

    private bool IsInsideOfView()
    {
        Vector2 vp = Camera.main.WorldToViewportPoint(_unit.transform.position);
        return 0.2f < vp.x && vp.x < 0.8f;
    }

    private void DrawHpBar()
    {
        CalculateHpRate();  // hp 비율 계산
        SetScale();         // hp 비율에 따른 트랜스폼 변환
        SetColor();         // hp 비율에 따른 hp바 색상 변환
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

    private IEnumerator FadeInCoroutine()
    {
        yield return null;

        while (true)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, Time.deltaTime);
            float f = (transform.localScale - Vector3.one).sqrMagnitude;
            if (f * f < float.Epsilon)
            {
                yield break;
            }
            yield return null;
        }
    }

    private IEnumerator FadeOutCoroutine()
    {
        yield return null;

        while (true)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, Time.deltaTime);
            float f = (transform.localScale - Vector3.one).sqrMagnitude;
            if (f * f < float.Epsilon)
            {
                _fadeInOutCoroutine = null;
                yield break;
            }
            yield return null;
        }
    }
}
