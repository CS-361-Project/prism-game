using UnityEngine;
using System.Collections;

public class EmptyBlock : Block {
	public override void init(Color c, Color bgColor, Board b, Transform parent) {
		transform.parent = parent;
		name = "Empty Block";
	}

	public override bool isPassable() {
		return true;
	}

	public override void onBackgroundChange(Color bgColor) {
		//blockModel.setColor(bgColor);
	}

	public override void onBGTransition(Color a, Color b, float t) {
	}

	public override bool passableWithBG(Color bgColor) {
		return true;
	}
}

