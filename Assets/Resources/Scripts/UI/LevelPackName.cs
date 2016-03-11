using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelPackName : MonoBehaviour {
	Text text;
	// Use this for initialization
	void Start () {
		text = GetComponent<Text>();
	}
	
	public void setLevelPack(string s) {
		text.text = s;
	}
}
