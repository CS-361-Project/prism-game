using UnityEngine;
using System.Collections;

public class LeverBlock : Block {
	Color leverColor;
	bool state;
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
		state = GameManager.CustomColors.contains(bgColor, leverColor);
		blockModel.setActive(state);
	}

	public void toggle() {
		Color c = board.getBackgroundColor();
		if (state) {
			board.setBackground(GameManager.CustomColors.subColor(c, leverColor));
			print("Subtracted " + leverColor + " from " + c);
			state = false;
		}
		else {
			board.setBackground(GameManager.CustomColors.addColor(c, leverColor));
			print("Added " + leverColor + " to " + c);
			state = true;
		}
	}

	public bool getState() {
		return state;
	}
}