using UnityEngine;
using System.Collections;

public class LeverBlock : Block {
	public Color leverColor;
	public bool isToggled;
	Board board;

	//AudioSource soundEffect;
	AudioSource audioSource;
	public AudioClip toggleOnSound;
	public AudioClip toggleOffSound;

	public ColorModel colorModel;

	public override void init(Color c, Color bgColor, Board b, Transform parent) {
		blockModel = Instantiate(Resources.Load<GameObject>("Prefabs/Lever")).GetComponent<LeverModel>();
		blockModel.init(this.transform, c);
		transform.parent = parent;
		name = "Lever";
		board = b;
		leverColor = c;
		isToggled = false;

		colorModel = GameObject.Find ("RGB Diagram").GetComponent<ColorModel> ();

		//Initialize AudioSource
		audioSource = gameObject.AddComponent<AudioSource>();
		toggleOnSound = Resources.Load("Audio/switchOn", typeof(AudioClip)) as AudioClip;
		toggleOffSound = Resources.Load("Audio/switchOff", typeof(AudioClip)) as AudioClip;
	}

	public override bool isPassable() {
		return true;
	}

	public override bool passableWithBG(Color bgColor) {
		return true;
	}

	public override void onBackgroundChange(Color bgColor) {
		isToggled = CustomColors.contains(bgColor, leverColor);
		blockModel.setActive(isToggled);
	}

	public void toggle() {
		Color c = board.getBackgroundColor();
		if (isToggled) {
			board.startBGTransition(CustomColors.subColor(c, leverColor));
			//turn off
			audioSource.PlayOneShot(toggleOffSound, .2f);
			colorModel.onSwitch (CustomColors.subColor (c, leverColor));
		}
		else {
			board.startBGTransition(CustomColors.addColor(c, leverColor));
			audioSource.PlayOneShot(toggleOnSound, .2f);
			colorModel.onSwitch(CustomColors.addColor(c, leverColor));
		}


	}

	public bool getState() {
		return isToggled;
	}
}