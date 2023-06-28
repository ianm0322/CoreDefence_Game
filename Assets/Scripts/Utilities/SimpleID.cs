using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleID<T>
{
    private static int _currentId;
    public static int Get()
    {
        return _currentId++;
    }
}
