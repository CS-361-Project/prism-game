using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;

public class LevelPackManager : MonoBehaviour {
	GameObject packPanel;
	GameObject levelPanel;
	GameObject activePack;
	
	// Use this for initialization
	void Start() {
		packPanel = GameObject.Find("LevelPackPanel");
		levelPanel = GameObject.Find("Level Selection");
		levelPanel.SetActive(false);
		initLevelPacks();
	}

	void initLevelPacks() {
		TextAsset packFile = Resources.Load<TextAsset>("Levels/LevelPacks");
		string[] directories = packFile.text.Split(new string[] { "\r\n", "\n" }, System.StringSplitOptions.None);
		foreach (string pack in directories) {
			GameObject g = Instantiate(Resources.Load<GameObject>("Prefabs/LevelPanel"));
			g.SetActive(false);
			g.transform.SetParent(levelPanel.transform, false);
			g.GetComponent<LevelButtonManager>().init(pack);
			addLevelPack(g, pack);
		}
	}

	void addLevelPack(GameObject panel, string name) {
		GameObject buttonObj = Instantiate(Resources.Load<GameObject>("Prefabs/PackButton"));
		Button button = buttonObj.GetComponent<Button>();
		buttonObj.transform.SetParent(packPanel.transform, false);
		buttonObj.GetComponentInChildren<Text>().text = name;
		button.onClick.AddListener(() => setActivePack(panel));
	}

	void setActivePack(GameObject obj) {
		if (activePack != null) {
			activePack.SetActive(false);
		}
		else {
			packPanel.SetActive(false);
			levelPanel.SetActive(true);
		}
		obj.SetActive(true);
		activePack = obj;
	}

	public void showPackSelection() {
		if (activePack != null) {
			activePack.SetActive(false);
			levelPanel.SetActive(false);
			activePack = null;
		}
		packPanel.SetActive(true);
	}
}

