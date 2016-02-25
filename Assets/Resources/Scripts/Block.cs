using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour {
	protected BlockModel blockModel;
	protected Color baseColor;
	public virtual void init(Color c, Color bgColor, Board b, Transform parent) {
		transform.parent = parent;
		blockModel = Instantiate(Resources.Load<GameObject>("Prefabs/Block")).GetComponent<BlockModel>();
		baseColor = c;
		blockModel.init(transform, baseColor);
		name = "Block";
		onBackgroundChange(bgColor);
	}

	public virtual void onBackgroundChange(Color bgColor) {
		if (bgColor == baseColor) {
			blockModel.setActive(false);
		}
		else {
			blockModel.setActive(true);
		}
	}

	public virtual void onBGTransition(Color from, Color to, float progress) {
		if (to == baseColor) {
			if (progress >= 1) {
				blockModel.setActive(false);
			}
			else {
				blockModel.setTransitionColor(true, progress);
			}
		}
		else if (from == baseColor) {
			if (progress >= 1) {
				blockModel.setActive(true);
			}
			else {
				blockModel.setTransitionColor(false, progress);
			}
		}
	}

	public virtual bool isPassable() {
		return !blockModel.isActive();
	}

	public void setColor(Color c, Color bgColor) {
		baseColor = c;
		blockModel.setColor(c);
		onBackgroundChange(bgColor);
	}
}
