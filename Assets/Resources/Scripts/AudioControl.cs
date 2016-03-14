using UnityEngine;
using System.Collections;

public class AudioControl : MonoBehaviour {
	float volume;
	void Start() {
		setVolume(1.0f);
	}
	public void setVolume(float v) {
		AudioListener.volume = v;
		volume = v;
	}
	public void toggleVolume() {
		if (volume >= 0) {
			setVolume(0.0f);
		}
		else {
			setVolume(1.0f);
		}
	}
}
