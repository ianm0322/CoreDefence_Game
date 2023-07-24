using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool IsActive { get; private set; } = false; // 코드를 작동시키는지에 대한 변수

    // Data
    public float moveSpeed;
    public float jumpPower;

    // Component
    [HideInInspector]
    public PlayerBody PlayerMovement;
    [HideInInspector]
    public Rigidbody Rigid;
    [HideInInspector]
    public CapsuleCollider Collider;
    [HideInInspector]
    public CD_GameObject Body;

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
        TryGetComponent(out PlayerMovement);
        TryGetComponent(out Collider);
        TryGetComponent(out Body);

        // ######### Test
        TryGetComponent(out Rigid);
        fireDelayYield = new WaitForSeconds(fireDelay);
    }

    private void Start()
    {
        Body.OnDiedEvent += Spawn;
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
    }

    protected void MoveUpdate()
    {
        // ###### Test: Input은 나중에 따로 분리하기
        float mx = Input.GetAxisRaw("Mouse X") * StaticDataManager.Instance.MouseSensitive;
        float my = Input.GetAxisRaw("Mouse Y") * StaticDataManager.Instance.MouseSensitive;
        lookDir.Set(-my, mx, 0f);
        PlayerMovement.LookDirection += lookDir;

        float h, v;
        h = Input.GetAxisRaw("Horizontal");     // 인풋
        v = Input.GetAxisRaw("Vertical");

        Vector3 dir = this.transform.forward * v + this.transform.right * h;
        if (h != 0f && v != 0f)
            dir /= 1.4f;

        PlayerMovement.Move(dir * moveSpeed);
    }

    protected void JumpUpdate()
    {
        if (Input.GetButtonDown("Jump") && PlayerMovement.IsGround)
        {
            // Jump script
            PlayerMovement.Jump(jumpPower);
        }
        else if (!PlayerMovement.IsGround)
        {
            // Fall script
        }
    }

    private void ShootingUpdate()
    {
        gunTr.LookAt(Camera.main.transform.position + Camera.main.transform.forward * 1000f);

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

    public void Spawn()
    {
        this.transform.position = StageManager.Instance.PlayerSpawnPoint.position;
        PlayerMovement.LookDirection = StageManager.Instance.PlayerSpawnPoint.eulerAngles;
        Body.SetHp(Body.MaxHp);
    }

    public void SetWeapon(WeaponBase weapon, bool isEquipNow = true)
    {
        if (this._weapon != null)
        {
            this._weapon.gameObject.SetActive(false);
        }

        if (weapon != null)
        {
            weapon.transform.parent = gunTr;
            weapon.transform.localPosition = weapon.equipPositionOffset;
            weapon.transform.localRotation = Quaternion.Euler(weapon.equipRotationOffset);

            if (isEquipNow)
            {
            this._weapon = weapon;
            weapon.gameObject.SetActive(true);
            }
        }
    }
}
