using UnityEngine;
using System.Collections;
using UnityUtilLib;

public class TestDrawSprite : TestScript {

	public Sprite sprite;
	public Vector2 position;
	public Vector2 scale;
	public float rotation;
	public Color color = Color.white;
	public Material material;
	private MaterialPropertyBlock mpb;

	void Start() {
		mpb = Util.SpriteToMPB(sprite, color);
	}

	void Update() {
		for(int i = 0; i < 10; i++) {
			Matrix4x4 transform = Matrix4x4.TRS(position + Vector2.right * i, Quaternion.Euler(0f, 0f, rotation), Util.SpriteScale(sprite, scale));
			Util.DrawSprite(sprite, position , Quaternion.Euler(0f, 0f, rotation), scale, color, material, mpb);
		}
	}
}
