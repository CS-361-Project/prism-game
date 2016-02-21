using UnityEngine;
using System.Collections;

public class EmptyBlock : Block {
	public override void init(Color c, Color bgColor, Board parent) {
		transform.parent = parent.transform;
		name = "Empty Block";
	}

	public override bool isPassable() {
		return true;
	}

	public override void onBackgroundChange(Color bgColor) {
	}
}

