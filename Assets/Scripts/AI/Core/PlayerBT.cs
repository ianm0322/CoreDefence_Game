using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BT;

public class PlayerBT : IBehaviorTree
{
    RootNode _root;
    GameObject _player;

    CD_GameObject _body;

    public PlayerBT(GameObject player)
    {
        this._player = player;

        player.TryGetComponent(out _body);
    }

    public RootNode MakeBT()
    {
        return new RootNode(
            new SelectorNode(
                new IsDiedNode(_body)//,

                //new PlayerMoveNode(),

                /*
                 * Selector(
                 *      IsGround
                 *      Player Jump
                 * )
                 */

                )
            );
    }

    public void Operate()
    {
        _root.Evaluate();
    }
}
