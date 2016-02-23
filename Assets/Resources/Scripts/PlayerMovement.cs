using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {
	Board board;
	int x, y;
	float lastMovement = -1.0f;
	// Use this for initialization
	public void init(Board b) {
		transform.parent = b.transform;
		GetComponent<SpriteRenderer>().color = GameManager.CustomColors.Brown;
		board = b;
		x = 0;
		y = b.getHeight() - 1;
		updatePosition();
	}

	public void move(Vector2 direction) {
		bool moved;
		int dx = (int)direction.x;
		int dy = (int)direction.y;
		if ((moved = board.getBlockPassable(x + dx, y + dy))) {
			x = x + dx;
			y = y + dy;
			updatePosition();
		}
		else {
			lastMovement = Time.time;
		}
	}

	public void updatePosition() {
		transform.position = board.getBlockPosition(x, y);
		if (board.getBlock(x, y).name == "Lever") {
			LeverBlock lever = (LeverBlock)board.getBlock(x, y);
			lever.toggle();
		}
		lastMovement = Time.time;
	}

	public bool readyToMove() {
		return (Time.time - lastMovement) >= 0.15f;
	}
}
