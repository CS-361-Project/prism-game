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
	public bool redClicked = true;
	public Text gTitle;
	public bool greenClicked = true;
	public Text bTitle;
	public bool blueClicked = true;

	public Image quitScreen;

	// Use this for initialization
	void Awake () {

		exitMenu = exitMenu.GetComponent<Canvas> ();

		/*quitScreen = quitScreen.GetComponent<Image> ();
		quitScreen.color = CustomColors.Black;*/

		main = main.GetComponent<Camera>();

		//Buttons 
		startButton = startButton.GetComponent<Button> ();
		ColorBlock startCol = startButton.colors;
		startCol.normalColor = CustomColors.Yellow;
		float diffr = (1F - CustomColors.Yellow.r)/2.0F;
		float diffg = (1F - CustomColors.Yellow.g)/2.0F;
		float diffb = (1F - CustomColors.Yellow.b)/2.0F;
		startCol.highlightedColor = new Color (CustomColors.Yellow.r+diffr, CustomColors.Yellow.g+diffg, CustomColors.Yellow.b+diffb);
		startButton.colors = startCol;

		exitButton = exitButton.GetComponent<Button> ();
		ColorBlock exitCol = exitButton.colors;
		exitCol.normalColor = CustomColors.Cyan;
		float diffr1 = (1F - CustomColors.Cyan.r)/2F;
		float diffg1 = (1F - CustomColors.Cyan.g)/2F;
		float diffb1 = (1F - CustomColors.Cyan.b)/2F;
		exitCol.highlightedColor = new Color (CustomColors.Cyan.r + diffr1, CustomColors.Cyan.g + diffg1, CustomColors.Cyan.b +diffb1);
		exitButton.colors = exitCol;

		howTo = howTo.GetComponent<Button> ();
		ColorBlock howToCol = howTo.colors;
		howToCol.normalColor = CustomColors.Magenta;
		float diffr2 = (1F - CustomColors.Magenta.r)/2F;
		float diffg2 = (1F - CustomColors.Magenta.g)/2F;
		float diffb2 = (1F - CustomColors.Magenta.b)/2F;
		howToCol.highlightedColor = new Color (CustomColors.Magenta.r + diffr2, CustomColors.Magenta.g + diffg2, CustomColors.Magenta.b +diffb2);
		howTo.colors = howToCol;

		yesQuit = yesQuit.GetComponent<Button> ();
		ColorBlock yesCol = yesQuit.colors;
		yesCol.normalColor = CustomColors.Green;
		float diffr3 = (1F - CustomColors.Green.r)/2F;
		float diffg3 = (1F - CustomColors.Green.g)/2F;
		float diffb3 = (1F - CustomColors.Green.b)/2F;
		yesCol.highlightedColor = new Color (CustomColors.Yellow.r + diffr3, CustomColors.Yellow.g + diffg3, CustomColors.Yellow.b +diffb3);
		yesQuit.colors = yesCol;

		noQuit = noQuit.GetComponent<Button> ();
		ColorBlock noCol = noQuit.colors;
		noCol.normalColor = CustomColors.Red;
		float diffr4 = (1F - CustomColors.Red.r)/2F;
		float diffg4 = (1F - CustomColors.Red.g)/2F;
		float diffb4 = (1F - CustomColors.Red.b)/2F;
		noCol.highlightedColor = new Color (CustomColors.Red.r + diffr4, CustomColors.Red.g + diffg4, CustomColors.Red.b +diffb4);
		noQuit.colors = noCol;

		//Text

		exitMessage = exitMessage.GetComponent<Text> ();
		exitMessage.color = CustomColors.Blue;

		rTitle = rTitle.GetComponent<Text> ();
		rTitle.color = CustomColors.Red;

		gTitle = gTitle.GetComponent<Text> ();
		gTitle.color = CustomColors.Green;

		bTitle = bTitle.GetComponent<Text> ();
		bTitle.color = CustomColors.Blue;


		exitMenu.enabled = false;
	}

	public void ExitPressed(){
		exitMenu.enabled = true;
		startButton.enabled = false;
		exitButton.enabled = false;
		howTo.enabled = false;
	}

	public void NoPressed(){
		exitMenu.enabled = false;
		startButton.enabled = true;
		exitButton.enabled = true;
		howTo.enabled = true;
	}

	public void StartGame(){
		SceneManager.LoadScene ("scene");
	}

	public void QuitGame(){
		Application.Quit ();
	}

	static Color HexToColor(string hex) {
		byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
		byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
		byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
		return new Color32(r, g, b, 255);
	}

	public void BackgroundChangeRed(){
		if (redClicked) {
			main.backgroundColor = CustomColors.Red;
			redClicked = !redClicked;
		} else {
			main.backgroundColor = new Color (0, 0, 0);
			redClicked = !redClicked;
		}
	}

	public void BackgroundChangeGreen(){
		if (greenClicked) {
			main.backgroundColor = CustomColors.Green;
			greenClicked = !greenClicked;
		} else {
			main.backgroundColor = new Color (0, 0, 0);
			greenClicked = !greenClicked;
		}
	}

	public void BackgroundChangeBlue(){
		if (blueClicked) {
			main.backgroundColor = CustomColors.Blue;
			blueClicked = !blueClicked;
		} else {
			main.backgroundColor = new Color (0, 0, 0);
			blueClicked = !blueClicked;
		}
	}

}
