using UnityEngine;
using System.Collections;

public class EnemyModel : MonoBehaviour {
	protected SpriteRenderer rend;
	public Sprite leftArrow, rightArrow, upArrow, downArrow, leftArrowOutline, rightArrowOutline, upArrowOutline, downArrowOutline;

	public void init (Transform parent, int direcX, int direcY) {
		transform.parent = parent;
		transform.localPosition = new Vector3(0, 0, 0);
		print("I have been created" + parent);

		GameObject obj = new GameObject();
		obj.name = "Direction Indicators";
		obj.transform.parent = parent;
		obj.transform.localPosition = new Vector3(0, 0, 0);
		obj.transform.localScale = transform.localScale;
		rend = obj.AddComponent<SpriteRenderer>();
		rend.sortingLayerName = "Characters";
		rend.sortingOrder = 2;
		rend.color = new Color(1, 1, 1);
		//leftArrow = Resources.Load<Sprite>("Sprites/leftArrow");
		//rightArrow = Resources.Load<Sprite>("Sprites/rightArrow");

		rend.sprite = rightArrow;
		//changeIndicator();
	}



	public void setColor(Color x){
		GetComponent<SpriteRenderer>().color = x;

	}

	//This is not a universal function and instead is only for horizontal AI
	public void changeIndicator(bool right){
		//check what the direction is 
		if(right){
			rend.sprite= rightArrow;
		} 
		else {
			rend.sprite = leftArrow;
		}

	}


}
