using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerMovement
{
    void Move(Vector3 movement);
    void Jump(float power);
    void SetActive(bool enabled);
}

public class PlayerBody : MonoBehaviour
{
    [Serializable]
    private struct RayTrBundle
    {
        public Transform foward;
        public Transform back;
        public Transform right;
        public Transform left;
        public Transform top;
        public Transform bottom;
    }

    [Serializable]
    public struct PlayerBodyInfo
    {
        public float avilableSlope;
    }

    private Rigidbody _rigid;
    private CapsuleCollider _collider;

    [SerializeField]
    private PlayerBodyInfo info;

    private Vector3 _lookDir;
    public Vector3 LookDirection
    {
        get
        {
            return _lookDir;
        }
        set
        {
            _lookDir.Set(
                Mathf.Clamp(value.x, _cameraXLimitMin, _cameraXLimitMax), // �Է�->��ȸ���� ��ȯ
                Mathf.Repeat(value.y, 360f),
                0f
            );
            this.transform.eulerAngles = Vector3.up * _lookDir.y;
        }
    }

    [SerializeField]
    private RayTrBundle rayPos;
    public bool IsGround { get; private set; }
    public bool IsFalling => (!IsGround && _rigid.velocity.y <= 0f);    // �������� = ���� �������� �ʾҴ�, �׸��� y���� ���� �������� �̵��ϰ� �ִ�.
    public bool IsJump { get; private set; }
    public float GroundSlope { get; private set; }
    public float GroundDist { get; private set; }
    public float FallTime { get; private set; }

    public bool UseGravity { get; set; } = true;
    public bool DontUpdateVelocity { get; set; } = false;

    public AnimationCurve gravityCurve;

    public Vector3 DeltaPosition { get; private set; }

    [Header("Properties")]
    [SerializeField]
    private LayerMask _groundLayer;
    [SerializeField]
    private float _cameraXLimitMax;
    [SerializeField]
    private float _cameraXLimitMin;

    #region Temp Variables
    private Ray _ray;
    private Vector3 _prePosition;

    private Vector3 _velocity;
    private Vector3 _moveVelocity;
    private Vector3 _gravityVelocity;
    #endregion

    private void Awake()
    {
        TryGetComponent(out _rigid);
        TryGetComponent(out _collider);
    }

    private void Start()
    {
        LookDirection = this.transform.eulerAngles;
    }

    private void Update()
    {
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        CheckGround(out hit);
        DeltaPosition = _rigid.position - _prePosition;
        _prePosition = _rigid.position;
        if(IsGround && GroundSlope <= info.avilableSlope && GroundSlope > float.Epsilon)
        {
            _collider.material.dynamicFriction = 1f;
        }
        else
        {
            _collider.material.dynamicFriction = 0f;
        }

        if (!DontUpdateVelocity)
        {
            GravityUpdate();

            if (IsGround)
            {
                // ���� ���鿡 ���� �̵� ������ �����ϴ� �޼ҵ�
                float slopePower = Mathf.Sin(GroundSlope * Mathf.Deg2Rad) * _moveVelocity.magnitude; // ���� ��翡 ���� ������ ��

                // ��� ��� ���ϱ�()
                Vector3 n_comma = hit.normal; // normal�� xz���� �����Ŵ
                n_comma.y = 0;
                n_comma.Normalize();
                float relateSlope = -Vector3.Dot(n_comma, _moveVelocity.normalized);  // ���� �̵��Ϸ��� ���������� ���

                _moveVelocity.y = slopePower * relateSlope;
            }
            _velocity = _moveVelocity + _gravityVelocity;
            _rigid.velocity = _velocity;
        }
    }

    public void Move(Vector3 movement)
    {
        _moveVelocity = movement;
    }

    public void SetGravity(Vector3 velocity)
    {
        _gravityVelocity = velocity;
    }

    public void AddGravity(Vector3 velocity)
    {
        _gravityVelocity += velocity;
    }

    public void Jump(float power)
    {
        SetGravity(Vector3.up * power);
        IsJump = true;
    }

    private void GravityUpdate()
    {
        if (!IsGround && UseGravity)    //  ������ ������ ������ �߷� �ΰ�
        {
            FallTime += Time.fixedDeltaTime;
            _gravityVelocity += Vector3.down * gravityCurve.Evaluate(FallTime) * Time.fixedDeltaTime;
            if (IsFalling)
                IsJump = false;
        }
        else if (!IsJump) // ������ ������ �߷� �ʱ�ȭ
        {
            FallTime = 0f;
            _gravityVelocity.y = Mathf.Max(_gravityVelocity.y, 0f);
        }
    }

    private bool CheckGround(out RaycastHit groundHit)
    {
        _ray.origin = rayPos.bottom.position;
        _ray.direction = Vector3.down;
        if (Physics.SphereCast(_ray, _collider.radius, out groundHit, float.PositiveInfinity, _groundLayer))
        {
            float nowSlopeAngle = Vector3.Dot(Vector3.up, groundHit.normal);

            GroundSlope = Mathf.Acos(nowSlopeAngle) * Mathf.Rad2Deg;    // ��� ���ϱ�
            GroundDist = groundHit.distance - _collider.height * 0.5f;  // �ٴڰ��� �Ÿ� ���ϱ�
            if(((GroundDist <= 0.2f && GroundSlope <= info.avilableSlope + 0.1f)||(IsGround == true && GroundDist <= 1f)))
            {
                IsGround = true;
                return true;
            }
            else
            {
                IsGround = false;
                return false;
            }
        }
        GroundSlope = float.NaN;
        IsGround = false;
        return false;
    }
}
