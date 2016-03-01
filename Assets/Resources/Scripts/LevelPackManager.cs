using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;

public class LevelPackManager : MonoBehaviour {
	GameObject packPanel;
	GameObject activePack;
	
	// Use this for initialization
	void Start() {
		packPanel = GameObject.Find("LevelPackPanel");
		initLevelPacks();
	}

	void initLevelPacks() {
		string[] directories = Directory.GetDirectories("Assets/Resources/Levels/");
		DirectoryInfo[] dis = new DirectoryInfo[directories.Length];
		for (int i=0; i<directories.Length; i++) {
			dis[i] = new DirectoryInfo("Assets/Resources/Levels/" + directories[i]);
		}
		foreach (DirectoryInfo di in dis) {
			GameObject g = Instantiate(Resources.Load<GameObject>("Prefabs/LevelPanel"));
			g.SetActive(false);
			g.transform.SetParent(packPanel.transform.parent, false);
			g.GetComponent<LevelButtonManager>().init(di.Name);
			addLevelPack(g, di.Name);
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
		}
		obj.SetActive(true);
		activePack = obj;
	}

	public void showPackSelection() {
		if (activePack != null) {
			activePack.SetActive(false);
			activePack = null;
		}
		packPanel.SetActive(true);
	}
}

