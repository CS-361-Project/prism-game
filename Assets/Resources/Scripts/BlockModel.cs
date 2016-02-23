using UnityEngine;
using System.Collections;

public class BlockModel : MonoBehaviour {
	protected SpriteRenderer rend;
	protected Sprite activeSprite;
	protected Sprite inactiveSprite;
	public Color baseColor;
	protected bool active;

	public virtual void init (Transform parent, Color baseColor) {
		transform.parent = parent;
		transform.localPosition = new Vector3(0, 0, 0);

		rend = GetComponent<SpriteRenderer>();
		this.baseColor = baseColor;
		rend.color = baseColor;
		rend.sortingLayerName = "Foreground";

		active = true;
		activeSprite = rend.sprite;
		inactiveSprite = Resources.Load<Sprite>("Sprites/Block_old");
	}

	public virtual void setActive(bool active) {
		this.active = active;
		if (active) {
			rend.sprite = activeSprite;
			rend.color = baseColor;
		}
		else {
			rend.sprite = inactiveSprite;
			if (baseColor == GameManager.CustomColors.White) {
				rend.color = baseColor - new Color(.2f, .2f, .2f, 0);
			}
			else {
				rend.color = baseColor + new Color(.3f, .3f, .3f);
			}
		}
	}

	public bool isActive() {
		return active;
	}

	public void setColor(Color c) {
		baseColor = c;
		setActive(active);
	}
}

