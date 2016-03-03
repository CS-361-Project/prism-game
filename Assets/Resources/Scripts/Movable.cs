using UnityEngine;
using System.Collections;

public class Movable : MonoBehaviour {
	protected Board board;

	protected int x, y, oldX, oldY, moveDirX, moveDirY;
	protected float lastMovement = 0.0f;

	public float size = 0.7f;
	public float blockSize = 1.0f;
	public float targetSquish = 0.5f;
	public float targetExpand = 0.9f;
	public float moveSquish = 0.6f;
	public float moveStretch = 1.15f;
	public bool animating = false;
	public bool moving = false;

	public virtual void init(Board b, int xPos, int yPos) {
		transform.parent = b.transform;
		board = b;
		x = xPos;
		y = yPos;
		oldX = x;
		oldY = y;
		transform.localScale = new Vector3(size, size, 1);
		transform.position = board.getBlockPosition(x, y);
	}

	public virtual bool move(Vector2 direction) {
		if (direction == Vector2.zero) {
			return false;
		}
		bool moved;
		animating = true;
		int dx = (int)direction.x;
		int dy = (int)direction.y;
		moveDirX = dx;
		moveDirY = dy;
		oldX = x;
		oldY = y;
		moving = false;
		moved = canPassThrough(x + dx, y + dy);
		if (moved) {
			x = x + dx;
			y = y + dy;
			moving = true;
		}
		onMovementStart();
		return moved;
	}

	public virtual bool canPassThrough(int x, int y) {
		return board.getBlockPassable(x, y);
	}

	public void whileMoving(float percentDone) {
		if (percentDone >= 1.0f) {
			percentDone = 1.0f;
			moving = false;
			animating = false;
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

	public virtual void onMovementStart() {
		lastMovement = Time.time;
	}

	public void finishMovementImmedate() {
		whileMoving(1.0f);
	}

	public bool finishedMovement() {
		return !animating;
	}

	public float timeSinceLastMovement() {
		return Time.time - lastMovement;
	}

	public float lastMovementTime() {
		return lastMovement;
	}

	public int[] getPos(){
		int[] d = {x,y};
		return d;
	}

	public int[] getOldPos() {
		int[] d = {oldX, oldY};
		return d;
	}
}

