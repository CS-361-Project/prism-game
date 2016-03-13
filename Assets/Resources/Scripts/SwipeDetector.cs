using UnityEngine;
using System.Collections;

public class SwipeDetector : MonoBehaviour {
	Vector3 startPos = Vector3.zero;
	const int minSwipeDistX = 50;
	const int minSwipeDistY = 50;
	bool movedOnThisSwipe = false;

	public Vector2 getSwipeDirection() {
		if (Input.touchCount > 0) {
			Touch touch = Input.touches[0];
			switch (touch.phase) {
				case TouchPhase.Began:
					print("Started new swipe");
					startPos = touch.position;
					movedOnThisSwipe = false;
					break;
				case TouchPhase.Stationary:
					print("Started new swipe");
					startPos = touch.position;
					movedOnThisSwipe = false;
					break;
				case TouchPhase.Moved:
					if (!movedOnThisSwipe) {
						print("New position: " + touch.position);
						Vector2 pos = touch.position;
						if (Mathf.Abs(startPos.x - pos.x) >= minSwipeDistX) {
							movedOnThisSwipe = true;
							if (Mathf.Sign(startPos.x - pos.x) > 0) {
								// right swipe
								return Vector2.right;
							}
							else {
								// left swipe;
								return Vector2.left;
							}
						}
						else if (Mathf.Abs(startPos.y - pos.y) >= minSwipeDistY) {
							movedOnThisSwipe = true;
							if (Mathf.Sign(startPos.y - pos.y) > 0) {
								// up swipe
								return Vector2.up;
							}
							else {
								// down swipe
								return Vector2.down;
							}
						}
						else {
							return Vector2.zero;
						}
					}
					break;
			}
		}
		return Vector2.zero;
	}
}

