using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class menu : MonoBehaviour {

	public Canvas exitMenu;

	public Button startButton;
	public Button exitButton;
	public Button howTo;
	public Button yesQuit;
	public Button noQuit;

	public Text exitMessage;
	public Text rTitle;
	public Text gTitle;
	public Text bTitle;


	public static Color Red = HexToColor("FF6978");
	public static Color Green = HexToColor("94EA54");
	public static Color Blue = HexToColor("3885F9");
	public static Color Yellow = HexToColor("F4EE36");
	public static Color Magenta = HexToColor("C85ED8");
	public static Color Cyan = HexToColor("4CEDDE");
	public static Color Black = HexToColor("464B63");
	public static Color White = HexToColor("FFF8F4");
	public static Color Grey = HexToColor("D0D7E5");

	// Use this for initialization
	void Start () {

		exitMenu = exitMenu.GetComponent<Canvas> ();

		//Buttons 
		startButton = startButton.GetComponent<Button> ();
		ColorBlock startCol = startButton.colors;
		startCol.normalColor = Yellow;
		startButton.colors = startCol;

		exitButton = exitButton.GetComponent<Button> ();
		ColorBlock exitCol = exitButton.colors;
		exitCol.normalColor = Cyan;
		exitButton.colors = exitCol;

		howTo = howTo.GetComponent<Button> ();
		ColorBlock howToCol = howTo.colors;
		howToCol.normalColor = Magenta;
		howTo.colors = howToCol;

		yesQuit = yesQuit.GetComponent<Button> ();
		ColorBlock yesCol = yesQuit.colors;
		yesCol.normalColor = Green;
		yesQuit.colors = yesCol;

		noQuit = noQuit.GetComponent<Button> ();
		ColorBlock noCol = noQuit.colors;
		noCol.normalColor = Red;
		noQuit.colors = noCol;

		//Text

		exitMessage = exitMessage.GetComponent<Text> ();
		exitMessage.color = Blue;

		rTitle = rTitle.GetComponent<Text> ();
		rTitle.color = Red;

		gTitle = gTitle.GetComponent<Text> ();
		gTitle.color = Green;

		bTitle = bTitle.GetComponent<Text> ();
		bTitle.color = Blue;


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
		SceneManager.LoadScene (1);
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
