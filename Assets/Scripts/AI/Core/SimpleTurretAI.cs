using BT;
using static BT.NodeHelper;
using UnityEngine;

public class TestSetData : BTNode
{
    private string _key;
    private int _data;

    public TestSetData(string key, int data)
    {
        _key = key;
        _data = data;
    }

    protected override BTState OnUpdate()
    {
        Blackboard.SetData<int>(_key, _data);
        return BTState.Success;
    }
}

public class TestPrintData : BTNode
{
    private string _key;

    public TestPrintData(string key)
    {
        _key = key;
    }

    protected override BTState OnUpdate()
    {
        Debug.Log(Root.Blackboard.GetData<int>(_key));
        return BTState.Success;
    }
}

public class SimpleTurretAI : FacilityAI, IShooter, ITargetter
{
    public Transform GunTr;
    public Transform HeadTr;

    [field: SerializeField]
    public Transform Target { get; set; }
    public EntitySelector Scanner { get; set; }

    protected override void Awake()
    {
        base.Awake();
        Scanner = new EntitySelector(
            new SphereScanner(transform, AIInfo.DetectRange, AIInfo.DetectTargetLayer),
            new EntityClassifier_RayClassifier(transform, AIInfo.DetectTargetTags)
            );
    }

    public override RootNode MakeBT()
    {
        return Root(this, Sequence(
                new TestPrintData("Key")
                )
            );
        //return Root(
        //    Select(
        //        new IsParalysisNode(this),

        //        Sequence(   // Target is exist
        //            new IsTargetExistNode(this),
        //            new LookAtTargetNode(this, HeadTr, new Vector3(0, 0, 0), new Vector3(1, 0, 1)),

        //            Select(
        //                Sequence(   // Target out of range sequence
        //                    new CheckTargetOutOfRangeNode(this.transform, this, AIInfo),
        //                    new SetTargetNullNode(this)
        //                    ),

        //                Sequence(   // Attack sequence
        //                    new CountdownNode(FacilityInfo.AttackCount).SetOption(false),
        //                    new SimpleShootingNode(this),
        //                    new WaitForAttackSpeedNode(this.FacilityInfo)
        //                    ),

        //                new WaitForAttackDelayNode(this.FacilityInfo)
        //                )
        //            ),

        //        new SetTargetNode(this)
        //        )
        //    );
    }

    public void Shot()
    {
        EntityManager.Instance.CreateBullet(FacilityInfo.Bullet, GunTr);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        Blackboard.SetData<int>("Key", Mathf.FloorToInt(Time.time));
    }
}