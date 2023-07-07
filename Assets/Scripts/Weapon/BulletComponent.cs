using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletComponent : MonoBehaviour
{
    public BulletMovement movementModule;
    public BulletEffect effectModule;

    private void FixedUpdate()
    {
        movementModule.OnUpdate();
        if (movementModule.IsHit())
        {
            effectModule.OnHit();
        }
    }
}

public abstract class BulletMovement
{
    public abstract void OnUpdate();
    public abstract bool IsHit();
}

public abstract class BulletEffect
{
    public abstract void OnHit();
}