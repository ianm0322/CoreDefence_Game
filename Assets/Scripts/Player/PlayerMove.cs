using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody _rigid;

    [SerializeField]
    private float _accel;

    public void Move(Vector3 movement)
    {
        // accel��ŭ ����.
        // ���������� movement��ŭ �ӵ� ����.
        Vector3 moveDir = movement.normalized;
        Vector3 velocity = _rigid.velocity;

        velocity.y = 0;

        // ����
        if(velocity.sqrMagnitude < movement.sqrMagnitude)
        {
            _rigid.AddForce(moveDir * _accel, ForceMode.Acceleration);
        }
    }
}
