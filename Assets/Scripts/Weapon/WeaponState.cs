[System.Flags]
[System.Serializable]
public enum WeaponState
{
    None                    = 0,
    Default                 = 1,
    Firing                  = 2,
    Reloading               = 4,
    Jammed                  = 8,
    NoAmmo                  = 16,

    Everything = -1,
    CanFireState = Firing | Default,
    CantFireState = Reloading | Jammed | NoAmmo,
}