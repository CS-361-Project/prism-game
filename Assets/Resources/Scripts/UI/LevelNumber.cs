using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelNumber : MonoBehaviour {
	Text text;
	int currLevel = 0;
	// Use this for initialization
	void Start () {
		currLevel = 0;
	}
	public void OnEnable() {
		text = GetComponent<Text>();
		text.text = currLevel.ToString ();
	}
	public void setLevel(int n) {
		currLevel = n;
	}
}
