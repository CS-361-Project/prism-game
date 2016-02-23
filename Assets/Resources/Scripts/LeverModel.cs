using UnityEngine;
using System.Collections;

public class LeverModel : BlockModel {
	public override void init (Transform parent, Color baseColor) {
		transform.parent = parent;
		transform.localPosition = new Vector3(0, 0, 0);

		rend = GetComponent<SpriteRenderer>();
		this.baseColor = baseColor;
		rend.color = baseColor;
		rend.sortingLayerName = "Foreground";

		active = false;
		inactiveSprite = rend.sprite;
		activeSprite = Resources.Load<Sprite>("Sprites/Lever_Right");
	}

	public override void setActive(bool active) {
		this.active = active;
		if (active) {
			rend.sprite = activeSprite;
		}
		else {
			rend.sprite = inactiveSprite;
		}
	}

	public Color getColor() {
		return rend.color;
	}
}

