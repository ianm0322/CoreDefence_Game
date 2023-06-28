using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class EntityData
    {
        public int Hp;
        public int Def;
        public int Atk;
        public float MoveSpeed;
        public float AttackSpeed;
    }

    public class WeaponData
    {
		public int					Atk;
		public float				AttackSpeed;
		public float				ReloadSpeed;
		public float				Range;
		public int					MaxAmmo;
    }

	public class BulletData
    {
		public int					Damage;
		public float				Speed;
		public float				MaxDist;
		public float				LifeTime;
		public int					PenetrationCount;
    }
}