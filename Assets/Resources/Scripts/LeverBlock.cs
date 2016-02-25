using UnityEngine;
using System.Collections;

public class LeverBlock : Block {
	public Color leverColor;
	public bool state;
	Board board;

	public override void init(Color c, Color bgColor, Board parent) {
		blockModel = Instantiate(Resources.Load<GameObject>("Prefabs/Lever")).GetComponent<LeverModel>();
		blockModel.init(this.transform, c);
		transform.parent = parent.transform;
		name = "Lever";
		board = parent;
		leverColor = c;
		state = false;
	}

	public override bool isPassable() {
		return true;
	}

	public override void onBackgroundChange(Color bgColor) {
		state = CustomColors.contains(bgColor, leverColor);
		blockModel.setActive(state);
	}

	public void toggle() {
		Color c = board.getBackgroundColor();
		print("Toggling lever from state: " + state);
		if (state) {
			board.setBackground(CustomColors.subColor(c, leverColor));
		}
		else {
			board.setBackground(CustomColors.addColor(c, leverColor));
		}
	}

	public bool getState() {
		return state;
	}
}