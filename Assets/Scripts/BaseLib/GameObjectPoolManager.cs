using UnityEngine;
using System;
using System.Collections.Generic;

namespace BaseLib
{
	public class GameObjectPoolManager : CachedObject
	{
		public GameObjectPool<T> RegisterObjectPool<T>(string name, GameObject prefab, int initialSpawn = 50, int spawnOnEmpty = 10)
			where T : PooledScript
		{
			GameObject container = new GameObject (name);
			container.transform.parent = Transform;

		}
	}


	public class GameObjectPool<T> where T : PooledScript
	{
		private Queue<T> inactive;

		public GameObjectPool(GameObject prefab, int initialSpawn, int spawnOnEmpty)
		{

		}

		public void Return(T o)
		{

		}

		public T Get(GameObject prefab = null)
		{

		}
	}

	[RequireComponent(typeof(BoxCollider2D))]
	[RequireComponent(typeof(CircleCollider2D))]
	[RequireComponent(typeof(SpriteRenderer))]
	public abstract class PooledScript : CachedObject
	{
		public IPool objectPool;
		public GameObject prefab;
		private BoxCollider2D boxCollider;
		private CircleCollider2D circleCollider;
		private SpriteRenderer spriteRenderer;

		public override void Awake()
		{
			base.Awake ();
			boxCollider = GetComponent<BoxCollider2D> ();
			circleCollider = GetComponent<CircleCollider2D> ();
			spriteRenderer = GetComponent<SpriteRenderer> ();
		}

		public virtual void MatchPrefab(GameObject gameObj)
		{
			if(prefab != gameObj)
			{
				BoxCollider2D boxTest = gameObj.GetComponent<BoxCollider2D>();
				CircleCollider2D circleTest = gameObj.GetComponent<CircleCollider2D>();
				SpriteRenderer spriteTest = gameObj.GetComponent<SpriteRenderer>();

				spriteRenderer.sprite = spriteTest.sprite;
				spriteRenderer.color = spriteTest.color;

				if(boxTest != null)
				{
					boxCollider.center = boxTest.center;
					boxCollider.size = boxTest.size;
				}

				if(circleTest != null)
				{
					circleCollider.center = circleTest.center;
					circleCollider.radius = circleTest.radius;
				}

				boxCollider.enabled = boxTest != null;
				circleCollider.enabled = circleTest != null;
			}
		}

		public virtual void Activate()
		{
			GameObject.SetActive (true);
		}

		public virtual void Deactivate()
		{
			GameObject.SetActive (false);
			objectPool.Return (this);
		}
	}

	public interface IPool
	{
		object Get(GameObject prefab = null);

		void Return(object o);
	}
}