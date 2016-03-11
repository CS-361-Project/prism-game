using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelNumber : MonoBehaviour {
	Text text;
	// Use this for initialization
	void Start () {
		text = GetComponent<Text>();
	}
	public void setLevel(int n) {
		text.text = n.ToString();
	}
}
