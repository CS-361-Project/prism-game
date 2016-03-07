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
		borders (Camera.main.orthographicSize);
		if (direction) {
			transform.Translate (speed, 0, 0);
		} else {
			transform.Translate (0, speed, 0);
		}

	
	}
	void borders(float size){
		if (transform.localPosition.x > size || transform.localPosition.x < -size) {
			if (transform.localPosition.x > size) {
				transform.localPosition = new Vector3 (transform.localPosition.x-(size*2),transform.localPosition.y,0);
			} else {
				transform.localPosition = new Vector3 (transform.localPosition.x+(size*2),transform.localPosition.y,0);
			} 
		}
		if (transform.localPosition.y > size || transform.localPosition.y < -size) {
			if (transform.localPosition.y > size) {
				transform.localPosition = new Vector3 (transform.localPosition.x,transform.localPosition.y-(size*2),0);
			} else {
				transform.localPosition = new Vector3 (transform.localPosition.x,transform.localPosition.y+(size*2),0);
			} 
		}
	}
}
