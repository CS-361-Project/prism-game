using UnityEngine;
using System.Collections;

public class Exit : MonoBehaviour {
	Board board;
	int x, y;
	public void init(Board parent) {
		board = parent;
		transform.parent = parent.transform;
		x = parent.getWidth() - 1;
		y = 0;
		transform.position = parent.getBlockPosition(x, y);
	}
	public void moveTo(int newX, int newY) {
		x = newX;
		y = newY;
		transform.position = board.getBlockPosition(x, y);
	}
}
