using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class LevelButtonManager : MonoBehaviour {
	Button[] buttons;
	GameObject levelPanel;
	int numLevels;
	string packName;
	// assign in the editor

	public void init(string levelPackName) {
		packName = levelPackName;
		levelPanel = gameObject;
		numLevels = levelCount();
		buttons = new Button[numLevels];
		for (int i = 0; i < numLevels; i++) {
			GameObject button = Instantiate(Resources.Load<GameObject>("Prefabs/Button"));
			button.transform.SetParent(levelPanel.transform, false);

			buttons[i] = button.GetComponent<Button>();
			buttons[i].gameObject.GetComponentInChildren<Text>().text = i.ToString();
			int d = i;
			buttons[i].onClick.AddListener(() => OnSelect(d));
//			setPositionForButton(i);
		}
	}

	public void OnSelect(int i) {
		GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		GameObject levelSelectUI = GameObject.Find("Level Selection");
		levelSelectUI.SetActive(false);
		int level = int.Parse(buttons[i].GetComponentInChildren<Text>().text);
		gm.loadLevel(packName, level);
	}

	int levelCount() {
		int i = 0;
		// Add file sizes.
		DirectoryInfo d = new DirectoryInfo("Assets/Resources/Levels/" + packName);
		FileInfo[] fis = d.GetFiles();
		foreach (FileInfo fi in fis) {
			if (fi.Name.StartsWith("level") && fi.Extension.Equals(".txt")) {
				i++;
			}
		}
		return i;
	}
}
