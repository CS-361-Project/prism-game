using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class LevelButtonManager : MonoBehaviour {
	List<Button> buttons;
	GameObject levelPanel;
	int numLevels;
	string packName;
	// assign in the editor

	public void init(string levelPackName) {
		packName = levelPackName;
		levelPanel = gameObject;
		buttons = new List<Button>();
		int i = 0;
		GameObject currPanel = Instantiate(Resources.Load<GameObject>("Prefabs/LevelPanel"));
		currPanel.transform.SetParent(levelPanel.transform, false);
		while (Resources.Load<TextAsset>("Levels/" + levelPackName + "/level" + i) != null) {
			if (i % 25 == 0 && i > 0) {
				currPanel = Instantiate(Resources.Load<GameObject>("Prefabs/LevelPanel"));
				currPanel.transform.SetParent(levelPanel.transform, false);
			}
			GameObject buttonObj = Instantiate(Resources.Load<GameObject>("Prefabs/Button"));
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
