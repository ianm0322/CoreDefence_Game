using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField]
    private Transform _follow;
    [SerializeField]
    private Vector3 _offset;
    private Vector3 _lookdir;
    [SerializeField]
    private float _camMinYLimit = -80f;
    [SerializeField]
    private float _camMaxYLimit = 80f;

    public void Init()
    {
        _lookdir = this.transform.eulerAngles;
    }

    void Update()
    {
        this.transform.eulerAngles = GameManager.Instance.player._playerMovement.LookDirection;
        this.transform.position = _follow.position + _offset;
    }

    // Test
    private void Start()
    {
        Init();
    }
}
