using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class menu : MonoBehaviour {

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

	// Use this for initialization
	void Start () {

		exitMenu = exitMenu.GetComponent<Canvas> ();

		main = main.GetComponent<Camera>();
		main.backgroundColor = CustomColors.Black;

		//Buttons 
		startButton = startButton.GetComponent<Button> ();
		ColorBlock startCol = startButton.colors;
		startCol.normalColor = CustomColors.Yellow;
		startButton.colors = startCol;

		exitButton = exitButton.GetComponent<Button> ();
		ColorBlock exitCol = exitButton.colors;
		exitCol.normalColor = CustomColors.Cyan;
		exitButton.colors = exitCol;

		howTo = howTo.GetComponent<Button> ();
		ColorBlock howToCol = howTo.colors;
		howToCol.normalColor = CustomColors.Magenta;
		howTo.colors = howToCol;

		yesQuit = yesQuit.GetComponent<Button> ();
		ColorBlock yesCol = yesQuit.colors;
		yesCol.normalColor = CustomColors.Green;
		yesQuit.colors = yesCol;

		noQuit = noQuit.GetComponent<Button> ();
		ColorBlock noCol = noQuit.colors;
		noCol.normalColor = CustomColors.Red;
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

}
