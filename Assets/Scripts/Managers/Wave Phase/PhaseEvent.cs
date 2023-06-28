[System.Serializable]
public abstract class PhaseEvent : System.IComparable<PhaseEvent>
{
    public int Id;
    public float StartTime;

    protected PhaseEvent(float startTime)
    {
        Id = SimpleID<PhaseEvent>.Get();
        StartTime = startTime;
    }

    public int CompareTo(PhaseEvent other)
    {
        if (StartTime > other.StartTime)
            return 1;
        else if (StartTime < other.StartTime)
            return -1;
        else
            return 0;
    }

    public abstract void Execute();
}