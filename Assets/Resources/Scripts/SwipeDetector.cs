using UnityEngine;
using System.Collections;

public class SwipeDetector : MonoBehaviour {
	Vector2 startPos = Vector2.zero;
	Vector2 lastPos = Vector2.zero;
	IntPoint lastCoords = IntPoint.zero;
	float blockSize = 100;
	bool moved;

	public void init(Board b) {
		blockSize = 100;
		if (b.blocks.Length > 0) {
			blockSize = b.getBlock(0, 0).gameObject.transform.localScale.x;
		}
	}

	public Vector2 getSwipeDirection() {
		moved = false;
		if (Input.touchCount > 0) {
			Touch touch = Input.touches[0];
			switch (touch.phase) {
			case TouchPhase.Began:
				startPos = touch.position;
				lastCoords = IntPoint.zero;
				break;
			case TouchPhase.Moved:
				Vector2 pos = touch.position;
				IntPoint relCoords = new IntPoint(Mathf.RoundToInt((pos.x - startPos.x) / blockSize), Mathf.RoundToInt((pos.y - startPos.y) / blockSize));
				if (relCoords != lastCoords) {
					IntPoint dir = relCoords - lastCoords;
					dir.normalize();
					lastCoords = relCoords;
					return dir.getVector2();
				}
				break;
			}
		}
		return Vector2.zero;
	}
}

