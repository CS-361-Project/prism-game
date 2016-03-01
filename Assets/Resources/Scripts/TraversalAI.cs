﻿using UnityEngine;
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
	public void init(Board B) {
		GetComponent<SpriteRenderer>().color = CustomColors.Traversal_AI;
		board = B;
		x = 1;
		y = 0;
		transform.position = board.getBlockPosition(x, y);
		transform.localScale = new Vector3(size, size, 1);
		moveDirX = -1;
		moveDirY = 0;
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
			print("Finished animation");
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
