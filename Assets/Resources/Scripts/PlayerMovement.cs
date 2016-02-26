using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {
	Board board;
	int x, y, oldX,oldY, moveDirX,moveDirY;
	float lastMovement = -1.0f;
	//animation variables
	public float size = 0.7f;
	public float blockSize = 1.0f;
	public float targetSquish = 0.5f;
	public float targetExpand = 0.9f;
	public float moveSquish = 0.6f;
	public float moveStretch = 1.15f;
	public bool moving = false;

	SpriteRenderer rend;
	Color baseColor = Color.white;
	Color greyColor = CustomColors.Grey;
	// Use this for initialization
	public void init(Board b) {
		transform.parent = b.transform;
		rend = GetComponent<SpriteRenderer>();
		rend.color = baseColor;
		board = b;
		x = 0;
		y = b.getHeight() - 1;
		transform.position = board.getBlockPosition(x, y);
		//updatePosition();
	}

	public bool move(Vector2 direction) {
		if (direction == Vector2.zero) {
			return false;
		}
		bool moved;
		moving = true;
		int dx = (int)direction.x;
		int dy = (int)direction.y;
		moveDirX = dx;
		moveDirY = dy;
		oldX = x;
		oldY = y;
		if ((moved = board.getBlockPassable(x + dx, y + dy))) {
			x = x + dx;
			y = y + dy;
			updatePosition();
		}
		else {
			lastMovement = Time.time;
		}
		return moved;
	}

	public void onBackgroundTransition(Color oldBG, Color newBG, float progress) {
		if (newBG == CustomColors.White) {
			rend.color = Color.Lerp(baseColor, greyColor, progress);
		}
		else if (oldBG == CustomColors.White) {
			rend.color = Color.Lerp(greyColor, baseColor, progress);
		}
	}

	public void whileMoving(float percentDone){
		if (percentDone >= 1.0f) {
			percentDone = 1.0f;
			moving = false;
		}
		Vector3 target = board.getBlockPosition(x, y);
		Vector3 old = board.getBlockPosition(oldX, oldY);
		if (oldX == x && oldY == y) {
			if (moveDirX != 0) {
				target = new Vector3 (old.x + moveDirX * (blockSize - targetSquish) / 2.0f, old.y, 0);
				transform.position = Vector3.Lerp (old, target, Mathf.Sin (Mathf.PI * percentDone));
				transform.localScale = new Vector3 (
					Mathf.Sin (Mathf.PI * percentDone) * (targetSquish - size) + size,
					Mathf.Sin (Mathf.PI * percentDone) * (targetExpand - size) + size, 0);
			} 
			else if (moveDirY != 0) {
				target = new Vector3 (old.x, old.y + moveDirY * (blockSize - targetSquish) / 2.0f, 0);
				transform.position = Vector3.Lerp (old, target, Mathf.Sin (Mathf.PI * percentDone));
				transform.localScale = new Vector3 (
					Mathf.Sin (Mathf.PI * percentDone) * (targetExpand - size) + size,
					Mathf.Sin (Mathf.PI * percentDone) * (targetSquish - size) + size, 0);
			} 
		} else {
			transform.position = Vector3.Lerp (old, target, percentDone);
			if (oldX != x) {
				transform.localScale = new Vector3((moveStretch - size) * Mathf.Sin (Mathf.PI * percentDone) + size,
					(moveSquish - size) * Mathf.Sin (Mathf.PI * percentDone) + size, 0);
			}
			if (oldY != y) {
				transform.localScale = new Vector3((moveSquish - size) * Mathf.Sin (Mathf.PI * percentDone) + size,
					(moveStretch - size) * Mathf.Sin (Mathf.PI * percentDone) + size,0);
			}
		}
	}

	public void updatePosition() {
		//transform.position = board.getBlockPosition(x, y);
		if (board.getBlock(x, y).name == "Lever") {
			LeverBlock lever = (LeverBlock)board.getBlock(x, y);
			lever.toggle();
		}
		lastMovement = Time.time;
	}

	public void finishMovementImmedate() {
		whileMoving(1.0f);
	}

	public bool finishedMovement() {
		return !moving;
	}

	public float timeSinceLastMovement() {
		return Time.time - lastMovement;
	}
}
