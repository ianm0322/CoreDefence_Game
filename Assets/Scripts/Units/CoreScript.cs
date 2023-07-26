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

    private void Start()
    {
        StartCoroutine(CheckDieCoroutine());
    }

    private IEnumerator CheckDieCoroutine()
    {
        yield return null;
        WaitForSeconds wait = new WaitForSeconds(0.1f);
        while (!_body.IsDied)
        {
            yield return wait;
        }
        Die();
        StopAllCoroutines();
    }

    public void Die()
    {
        GameManager.Instance.GameOver();
    }
}
