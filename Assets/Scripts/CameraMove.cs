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
        if (StageManager.Instance.Player != null)
        {
            this.transform.eulerAngles = StageManager.Instance.Player.PlayerMovement.LookDirection;
            this.transform.position = _follow.position + _offset;
        }
    }

    // Test
    private void Start()
    {
        Init();
    }
}
