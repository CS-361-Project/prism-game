using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class LevelButtonManager : MonoBehaviour {
	Button[] buttons;
	GameObject levelPanel;
	int numLevels;
	// assign in the editor

	void Start() {
		numLevels = levelCount();
		buttons = new Button[numLevels];
		levelPanel = GameObject.Find("LevelPanel");
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
		gm.loadLevel(level);
	}

	int levelCount() {
		int i = 0;
		// Add file sizes.
		DirectoryInfo d = new DirectoryInfo("Assets/Resources/Levels");
		FileInfo[] fis = d.GetFiles();
		foreach (FileInfo fi in fis) {
			if (fi.Name.StartsWith("level") && fi.Extension.Equals(".txt")) {
				i++;
			}
		}
		return i;
	}

//	public void setPositionForButton(int i) {
//		RectTransform panelTransform = levelPanel.GetComponent<RectTransform>();
//		RectTransform buttonTransform = buttons[i].GetComponent<RectTransform>();
//		float x = -panelTransform.rect.width / 2 + 35 * (i % 6) + 10;
//		float y = panelTransform.rect.height / 2 - 35 * (1 + (Mathf.Floor(i / 6) % 6)) - 10;
//		buttonTransform.offsetMin = new Vector2(x, y);
//		buttonTransform.offsetMax = new Vector2(x + 35, y + 35);
//	}
}
