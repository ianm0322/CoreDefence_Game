using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreScript : MonoBehaviour
{
    CD_GameObject _body;

    private void Awake()
    {
        TryGetComponent(out _body);

        _body.OnDiedEvent += Die;
    }

    public void Die()
    {
        GameManager.Instance.GameOver();
    }
}
