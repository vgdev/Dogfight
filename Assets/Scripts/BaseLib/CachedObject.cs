using System;
using UnityEngine;

namespace BaseLib
{
	public abstract class CachedObject : MonoBehaviour
	{
		private GameObject gameObj;
		private Transform trans;
		
		public Transform Transform
		{
			get
			{
				if(trans == null)
				{
					trans = transform;
				}
				return trans;
			}
		}
		
		public GameObject GameObject
		{
			get
			{
				if(gameObj == null)
				{
					gameObj = gameObject;
				}
				return gameObj;
			}
		}
		
		public virtual void Awake()
		{
			trans = transform;
			gameObj = gameObject;
		}
	}
}

