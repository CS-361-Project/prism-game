using UnityEngine;
using System.Collections;

public class VerticalEnemy : Enemy {
	public override void init(Board B, int xPos, int yPos) {
		//GetComponent<SpriteRenderer>().color = CustomColors.TraversalEnemy;
		base.init(B, xPos, yPos, 0, 1);
		//enemyModel.transform.Rotate(0, 0, -90);
	}


	public override void changeDirection() {
		//int oldDirX = moveDirX;
		moveDirX *= -1;
		moveDirY *= -1;
		//change the direction indicator for a horizontal AI
		if(moveDirY==1){
			enemyModel.changeIndicator(true);
		} else{
			enemyModel.changeIndicator(false);
		}
	}

}