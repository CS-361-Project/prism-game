using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;

public class MenuManager : MonoBehaviour {

	public enum menus {levelSelect, pauseMenu, packMenu, ingameUI, backgroundBlocks}
	GameObject[] menusArray; 
	LevelNumber number;
	LevelPackName packName;
	//public bool inLevel = false;

	GameObject levelSelection, packSelection, pauseMenu, ingameUI, backgroundBlocks;

	// Use this for initialization
	void Start () {
		menusArray = new GameObject[Enum.GetNames(typeof(menus)).Length];
		levelSelection = GameObject.Find ("LevelSelection");
		pauseMenu = GameObject.Find ("PauseMenu");
		packSelection = GameObject.Find("LevelPackScroll");
		ingameUI = GameObject.Find("IngameUI");
		backgroundBlocks = GameObject.Find ("Background Blocks");
		number = GameObject.Find("LevelNumber").GetComponent<LevelNumber>();
		packName = GameObject.Find("LevelPackName").GetComponent<LevelPackName>();


		menusArray [(int)menus.levelSelect] = levelSelection;
		menusArray [(int)menus.pauseMenu] = pauseMenu;
		menusArray [(int)menus.packMenu] = packSelection;
		menusArray[(int)menus.ingameUI] = ingameUI;
		menusArray [(int)menus.backgroundBlocks] = backgroundBlocks;

		if (menusArray [(int)menus.levelSelect] == null) {
			print("Unable to find level selection");
		}
		if (menusArray [(int)menus.pauseMenu] == null) {
			print("Unable to find pause menu");
		}
		if (menusArray [(int)menus.packMenu] == null) {
			print("Unable to find pack selection");
		}
		if (menusArray [(int)menus.backgroundBlocks] == null) {
			print("Unable to find background blocks");
		}
		else {
			menusArray [(int)menus.pauseMenu].SetActive(false);
		}

	}

	public bool menuOpen (int menu){
		return menusArray [menu].activeSelf;
	}
		
	public bool inLevel (){
		for (int i = 0; i < menusArray.Length - 1; i++) {
			if (menusArray [i].activeSelf && menusArray[i]!=ingameUI) {
				return false;
			}
		}
		return true;
	}

	public void closeMenu (int menu){
		menusArray [menu].SetActive (false);
	}

	public void openMenu (int menu){
		menusArray [menu].SetActive (true);
	}

	public void returnToStart(){
		SceneManager.LoadScene("startUpMenu");
	}

	public void pauseMenuButton(){
		openMenu((int)menus.pauseMenu);
		openMenu((int)menus.backgroundBlocks);
		closeMenu((int)menus.ingameUI);
	}

	public void updateLevelInUI(string levelPack, int levelNumber) {
		number.setLevel(levelNumber);
		packName.setLevelPack(levelPack);
	}
}
