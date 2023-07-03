using UnityEngine;

public interface ITargetter
{
    Transform Target { get; set; }
    EntitySelector Scanner { get; set; }
}
