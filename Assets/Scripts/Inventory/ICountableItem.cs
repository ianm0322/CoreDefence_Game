using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICountableItem : IItem
{
    int Count { get; set; }
}