using UnityEngine;
using System.Collections;

public class Player : Movable {
	//Sound Effects
	AudioSource audioSource;
	public AudioClip moveSound;
	public AudioClip squishSound;

	SpriteRenderer rend;
	Color baseColor = Color.white;
	Color greyColor = CustomColors.Grey;

	// Use this for initialization
	public void init(Board b) {
		base.init(b, 0, b.getHeight() - 1);
		rend = GetComponent<SpriteRenderer>();
		rend.color = baseColor;

		//Initialize AudioSource
		audioSource = gameObject.AddComponent<AudioSource>();
		moveSound = Resources.Load("Audio/ScrollUp", typeof(AudioClip)) as AudioClip;
		squishSound = Resources.Load("Audio/ScrollDown", typeof(AudioClip)) as AudioClip;
	}

	public override bool move(Vector2 direction) {
		float vol = determineVolume();
		if (base.move(direction)) {
			if (board.checkIfKillPlayer()) {
				board.killPlayer();
			}
			audioSource.PlayOneShot(moveSound, vol);
			return true;
		}
		else {
			audioSource.PlayOneShot(squishSound, vol);
			return false;
		}
	}

	float determineVolume(){
		//I want it to return full volume quicker and I want to take longer to get min volume
		float vol = (timeSinceLastMovement()*2 + 0.3f);
		vol = Mathf.Clamp(vol, .3f, 1.0f);
		return vol;
	}

	public void onBackgroundTransition(Color oldBG, Color newBG, float progress) {
		if (newBG == CustomColors.White) {
			if (progress >= 1) {
				rend.color = greyColor;
			}
			else {
				rend.color = Color.Lerp(baseColor, greyColor, progress);
			}
		}
		else if (oldBG == CustomColors.White) {
			if (progress >= 1) {
				rend.color = baseColor;
			}
			else {
				rend.color = Color.Lerp(greyColor, baseColor, progress);
			}
		}
	}

	public override void onMovementStart() {
		if (moving && board.getBlock(x, y).name == "Lever") {
			LeverBlock lever = (LeverBlock)board.getBlock(x, y);
			lever.toggle();
		}
		lastMovement = Time.time;
	}
}
