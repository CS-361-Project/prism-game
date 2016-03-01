using UnityEngine;
using System.Collections;

public class TraversalAI : MonoBehaviour {
	Board board;
	int x, y, oldX, oldY, moveDirX, moveDirY;
	Vector2 direction;

	//animation variables
	public float size = 0.7f;
	public float blockSize = 1.0f;
	public float targetSquish = 0.5f;
	public float targetExpand = 0.9f;
	public float moveSquish = 0.6f;
	public float moveStretch = 1.15f;
	public bool moving = false;


	// Use this for initialization
	public void init(Board B) {
		GetComponent<SpriteRenderer>().color = CustomColors.Traversal_AI;
		board = B;
		x = 1;
		y = 0;
		transform.position = board.getBlockPosition(x, y);
		updatePosition();

		direction = Vector2.left;

		//create a model object that deals with the graphics and call its init

	}

	void changeDirection() {
		direction = (direction == Vector2.left) ? Vector2.right : Vector2.left;
	}

	public void move() {
		moving = true;
		int dx = (int)direction.x;
		int dy = (int)direction.y;
		moveDirX = dx;
		moveDirY = dy;
		oldX = x;
		oldY = y;
		if (board.getBlockPassableAfterTransition(x + dx, y + dy)) {
			x = x + dx;
			y = y + dy;
			updatePosition();
		}
		else {
			//go in other direction
			print("Block " + (x+dx) + ", " + (y+dy) + " is not passable after transition");
			changeDirection();
			dx = (int)direction.x;
			dy = (int)direction.y;
			moveDirX = dx;
			moveDirY = dy;
			oldX = x;
			oldY = y;
			if ((board.getBlockPassable(x + dx, y + dy))) {
				x = x + dx;
				y = y + dy;
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

	public void updatePosition() {
		//places player in center of new block
		//transform.position = board.getBlockPosition(x, y);

		//tell block AI is on block
		board.getBlock(oldX, oldY).setHasEnemy(false);
		board.getBlock(x, y).setHasEnemy(true);

	}


	public void finishMovementImmedate() {
		whileMoving(1.0f);
	}

	public bool finishedMovement() {
		return !moving;
	}


}
