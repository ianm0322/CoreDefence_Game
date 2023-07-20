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
    private Animator _anim;

    public float HpRate = 1f;
    public float VisibleDist = 50f;
    public float VisibleWide = 0.2f;

    private Vector3 _scale;
    private Vector3 _position;
    private Vector3 _offset;

    private Coroutine _mainCoroutine;

    protected void Awake()
    {
        if (_unit == null)
        {
            this.transform.parent.TryGetComponent(out _unit);
        }

        TryGetComponent(out _renderer);
        TryGetComponent(out _anim);

        _scale = this.transform.localScale;
        _position = this.transform.localPosition;
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();

        if (IsInsideOfView())
        {
            SetVisible(true);
            DrawHpBar();
        }
        else
        {
            SetVisible(false);
        }
    }

    private bool IsInsideOfView()
    {
        Vector2 vp = Camera.main.WorldToViewportPoint(_unit.transform.position);
        bool cond1 = VisibleWide < vp.x && vp.x < 1f- VisibleWide;
        bool cond2 = MathUtility.CompareDist(_unit.transform.position - Camera.main.transform.position, VisibleDist) < 0;
        return cond1 && cond2;
    }

    private void DrawHpBar()
    {
        CalculateHpRate();  // hp ���� ���
        SetScale();         // hp ������ ���� Ʈ������ ��ȯ
        SetColor();         // hp ������ ���� hp�� ���� ��ȯ
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
        // rate�� 0~1�� ��, ���� red~green���� �����Ѵ�.
        // Red�� Green�� ������ ������ ���� rate�� �����Ͽ� ������ �����Ͽ���.
        // HSV���� ��ä ���̰��� �ڿ������� ������.
        _renderer.material.color = Color.HSVToRGB(Mathf.Lerp(COLOR_RED, COLOR_GREEN, Mathf.Pow(HpRate, 2)), 1f, 1f);
    }

    private void SetVisible(bool onoff)
    {
        _renderer.enabled = onoff;
    }
}
