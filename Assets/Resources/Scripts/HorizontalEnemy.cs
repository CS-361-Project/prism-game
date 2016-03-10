using UnityEngine;
using System.Collections;

public class HorizontalEnemy : Enemy {
	
	public override void init(Board B, int xPos, int yPos) {
		

		base.init(B, xPos, yPos, 1, 0);


	}


	public override void changeDirection() {
		//int oldDirX = moveDirX;
		moveDirX *= -1;
		moveDirY *= -1;
		//change the direction indicator for a horizontal AI
		if(moveDirX==1){
			enemyModel.changeIndicator(true);
		} else{
			enemyModel.changeIndicator(false);
		}
	}
}