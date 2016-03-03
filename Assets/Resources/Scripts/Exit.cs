using UnityEngine;
using System.Collections;

public class Exit : MonoBehaviour {
	Board board;
	public int x, y;
	public float size = 1.0f;
	SpriteRenderer rend;
	Color baseColor = CustomColors.White;
	Color greyColor = CustomColors.Grey;

	public void init(Board parent) {
		rend = GetComponent<SpriteRenderer>();
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

	public void onBackgroundTransition(Color oldBG, Color newBG, float progress) {
		if (newBG == CustomColors.White) {
			if (progress >= 1) {
				rend.color = greyColor;
			}
			else {
				rend.color = Color.Lerp(baseColor, greyColor, progress);
			}
		}
		else if (oldBG == CustomColors.White) {
			if (progress >= 1) {
				rend.color = baseColor;
			}
			else {
				rend.color = Color.Lerp(greyColor, baseColor, progress);
			}
		}
	}
}
