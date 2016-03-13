using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelPackName : MonoBehaviour {
	Text text;
	string currPack = "Tutorial";
	// Use this for initialization
	void Start () {
		text = GetComponent<Text>();
	}

	void OnEnable() {
		text = GetComponent<Text>();
		text.text = currPack;
	}
	
	public void setLevelPack(string s) {
		currPack = s;
	}
}
