using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MoveCounter : MonoBehaviour {
	int moves;
	Text text;
	// Use this for initialization
	void Awake () {
		moves = 0;
		text = GetComponent<Text>();
		text.text = moves.ToString();
	}
	
	public void increment() {
		moves++;
		text.text = moves.ToString();
	}

	public void reset() {
		moves = 0;
		text.text = moves.ToString();
	}
}
