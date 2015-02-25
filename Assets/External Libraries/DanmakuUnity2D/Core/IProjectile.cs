using System;
using UnityEngine;
using UnityUtilLib;

public interface IProjectile : IPrefabPooledObject<ProjectilePrefab> {

	Vector2 Position { get; set; }
	float Rotation { get; set; }
	Vector2 Scale { get; set; }

	float Velocity { get; set; }
	float Acceleration { get; }
	float VelocityCap { get; }

	Sprite Sprite { get; set; }
	Color Color { get; set; }
	Material Material { get; set; }

	Vector2 ColliderOffset { get; set; }
	float ColliderRadius { get; set; }

	void Draw();
	void Update(float dt);

	void SetAcceleration(float acceleration, float velocityCap);
}

