using UnityEngine;
using System.Collections;

public class BlockModel : MonoBehaviour {
	protected SpriteRenderer rend;
	public Sprite activeSprite;
	public Sprite inactiveSprite;
	public Color baseColor;
	public Color inactiveColor;
	protected bool active;

	public virtual void init (Transform parent, Color baseColor) {
		transform.parent = parent;
		transform.localPosition = new Vector3(0, 0, 0);

		rend = GetComponent<SpriteRenderer>();
		this.baseColor = baseColor;
		inactiveColor = baseColor - new Color(.1f, .1f, .1f, 0.3f);
		rend.color = baseColor;
		rend.sortingLayerName = "Foreground";

		setActive(true);
	}

	public virtual void setActive(bool active) {
		this.active = active;
		if (active) {
			rend.sprite = activeSprite;
			rend.color = baseColor;
		}
		else {
			rend.sprite = inactiveSprite;
			rend.color = inactiveColor;
		}
	}

	public bool isActive() {
		return active;
	}

	public void setColor(Color c) {
		baseColor = c;
		setActive(active);
	}

	public void setTransitionColor(bool prevState, float t) {
		if (prevState) {
			rend.color = Color.Lerp(baseColor, inactiveColor, t);
		}
		else {
			rend.color = Color.Lerp(inactiveColor, baseColor, t);
		}
	}
}

