using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObserver
{
    void DetectNotify(ObserverEvent info);
}

public interface ISubject
{
    void AttachObserver(IObserver observer);
    void DetachObserver(IObserver observer);
    void Notify(ObserverEvent info);
}


public abstract class ObserverEvent
{
    public object Invoker;
    public float InvokeTime;

    public ObserverEvent(object invoker)
    {
        Invoker = invoker;
        InvokeTime = Time.time;
    }
}
