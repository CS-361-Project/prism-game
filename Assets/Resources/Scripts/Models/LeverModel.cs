using UnityEngine;
using System.Collections;

public class LeverModel : BlockModel {
	SpriteRenderer outline;
	Sprite activeOutline, inactiveOutline;
	public override void init (Transform parent, Color baseColor) {
		transform.parent = parent;
		transform.localPosition = new Vector3(0, 0, 0);

		rend = GetComponent<SpriteRenderer>();
		this.baseColor = baseColor;
		rend.color = baseColor;
		rend.sortingLayerName = "Characters";

		GameObject obj = new GameObject();
		obj.name = "Lever Outline";
		obj.transform.parent = parent;
		obj.transform.localPosition = new Vector3(0, 0, 0);
		obj.transform.localScale = transform.localScale;
		outline = obj.AddComponent<SpriteRenderer>();
		outline.sortingLayerName = "Characters";
		outline.sortingOrder = 2;
		outline.color = new Color(1, 1, 1);
		activeOutline = Resources.Load<Sprite>("Sprites/Switch-On-Outline");
		inactiveOutline = Resources.Load<Sprite>("Sprites/Switch-Off-Outline");

		setActive(false);
	}

	public override void setActive(bool active) {
		this.active = active;
		if (active) {
			rend.sprite = activeSprite;
			outline.sprite = activeOutline;
		}
		else {
			rend.sprite = inactiveSprite;
			outline.sprite = inactiveOutline;
		}
	}

	public Color getColor() {
		return rend.color;
	}
}

