using UnityEngine;
using System.Collections;

public class SwipeDetector : MonoBehaviour {
	Vector2 startPos = Vector2.zero;
	Vector2 lastPos = Vector2.zero;
	const int minSwipeDistX = 100;
	const int minSwipeDistY = 100;
	bool movedOnThisSwipe = false;

	public Vector2 getSwipeDirection() {
		if (Input.touchCount > 0) {
			Touch touch = Input.touches[0];
			switch (touch.phase) {
				case TouchPhase.Began:
//					print("Started new swipe");
					startPos = touch.position;
					lastPos = startPos;
					movedOnThisSwipe = false;
					break;
//				case TouchPhase.Stationary:
//					print("Started new swipe");
//					startPos = touch.position;
//					movedOnThisSwipe = false;
//					break;
				case TouchPhase.Moved:
					Vector2 pos = touch.position;
					if (Vector2.Distance(lastPos, startPos) > Vector2.Distance(pos, startPos)) {
						startPos = pos;
						lastPos = pos;
						movedOnThisSwipe = false;
						print("Switched directions");
					}
					if (!movedOnThisSwipe) {
						if (Mathf.Abs(startPos.x - pos.x) >= minSwipeDistX) {
							movedOnThisSwipe = true;
							if (Mathf.Sign(startPos.x - pos.x) > 0) {
								return Vector2.left;
							}
							else {
								return Vector2.right;
							}
						}
						else if (Mathf.Abs(startPos.y - pos.y) >= minSwipeDistY) {
							movedOnThisSwipe = true;
							if (Mathf.Sign(startPos.y - pos.y) > 0) {
								return Vector2.down;
							}
							else {
								return Vector2.up;
							}
						}
						else {
							return Vector2.zero;
						}
					}
					lastPos = pos;
					break;
			}
		}
		//print("No swipes detected");
		return Vector2.zero;
	}
}

