using UnityEngine;
using System.Collections;

public class EnemyModel : MonoBehaviour {
	protected SpriteRenderer leftRend, rightRend, upRend, downRend;
	public Sprite leftArrow, rightArrow, upArrow, downArrow, leftArrowOutline, rightArrowOutline, upArrowOutline, downArrowOutline;

	public int directionX;
	public int directionY;
	public void init (Transform parent, int direcX, int direcY) {
		transform.parent = parent;
		transform.localPosition = new Vector3(0, 0, 0);
		//print("I have been created" + parent);
		/*directionX = direcX;
		directionY = direcY;


		//Left Direction Indicator
		GameObject obj = new GameObject();
		obj.name = "Left Direction Indicator";
		obj.transform.parent = parent;
		obj.transform.localPosition = new Vector3(0, 0, 0);
		obj.transform.localScale = transform.localScale;
		leftRend = obj.AddComponent<SpriteRenderer>();
		//rend.material.shader = Shader.Find("Transparent/Unlit"); 
		leftRend.sortingLayerName = "OffArrowLayer";
		leftRend.sortingOrder = 2;
		leftRend.color = new Color(1, 1, 1);

		//Right Direciton Indicator
		GameObject arrow = new GameObject();
		arrow.name = "Right Direction Indicator";
		arrow.transform.parent = parent;
		arrow.transform.localPosition = new Vector3(0, 0, 0);
		arrow.transform.localScale = transform.localScale;
		rightRend = arrow.AddComponent<SpriteRenderer>();
		rightRend.sortingLayerName = "OnArrowLayer";
		//render.material.shader = Shader.Find("Transparent/Unlit"); 
		rightRend.sortingOrder = 2;
		rightRend.color = new Color(1, 1, 1);

		//Up Direciton Indicator
		GameObject upArrowObj = new GameObject();
		upArrowObj.name = "Up Direction Indicator";
		upArrowObj.transform.parent = parent;
		upArrowObj.transform.localPosition = new Vector3(0, 0, 0);
		upArrowObj.transform.localScale = transform.localScale;
		upRend = upArrowObj.AddComponent<SpriteRenderer>();
		upRend.sortingLayerName = "OnArrowLayer";
		//render.material.shader = Shader.Find("Transparent/Unlit"); 
		upRend.sortingOrder = 2;
		upRend.color = new Color(1, 1, 1);

		//Down Direciton Indicator
		GameObject downArrowObj = new GameObject();
		downArrowObj.name = "Down Direction Indicator";
		downArrowObj.transform.parent = parent;
		downArrowObj.transform.localPosition = new Vector3(0, 0, 0);
		downArrowObj.transform.localScale = transform.localScale;
		downRend = downArrowObj.AddComponent<SpriteRenderer>();
		downRend.sortingLayerName = "OffArrowLayer";
		//render.material.shader = Shader.Find("Transparent/Unlit"); 
		downRend.sortingOrder = 2;
		downRend.color = new Color(1, 1, 1);


		if (direcY == 0) {
			leftRend.sprite = leftArrowOutline;
			rightRend.sprite = rightArrow;
		}
		else if (direcX == 0) {
			upRend.sprite = downArrow;
			downRend.sprite = upArrowOutline;

		}*/
	}



	public void setColor(Color x){
		GetComponent<SpriteRenderer>().color = x;

	}

	//This is not a universal function and instead is only for horizontal AI
	public void changeIndicator(bool right){
		//check what type of enemy it is then do the swithc
		if (directionY == 0) {
			if (right) {
				rightRend.sprite = rightArrow;
				leftRend.sprite = leftArrowOutline;
			}
			else {
				rightRend.sprite = leftArrow;
				leftRend.sprite = rightArrowOutline;

			}
		}
		else
		if (directionX == 0) {
				if (right) {
					upRend.sprite = upArrow;
					downRend.sprite = downArrowOutline;
				}
				else {
					upRend.sprite = downArrow;
					downRend.sprite = upArrowOutline;

				}

		}
	}


}
