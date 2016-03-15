using UnityEngine;
using System.Collections;

public class AudioControl : MonoBehaviour {
	float volume;
	public static AudioControl Instance;

	void Awake () {
		if (Instance == null) {
			DontDestroyOnLoad(gameObject);
			Instance = this;
		}
		else if (Instance != this) {
			Destroy (gameObject);
		}
	}

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
