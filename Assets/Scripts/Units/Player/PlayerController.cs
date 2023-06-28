using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : CD_GameObject
{
    public bool IsActive { get; private set; } = false; // 코드를 작동시키는지에 대한 변수

    // Data
    public float moveSpeed;
    public float jumpPower;

    // Component
    public PlayerBody _playerBody;
    public Rigidbody _rigid;
    public CapsuleCollider _collider;
    [SerializeField]
    public WeaponBase _weapon;

    [Header("Attack")]
    [SerializeField]
    private BulletData bulletData;
    [SerializeField]
    private Transform gunTr;
    [SerializeField]
    private float fireDelay;
    private float fireEndTime;
    private Coroutine fireCoroutine;
    private WaitForSeconds fireDelayYield;
    public bool IsShooting { get; private set; }

    // Temp
    public Vector3 lookDir;

    private void Awake()
    {
        TryGetComponent(out _playerBody);
        TryGetComponent(out _collider);

        // ######### Test
        TryGetComponent(out _rigid);
        fireDelayYield = new WaitForSeconds(fireDelay);
    }

    public void Init()
    {
        IsActive = true;
    }

    private void Update()
    {
        MoveUpdate();
        JumpUpdate();
        ShootingUpdate();

        if (_weapon)
        {
            _weapon.transform.parent = gunTr;
            _weapon.transform.localPosition = Vector3.zero;
        }
    }

    private void FixedUpdate()
    {

    }

    protected void MoveUpdate()
    {
        // ###### Test: Input은 나중에 따로 분리하기
        float mx = Input.GetAxisRaw("Mouse X") * StaticDataManager.Instance.MouseSensitive;
        float my = Input.GetAxisRaw("Mouse Y") * StaticDataManager.Instance.MouseSensitive;
        lookDir.Set(-my, mx, 0f);
        _playerBody.LookDirection += lookDir;

        float h, v;
        h = Input.GetAxisRaw("Horizontal");     // 인풋
        v = Input.GetAxisRaw("Vertical");

        Vector3 dir = this.transform.forward * v + this.transform.right * h;
        if (h != 0f && v != 0f)
            dir /= 1.4f;

        _playerBody.Move(dir * moveSpeed);
    }

    protected void JumpUpdate()
    {
        if (Input.GetButtonDown("Jump") && _playerBody.IsGround)
        {
            // Jump script
            _playerBody.Jump(jumpPower);
        }
        else if (!_playerBody.IsGround)
        {
            // Fall script
        }
    }

    private void ShootingUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ViewportPointToRay(Vector3.one * 0.5f), out hit, float.PositiveInfinity))
        {
            gunTr.LookAt(hit.point);
        }
        else
        {
            gunTr.rotation = Quaternion.Euler(_playerBody.LookDirection);
        }

        if (Input.GetButtonDown("Fire1"))
        {
            _weapon?.SetTrigger(true);
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            _weapon?.SetTrigger(false);
        }
        if (Input.GetKeyDown(KeyCode.R))
            Reload();
    }

    public void Fire()
    {
        if (_weapon != null)
        {
            _weapon.Fire();
            //DataManager.Instance.CreateBullet(bulletData, gunTr);
        }
    }

    public void Reload()
    {
        if (_weapon != null)
        {
            _weapon.Reload();
        }
    }

    private IEnumerator WhileFireCoroutine()
    {
        yield return null;
        while (true)
        {
            Fire();

            yield return new WaitForSeconds(fireDelay);
        }
    }

    public override void Die()
    {
        Debug.Log("플레이어 이즈 다이!");
    }
}
