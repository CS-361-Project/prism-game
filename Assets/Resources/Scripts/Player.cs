using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : Movable {
	//Sound Effects
	AudioSource audioSource;
	public AudioClip moveSound;
	public AudioClip squishSound;
	public GameObject eye;

	SpriteRenderer rend;
	Color baseColor = Color.white;
	Color greyColor = CustomColors.Grey;
	public int toggleCount;

	// Use this for initialization
	public void init(Board b) {
		base.init(b, 0, b.getHeight() - 1);
		rend = GetComponent<SpriteRenderer>();
		rend.color = baseColor;
		toggleCount = 0;

		//Initialize AudioSource
		audioSource = gameObject.AddComponent<AudioSource>();
		moveSound = Resources.Load("Audio/ScrollUp", typeof(AudioClip)) as AudioClip;
		squishSound = Resources.Load("Audio/ScrollDown", typeof(AudioClip)) as AudioClip;
	}

	public void Start(){
		//points the eye in the right direction initally
		Vector3 forward = board.getExit().transform.position - eye.GetComponent<Transform>().position;
		float ang = Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg;
		eye.GetComponent<Transform>().rotation = Quaternion.Euler(new Vector3(0, 0, ang));
	}

	public override bool move(Vector2 direction) {
		float vol = determineVolume();
		if (base.move(direction)) {
//			if (board.checkIfKillPlayer()) {
//				board.killPlayer();
//			}

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
//		float vol = (timeSinceLastMovement()*2 + 0.3f);
//		vol = Mathf.Clamp(vol, .3f, 1.0f);
//		return vol;
		return 0.05f;
	}

	public void onBackgroundTransition(Color oldBG, Color newBG, float progress) {
		if (newBG == CustomColors.White) {
			if (progress >= 1) {
				rend.color = greyColor;
				eye.GetComponent<SpriteRenderer>().color = greyColor;
			}
			else {
				rend.color = Color.Lerp(baseColor, greyColor, progress);
				eye.GetComponent<SpriteRenderer>().color = Color.Lerp(baseColor, greyColor, progress);
			}
		}
		else if (oldBG == CustomColors.White) {
			if (progress >= 1) {
				rend.color = baseColor;
				eye.GetComponent<SpriteRenderer>().color = baseColor;
			}
			else {
				rend.color = Color.Lerp(greyColor, baseColor, progress);
				eye.GetComponent<SpriteRenderer>().color = Color.Lerp(greyColor, baseColor, progress);
			}
		}
	}

	public override void whileMoving(float percentDone) {
		Vector3 forward = board.getExit().transform.position - eye.GetComponent<Transform>().position;
		float ang = Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg;
		eye.GetComponent<Transform>().rotation = Quaternion.Euler(new Vector3(0, 0, ang));
		base.whileMoving(percentDone);
	}

	public override void onMovementStart() {
		if (moving && board.getBlock(x, y).name == "Lever") {
			LeverBlock lever = (LeverBlock)board.getBlock(x, y);
			lever.toggle();
			toggleCount++;
		}
		lastMovement = Time.time;
	}
}
