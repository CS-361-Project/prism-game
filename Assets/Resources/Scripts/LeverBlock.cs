using UnityEngine;
using System.Collections;

public class LeverBlock : Block {
	public Color leverColor;
	public bool state;
	Board board;

	public override void init(Color c, Color bgColor, Board b, Transform parent) {
		blockModel = Instantiate(Resources.Load<GameObject>("Prefabs/Lever")).GetComponent<LeverModel>();
		blockModel.init(this.transform, c);
		transform.parent = parent;
		name = "Lever";
		board = b;
		leverColor = c;
		state = false;
	}

	public override bool isPassable() {
		return true;
	}

	public override bool passableWithBG(Color bgColor) {
		return true;
	}

	public override void onBackgroundChange(Color bgColor) {
		state = CustomColors.contains(bgColor, leverColor);
		blockModel.setActive(state);
	}

	public void toggle() {
		Color c = board.getBackgroundColor();
		if (state) {
			board.startBGTransition(CustomColors.subColor(c, leverColor));
		}
		else {
			board.startBGTransition(CustomColors.addColor(c, leverColor));
		}
	}

	public bool getState() {
		return state;
	}
}