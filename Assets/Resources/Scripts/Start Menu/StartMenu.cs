using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class StartMenu : MonoBehaviour {

	public Camera main;

	public Button startButton;
	public Button exitButton;
	public Button infoButton;
	public Button yesQuit;
	public Button noQuit;

	public Text exitMessage;
	public Text rTitle;
	public Text gTitle;
	public Text bTitle;

	public Image infoPanel;

	public bool bgTransitioning = false;
	Color newBG, oldBG;
	float lastColorChange = -1.0f;
	public float transitionTime = 1.0f;

	public Text quitMessage;

	// Use this for initialization
	void Awake() {

		main = main.GetComponent<Camera>();
		main.backgroundColor = CustomColors.White;
		newBG = main.backgroundColor;
		oldBG = main.backgroundColor;

		//Buttons 
		setupStartButton();
		setupExitButton();
		setupInfoButton();

	
		// quit menu/buttons
		setupExitMessage();
		setupYesQuitButton();
		setupNoQuitButton();


		// title letters
		setupTitle();

		setupInfoPanel();
	
	}

	private void setupInfoPanel(){
		infoPanel = infoPanel.GetComponent<Image>();
		infoPanel.gameObject.SetActive(false);

	}

	private void setupStartButton(){
		startButton = startButton.GetComponent<Button>();
		ColorBlock startCol = startButton.colors;
		startCol.normalColor = CustomColors.Yellow;
		startCol.highlightedColor = Color.Lerp(CustomColors.Yellow, CustomColors.White, .5F);
		startButton.colors = startCol;
	}

	private void setupExitButton(){
		exitButton = exitButton.GetComponent<Button>();
		ColorBlock exitCol = exitButton.colors;
		exitCol.normalColor = CustomColors.Cyan;
		exitCol.highlightedColor = Color.Lerp(CustomColors.Cyan, CustomColors.White, .5F);
		exitButton.colors = exitCol;
	}

	private void setupInfoButton(){
		infoButton = infoButton.GetComponent<Button>();
		ColorBlock infoButtonCol = infoButton.colors;
		infoButtonCol.normalColor = CustomColors.Magenta;
		infoButtonCol.highlightedColor = Color.Lerp(CustomColors.Magenta, CustomColors.White, .5F);
		infoButton.colors = infoButtonCol;
	}

	private void setupExitMessage(){
		exitMessage = exitMessage.GetComponent<Text>();
		exitMessage.color = CustomColors.Blue;
		exitMessage.gameObject.SetActive (false);
	}

	private void setupYesQuitButton(){
		yesQuit = yesQuit.GetComponent<Button>();
		ColorBlock yesCol = yesQuit.colors;
		yesCol.normalColor = CustomColors.Green;
		yesCol.highlightedColor = Color.Lerp(CustomColors.Green, CustomColors.White, .5F);
		yesQuit.colors = yesCol;
		yesQuit.gameObject.SetActive (false);
	}

	private void setupNoQuitButton(){
		noQuit = noQuit.GetComponent<Button>();
		ColorBlock noCol = noQuit.colors;
		noCol.normalColor = CustomColors.Red;
		noCol.highlightedColor = Color.Lerp(CustomColors.Red, CustomColors.White, .5F);
		noQuit.colors = noCol;
		noQuit.gameObject.SetActive (false);
	}

	private void setupTitle(){
		rTitle = rTitle.GetComponent<Text>();
		rTitle.color = CustomColors.Red;

		gTitle = gTitle.GetComponent<Text>();
		gTitle.color = CustomColors.Green;

		bTitle = bTitle.GetComponent<Text>();
		bTitle.color = CustomColors.Blue;
	}

	void Update() {
		if (bgTransitioning) {
			whileBGTransitioning(timeSinceLastColorChange() / transitionTime);
		}
	}

	public void InfoPressed(){
		BackgroundChangeMagenta();
		toggleMainMenu(false);
		infoPanel.gameObject.SetActive(true);
	}

	public void infoReturnPressed(){
		BackgroundReset();
		infoPanel.gameObject.SetActive(false);
		toggleMainMenu(true);
	}

	public void ExitPressed() {
		BackGroundChangeBlack ();
		toggleExitMenu(true);
		toggleMainMenu(false);

	}

	public void NoPressed() {
		BackgroundReset();
		toggleExitMenu(false);
		toggleMainMenu(true);
	}

	private void toggleMainMenu(bool toggle){
		startButton.gameObject.SetActive (toggle);
		exitButton.gameObject.SetActive (toggle);
		infoButton.gameObject.SetActive (toggle);

		rTitle.gameObject.SetActive (toggle);
		gTitle.gameObject.SetActive (toggle);
		bTitle.gameObject.SetActive (toggle);
	}

	private void toggleExitMenu(bool toggle){
		exitMessage.gameObject.SetActive(toggle);
		yesQuit.gameObject.SetActive (toggle);
		noQuit.gameObject.SetActive (toggle);
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

	public void BackGroundChangeBlack(){
		if (bgTransitioning) {
			finishBGTransitionImmediate();
		}
		startBGTransition(CustomColors.Black);
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

	/*public Color getBackgroundColor() {
		return main.backgroundColor;
	}*/
		

}
