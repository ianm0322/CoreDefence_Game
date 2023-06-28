using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BT
{
    public class BTContext
    {
        public GameObject       gameObject;
        public Transform        transform;
        public Rigidbody        rigidbody;
        public Collider         collider;

        public BTContext(GameObject obj)
        {
            SetContext(obj);
        }

        public virtual void SetContext(GameObject gameObject)
        {
            this.gameObject = gameObject;
            this.transform = gameObject.transform;
            gameObject.TryGetComponent(out rigidbody);
            gameObject.TryGetComponent(out collider);
        }
    }

    public class EnemyAIContext : BTContext
    {
        public EnemyAI ai;
        public NavMeshAgent agent;
        public CapsuleCollider capsuleCollider;

        public EnemyAIContext(GameObject obj) : base(obj)
        {
        }

        public override void SetContext(GameObject gameObject)
        {
            base.SetContext(gameObject);
            gameObject.TryGetComponent(out agent);
            gameObject.TryGetComponent(out ai);
            gameObject.TryGetComponent(out capsuleCollider);
        }
    }
}
