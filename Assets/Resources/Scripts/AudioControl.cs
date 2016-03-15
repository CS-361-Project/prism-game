using UnityEngine;
using System.Collections;

public class AudioControl : MonoBehaviour {
	float volume;
	AudioSource soundtrack;
	AudioClip[] tracks;
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
		tracks = new AudioClip[3];
		tracks[0] = Resources.Load<AudioClip>("Audio/easyTrack");
		tracks[1] = Resources.Load<AudioClip>("Audio/mediumTrack");
		tracks[2] = Resources.Load<AudioClip>("Audio/hardTrack");
		soundtrack = gameObject.GetComponent<AudioSource>();
		soundtrack.clip = tracks[Random.Range(0, 2)];
		setVolume(1.0f);
		soundtrack.Play();
	}

	void Update() {
		if (!soundtrack.isPlaying) {
			soundtrack.clip = tracks[Random.Range(0, 2)];
			soundtrack.Play();
		}
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
