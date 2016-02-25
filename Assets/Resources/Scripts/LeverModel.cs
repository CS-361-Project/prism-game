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

		setActive(false);
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

