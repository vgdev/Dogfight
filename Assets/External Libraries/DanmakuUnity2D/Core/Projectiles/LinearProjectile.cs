using System;
using UnityEngine;
using UnityUtilLib;

public class LinearProjectile : IProjectile {
	public Material Material {
		get {
			throw new NotImplementedException ();
		}
		set {
			throw new NotImplementedException ();
		}
	}

	private bool active;
	private Matrix4x4 transform;
	private Matrix4x4 visualTransform;
	private Material material;
	private MaterialPropertyBlock drawData;
	private Vector2 position;
	private float rotationFloat;
	private Vector3 scale;
	private float velocity;
	private float acceleration;
	private float velocityCap;
	private Sprite sprite;
	private Color color;
	private Vector2 colliderOffset;
	private float colliderRaidus;

	#region IProjectile implementation
	public void SetAcceleration(float accelration, float velocityCap) {
	}
	public void Draw () {
		Util.DrawSpriteUnscaled (visualTransform, material, drawData);
	}
	public void Update (float dt) {
		throw new NotImplementedException ();
	}
	public Vector2 Position {
		get {
			return position;
		}
		set {
			position = value;
		}
	}
	public float Rotation {
		get {
			return rotationFloat;
		}
		set {
			rotationFloat = value;
			float rad = rotationFloat * Util.Degree2Rad;
		}
	}
	public Vector2 Scale {
		get {
			return scale;
		}
		set {
			scale = value;
		}
	}

	public float Velocity {
		get {
			return velocity;
		}
		set {
			velocity = value;
		}
	}

	public float Acceleration {
		get {
			return acceleration;
		}
	}

	public float VelocityCap {
		get {
			return velocityCap;
		}
	}

	public Sprite Sprite {
		get {
			return sprite;
		}
		set {
			sprite = value;
		}
	}

	public Color Color {
		get {
			return color;
		}
		set {
			color = value;
		}
	}

	public Vector2 ColliderOffset {
		get {
			return colliderOffset;
		}
		set {
			colliderOffset = value;
		}
	}

	public float ColliderRadius {
		get {
			return colliderRaidus;
		}
		set {
			colliderRaidus = value;
		}
	}
	#endregion
	#region IPooledObject implementation
	public void MatchPrefab (ProjectilePrefab prefab)
	{
		throw new NotImplementedException ();
	}
	public ProjectilePrefab Prefab {
		get {
			throw new NotImplementedException ();
		}
		set {
			throw new NotImplementedException ();
		}
	}
	#endregion
	#region IPooledObject implementation
	public void Activate ()
	{
		throw new NotImplementedException ();
	}
	public void Deactivate ()
	{
		throw new NotImplementedException ();
	}
	public IPool Pool { get; set; }
	public bool IsActive {
		get {
			throw new NotImplementedException ();
		}
	}
	#endregion
}

