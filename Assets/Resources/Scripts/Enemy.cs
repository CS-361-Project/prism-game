using UnityEngine;
using System.Collections;

public class Enemy : Movable {
	
	public bool markedForDeath = false;
	protected EnemyModel enemyModel;

	public virtual void init(Board B, int xPos, int yPos, int xDirection, int yDirection) {
		base.init(B, xPos, yPos);
		moveDirX = xDirection;
		moveDirY = yDirection;
		board.setHasEnemy(x, y, true);
		markedForDeath = false;


		enemyModel = gameObject.GetComponentInChildren<EnemyModel>();
		enemyModel.init(this.transform, xDirection,yDirection);
		enemyModel.setColor(CustomColors.TraversalEnemy);
	}

	public void move(float time) {
		if (board.getBlockPassable(x, y) && !board.getBlockPassableAfterTransition(x, y)) {
			markedForDeath = true;
		}
		if (canPassThrough(x + moveDirX, y + moveDirY)) {
			move(new Vector2(moveDirX, moveDirY));
		}
		else {
			changeDirection();
			if (canPassThrough(x + moveDirX, y + moveDirY)) {
				move(new Vector2(moveDirX, moveDirY));
			}
			else {
				changeDirection();
			}
		}
		lastMovement = time;
	}

	public virtual void changeDirection() {
		moveDirX = -moveDirX;
		moveDirY = -moveDirY;
	}

	public override void onMovementStart() {
		board.setHasEnemy(oldX, oldY, false);
		board.setHasEnemy(x, y, true);
		lastMovement = Time.time;
	}

	public void onKill() {
		board.setHasEnemy(x, y, false);
	}

	public override bool canPassThrough(int x, int y) {
		return board.getBlockPassableAfterTransition(x, y);
	}


}

