using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;

public class MenuManager : MonoBehaviour {

	public enum menus {levelSelect, pauseMenu, packMenu, ingameUI}
	GameObject[] menusArray; 
	//public bool inLevel = false;

	GameObject levelSelection, packSelection, pauseMenu, ingameUI;

	// Use this for initialization
	void Start () {
		menusArray = new GameObject[Enum.GetNames(typeof(menus)).Length];
		levelSelection = GameObject.Find ("Level Selection");
		pauseMenu = GameObject.Find ("PauseMenu");
		packSelection = GameObject.Find("LevelPackPanel");
		ingameUI = GameObject.Find("IngameUI");


		menusArray [(int)menus.levelSelect] = levelSelection;
		menusArray [(int)menus.pauseMenu] = pauseMenu;
		menusArray [(int)menus.packMenu] = packSelection;
		menusArray[(int)menus.ingameUI] = ingameUI;

		if (menusArray [(int)menus.levelSelect] == null) {
			print("Unable to find level selection");
		}
		if (menusArray [(int)menus.pauseMenu] == null) {
			print("Unable to find pause menu");
		}
		if (menusArray [(int)menus.packMenu] == null) {
			print("Unable to find pack selection");
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
			if (menusArray [i].activeSelf) {
				return false;
			}
		}
		return true;
	}

	public void closeMenu (int menu){
		//inLevel = true;
		menusArray [menu].SetActive (false);
	}

	public void openMenu (int menu){
		//inLevel = false;
		menusArray [menu].SetActive (true);
	}

	public void returnToStart(){
		SceneManager.LoadScene("startUpMenu");
	}
}
