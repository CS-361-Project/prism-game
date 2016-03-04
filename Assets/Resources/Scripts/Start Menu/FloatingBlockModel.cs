using UnityEngine;
using System.Collections;

public class FloatingBlockModel : MonoBehaviour {
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
		inactiveColor = baseColor - new Color(.1f, .1f, .1f, 0.36f);
		rend.color = new Color(baseColor.r,baseColor.b,baseColor.g, .75F);
		rend.sortingLayerName = "Foreground";
	}
}
