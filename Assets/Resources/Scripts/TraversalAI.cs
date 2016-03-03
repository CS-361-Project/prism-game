using UnityEngine;
using System.Collections;

public class TraversalAI : MonoBehaviour {
	Board board;
	int x, y, oldX, oldY, moveDirX, moveDirY;

	//animation variables
	public float size = 0.7f;
	public float blockSize = 1.0f;
	public float targetSquish = 0.5f;
	public float targetExpand = 0.9f;
	public float moveSquish = 0.6f;
	public float moveStretch = 1.15f;
	public bool moving = false;
	public bool markedForDeath = false;


	// Use this for initialization
	public void init(Board B, int xPos, int yPos, int xDirection, int yDirection) {
		GetComponent<SpriteRenderer>().color = CustomColors.TraversalAI;
		board = B;
		x = xPos;
		y = yPos;
		transform.position = board.getBlockPosition(x, y);
		transform.localScale = new Vector3(size, size, 1);
		moveDirX = xDirection;
		moveDirY = yDirection;
		updatePosition();
	}

	void changeDirection() {
		moveDirX *= -1;
		moveDirY *= -1;
	}

	public void move() {
		moving = true;
		oldX = x;
		oldY = y;
		if (board.getBlockPassable(x, y) && !board.getBlockPassableAfterTransition(x, y)) {
			markedForDeath = true;
		}
		if (board.getBlockPassableAfterTransition(x + moveDirX, y + moveDirY)) {
			x = x + moveDirX;
			y = y + moveDirY;
			updatePosition();
		}
		else {
			//go in other direction
			changeDirection();
			if ((board.getBlockPassableAfterTransition(x + moveDirX, y + moveDirY))) {
				x = x + moveDirX;
				y = y + moveDirY;
				updatePosition();
			}
		}
	}

	public void whileMoving(float percentDone) {
		if (percentDone >= 1.0f) {
			percentDone = 1.0f;
			moving = false;
		}
		Vector3 target = board.getBlockPosition(x, y);
		Vector3 old = board.getBlockPosition(oldX, oldY);
		if (oldX == x && oldY == y) {
			if (moveDirX != 0) {
				target = new Vector3(old.x + moveDirX * (blockSize - targetSquish) / 2.0f, old.y, 0);
				transform.position = Vector3.Lerp(old, target, Mathf.Sin(Mathf.PI * percentDone));
				transform.localScale = new Vector3(
					Mathf.Sin(Mathf.PI * percentDone) * (targetSquish - size) + size,
					Mathf.Sin(Mathf.PI * percentDone) * (targetExpand - size) + size, 0);
			}
			else if (moveDirY != 0) {
				target = new Vector3(old.x, old.y + moveDirY * (blockSize - targetSquish) / 2.0f, 0);
				transform.position = Vector3.Lerp(old, target, Mathf.Sin(Mathf.PI * percentDone));
				transform.localScale = new Vector3(
					Mathf.Sin(Mathf.PI * percentDone) * (targetExpand - size) + size,
					Mathf.Sin(Mathf.PI * percentDone) * (targetSquish - size) + size, 0);
			} 
		}
		else {
			transform.position = Vector3.Lerp(old, target, percentDone);
			if (oldX != x) {
				transform.localScale = new Vector3((moveStretch - size) * Mathf.Sin(Mathf.PI * percentDone) + size,
					(moveSquish - size) * Mathf.Sin(Mathf.PI * percentDone) + size, 0);
			}
			if (oldY != y) {
				transform.localScale = new Vector3((moveSquish - size) * Mathf.Sin(Mathf.PI * percentDone) + size,
					(moveStretch - size) * Mathf.Sin(Mathf.PI * percentDone) + size, 0);
			}
		}
	}

	public void onKill() {
		board.setHasEnemy(x, y, false);
	}

	public void updatePosition() {
		//places player in center of new block
		//transform.position = board.getBlockPosition(x, y);

		//tell block AI is on block
		board.setHasEnemy(oldX, oldY, false);
		board.setHasEnemy(x, y, true);

	}


	public void finishMovementImmedate() {
		whileMoving(1.0f);
	}

	public bool finishedMovement() {
		return !moving;
	}


}
