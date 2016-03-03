using UnityEngine;
using System.Collections;

public class VerticalEnemy : Enemy {
	public override void init(Board B, int xPos, int yPos) {
		GetComponent<SpriteRenderer>().color = CustomColors.TraversalEnemy;
		base.init(B, xPos, yPos, 0, 1);
	}
}