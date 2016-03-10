using UnityEngine;
using System.Collections;

public class Enemy : Movable {
	
	public bool markedForDeath = false;
	protected EnemyModel enemyModel;


	public virtual void init(Board B, int xPos, int yPos, int xDirection, int yDirection) {
		base.init(B, xPos, yPos);
		moveDirX = xDirection;
		moveDirY = yDirection;
		board.setHasEnemy(x, y, this);
		markedForDeath = false;
		//should I or should I not instiate the prefab here?
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
			move(new Vector2(moveDirX, moveDirY));
		}
		lastMovement = time;
	}

	public virtual void changeDirection() {
		moveDirX *= -1;
		moveDirY *= -1;
	}

	public override void onMovementStart() {
		board.setHasEnemy(oldX, oldY, this);
		board.setHasEnemy(x, y, this);
		lastMovement = Time.time;
	}

	public void onKill() {
		board.setHasEnemy(x, y, this);
	}

	public override bool canPassThrough(int x, int y) {
		return board.getBlockPassableAfterTransition(x, y);
	}


}

