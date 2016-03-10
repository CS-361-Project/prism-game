using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class StartMenu : MonoBehaviour {

	public Canvas exitMenu;
	public Camera main;

	public Button startButton;
	public Button exitButton;
	public Button howTo;
	public Button yesQuit;
	public Button noQuit;

	public Text exitMessage;
	public Text rTitle;
	public Text gTitle;
	public Text bTitle;

	public bool bgTransitioning = false;
	Color newBG, oldBG;
	float lastColorChange = -1.0f;
	public float transitionTime = 1.0f;

	public Image quitScreen;

	// Use this for initialization
	void Awake() {

		exitMenu = exitMenu.GetComponent<Canvas>();

		quitScreen = quitScreen.GetComponent<Image>();
		quitScreen.color = CustomColors.White;

		main = main.GetComponent<Camera>();
		main.backgroundColor = CustomColors.White;
		newBG = main.backgroundColor;
		oldBG = main.backgroundColor;

		//Buttons 
		startButton = startButton.GetComponent<Button>();
		ColorBlock startCol = startButton.colors;
		startCol.normalColor = CustomColors.Yellow;
		startCol.highlightedColor = Color.Lerp(CustomColors.Yellow, CustomColors.White, .5F);
		startButton.colors = startCol;

		exitButton = exitButton.GetComponent<Button>();
		ColorBlock exitCol = exitButton.colors;
		exitCol.normalColor = CustomColors.Cyan;
		exitCol.highlightedColor = Color.Lerp(CustomColors.Cyan, CustomColors.White, .5F);
		exitButton.colors = exitCol;

		howTo = howTo.GetComponent<Button>();
		ColorBlock howToCol = howTo.colors;
		howToCol.normalColor = CustomColors.Magenta;
		howToCol.highlightedColor = Color.Lerp(CustomColors.Magenta, CustomColors.White, .5F);
		howTo.colors = howToCol;

		yesQuit = yesQuit.GetComponent<Button>();
		ColorBlock yesCol = yesQuit.colors;
		yesCol.normalColor = CustomColors.Green;
		yesCol.highlightedColor = Color.Lerp(CustomColors.Green, CustomColors.White, .5F);
		yesQuit.colors = yesCol;

		noQuit = noQuit.GetComponent<Button>();
		ColorBlock noCol = noQuit.colors;
		noCol.normalColor = CustomColors.Red;
		noCol.highlightedColor = Color.Lerp(CustomColors.Red, CustomColors.White, .5F);
		noQuit.colors = noCol;

		//Text

		exitMessage = exitMessage.GetComponent<Text>();
		exitMessage.color = CustomColors.Blue;

		rTitle = rTitle.GetComponent<Text>();
		rTitle.color = CustomColors.Red;

		gTitle = gTitle.GetComponent<Text>();
		gTitle.color = CustomColors.Green;

		bTitle = bTitle.GetComponent<Text>();
		bTitle.color = CustomColors.Blue;


		exitMenu.enabled = false;
	}

	void Update() {
		if (bgTransitioning) {
			
			whileBGTransitioning(timeSinceLastColorChange() / transitionTime);
		}
	}


	public void ExitPressed() {
		exitMenu.enabled = true;
		startButton.enabled = false;
		exitButton.enabled = false;
		howTo.enabled = false;
	}

	public void NoPressed() {
		exitMenu.enabled = false;
		startButton.enabled = true;
		exitButton.enabled = true;
		howTo.enabled = true;
	}

	public void StartGame() {
		finishBGTransitionImmediate();
		SceneManager.LoadScene("scene");
	}

	public void QuitGame() {
		Application.Quit();
	}

	static Color HexToColor(string hex) {
		byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
		byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
		byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
		return new Color32(r, g, b, 255);
	}

	public void BackgroundChangeRed() {
		if (bgTransitioning) {
			finishBGTransitionImmediate();
		}
		startBGTransition(CustomColors.Red);
	}

	public void BackgroundChangeGreen() {
		if (bgTransitioning) {
			finishBGTransitionImmediate();
		}
		startBGTransition(CustomColors.Green);
	}

	public void BackgroundChangeBlue() {
		if (bgTransitioning) {
			finishBGTransitionImmediate();
		}
		startBGTransition(CustomColors.Blue);
	}

	public void BackgroundChangeYellow() {
		if (bgTransitioning) {
			finishBGTransitionImmediate();
		}
		startBGTransition(CustomColors.Yellow);
	}

	public void BackgroundChangeCyan() {
		if (bgTransitioning) {
			finishBGTransitionImmediate();
		}
		startBGTransition(CustomColors.Cyan);
	}

	public void BackgroundChangeMagenta() {
		if (bgTransitioning) {
			finishBGTransitionImmediate();
		}
		startBGTransition(CustomColors.Magenta);
	}

	public void BackgroundReset() {
		if (bgTransitioning) {
			finishBGTransitionImmediate();
		}
		startBGTransition(CustomColors.White);
	}


	public void startBGTransition(Color bgColor) {
		newBG = bgColor;
		bgTransitioning = true;
		lastColorChange = Time.time;
	}

	public void finishBGTransitionImmediate() {
		newBG = main.backgroundColor;
		whileBGTransitioning(1.0f);
	}

	public void whileBGTransitioning(float t) {
		if (t >= 1) {
			main.backgroundColor = newBG;
			oldBG = newBG;
			bgTransitioning = false;
		}
		main.backgroundColor = Color.Lerp(oldBG, newBG, t);

	}

	public float timeSinceLastColorChange() {
		return Time.time - lastColorChange;
	}

	public Color getBackgroundColor() {
		return main.backgroundColor;
	}
		

}
