using UnityEngine;
using System.Collections;

public class FloatingBlock : MonoBehaviour {

	protected FloatingBlockManager manager;
	protected FloatingBlockModel floatingBlockModel;
	protected Color baseColor;
	protected float speed;
	protected bool direction;

	public virtual void init(Color c, FloatingBlockManager b, Transform parent, float sp) {
		manager = b;
		transform.parent = parent;
		floatingBlockModel = Instantiate (Resources.Load<GameObject> ("Prefabs/Block")).GetComponent<FloatingBlockModel>();
		baseColor = c;
		floatingBlockModel.init(this.transform, baseColor);
		name = "Floating Block";

		if (Random.Range (0, 10) <= 5) {
			direction = true;
		} else {
			direction = false;
		}
		speed = sp;
	}
		
	
	// Update is called once per frame
	void Update () {
		Vector3 screen = Camera.main.ScreenToWorldPoint (new Vector3 (Screen.width, Screen.height, 0));
		borders (screen.x, screen.y);

		if (direction) {
			transform.Translate (speed, 0, 0);
		} else {
			transform.Translate (0, speed, 0);
		}

	
	}
	void borders(float xsize, float ysize){
		if (transform.localPosition.x > xsize || transform.localPosition.x < -xsize) {
			if (transform.localPosition.x > xsize) {
				transform.localPosition = new Vector3 (transform.localPosition.x-(xsize*2),transform.localPosition.y,0);
			} else {
				transform.localPosition = new Vector3 (transform.localPosition.x+(xsize*2),transform.localPosition.y,0);
			} 
		}
		if (transform.localPosition.y > ysize || transform.localPosition.y < -ysize) {
			if (transform.localPosition.y > ysize) {
				transform.localPosition = new Vector3 (transform.localPosition.x,transform.localPosition.y-(ysize*2),0);
			} else {
				transform.localPosition = new Vector3 (transform.localPosition.x,transform.localPosition.y+(ysize*2),0);
			} 
		}
	}
}
