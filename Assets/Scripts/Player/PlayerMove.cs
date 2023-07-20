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
        // accel만큼 가속.
        // 최종적으로 movement만큼 속도 제한.
        Vector3 moveDir = movement.normalized;
        Vector3 velocity = _rigid.velocity;

        velocity.y = 0;

        // 가속
        if(velocity.sqrMagnitude < movement.sqrMagnitude)
        {
            _rigid.AddForce(moveDir * _accel, ForceMode.Acceleration);
        }
    }
}
