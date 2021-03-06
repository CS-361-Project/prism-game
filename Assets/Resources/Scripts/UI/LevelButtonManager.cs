﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class LevelButtonManager : MonoBehaviour {
	List<Button> buttons;
	GameObject levelPanel;
	int numLevels;
	string packName;
	GameData data;
	Outline outline;
	// assign in the editor

	public void init(string levelPackName) {
		data= GameObject.Find("GameData").GetComponent<GameData>();
		packName = levelPackName;
		levelPanel = gameObject;
		buttons = new List<Button>();
		int i = 0;
		GameObject currPanel = Instantiate(Resources.Load<GameObject>("Prefabs/LevelPanel"));
		currPanel.transform.SetParent(levelPanel.transform, false);
		if (Resources.Load<TextAsset>("Levels/" + levelPackName + "/level0") == null) {
			i = 1;
		}
		while (Resources.Load<TextAsset>("Levels/" + levelPackName + "/level" + i) != null) {
			if (i % 25 == 0 && i > 0) {
				currPanel = Instantiate(Resources.Load<GameObject>("Prefabs/LevelPanel"));
				currPanel.transform.SetParent(levelPanel.transform, false);
			}
			GameObject buttonObj = Instantiate(Resources.Load<GameObject>("Prefabs/Button"));
			outline = buttonObj.GetComponent<Outline>();
			if (data.getLevelStatus(packName, i) == 1) {
				outline.effectColor = CustomColors.HexToColor("003366");
			}
			else
			if (data.getLevelStatus(packName, i) == 2) {
				outline.effectColor = CustomColors.HexToColor("ffbf00");
			}
			buttonObj.transform.SetParent(currPanel.transform, false);
			Button button = buttonObj.GetComponent<Button>();
			buttons.Add(button);
			button.gameObject.GetComponentInChildren<Text>().text = i.ToString();
			int d = i;
			button.onClick.AddListener(() => OnSelect(d));
			i++;
		}
	}

	public void OnSelect(int i) {
		GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		gm.exitLevelSelection ();
		int level = int.Parse(buttons[i].GetComponentInChildren<Text>().text);
		gm.loadLevel(packName, level);
	}
}
