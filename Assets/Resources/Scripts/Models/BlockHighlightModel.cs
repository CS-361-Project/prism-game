using UnityEngine;
using System.Collections;

public class BlockHighlightModel : MonoBehaviour {
	Block block;
	float age = 0.0f;
	float maxAge = 5.0f;
	SpriteRenderer rend;
	Color colorA = CustomColors.White;
	Color colorB = CustomColors.Yellow;

	public void init(Block parent, float lifespan) {
		block = parent;
		transform.parent = parent.transform;
		transform.localPosition = new Vector3(0, 0, 0);
		transform.localScale = new Vector3(1, 1, 1);
		maxAge = lifespan;
		rend = gameObject.AddComponent<SpriteRenderer>();
		rend.sprite = Resources.Load<Sprite>("Sprites/BlockHighlight");
		rend.sortingLayerName = "Foreground";
	}

	void Start() {

	}

	void Update() {
		age += Time.deltaTime;
		if (age >= maxAge) {
			Destroy(this.gameObject);
		}
		rend.color = Color.Lerp(colorA, colorB, Mathf.Sin(age * Mathf.PI));
	}
}

