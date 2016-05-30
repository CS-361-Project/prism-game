using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;

public class LevelPackManager : MonoBehaviour {
	GameObject packPanel;
	GameObject levelPanel;
	GameObject levelContainer;
	GameObject activePack;
	public string currPack;
	ScrollRect levelScroll;
	GameManager gm;
	
	// Use this for initialization
	void Start() {
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		packPanel = GameObject.Find("LevelPackPanel");
		levelPanel = GameObject.Find("LevelSelection");
		levelContainer = GameObject.Find("LevelContainer");
		levelScroll = GameObject.Find("LevelScrollWindow").GetComponent<ScrollRect>();
		levelPanel.SetActive(false);
		initLevelPacks();
	}

	void initLevelPacks() {
		TextAsset packFile = Resources.Load<TextAsset>("Levels/LevelPacks");
		string[] directories = packFile.text.Split(new string[] { "\r\n", "\n" }, System.StringSplitOptions.None);
		foreach (string pack in directories) {
			addLevelPack(pack);
		}
	}

	void addLevelPack(string name) {
		GameObject buttonObj = Instantiate(Resources.Load<GameObject>("Prefabs/PackButton"));
		Button button = buttonObj.GetComponent<Button>();
		buttonObj.transform.SetParent(packPanel.transform, false);
		buttonObj.GetComponentInChildren<Text>().text = name;
		button.onClick.AddListener(() => setActivePack(name));
	}

	public void setActivePack(string name) {
		displayPack(name);
		gm.exitPackSelection();
		gm.goToLevelSelection();
		currPack = name;
	}

	public void displayPack(string name) {
		if (activePack != null) {
			print("Destroying active pack");
			Destroy(activePack);
		}
		print("Creating pack " + name);
		GameObject g = Instantiate(Resources.Load<GameObject>("Prefabs/LevelPanelContainer"));
		g.transform.SetParent(levelContainer.transform, false);
		g.GetComponent<LevelButtonManager>().init(name);
		activePack = g;
		levelScroll.content = g.GetComponent<RectTransform>();
		StartCoroutine(ResizeOnNextUpdate(g));
		currPack = name;
	}

	void setActivePack(GameObject obj) {
		if (activePack != null) {
			activePack.SetActive(false);
		}
		gm.exitPackSelection();
		gm.goToLevelSelection();
		obj.SetActive(true);
		activePack = obj;
		levelScroll.content = obj.GetComponent<RectTransform>();
		StartCoroutine(ResizeOnNextUpdate(obj));
	}

	public void destroyLevelSelectionPanel() {
		Destroy(activePack);
	}

	IEnumerator ResizeOnNextUpdate(GameObject obj) {
		yield return 0;
		RectTransform gt = obj.GetComponent<RectTransform>();
		gt.anchoredPosition = new Vector2(gt.rect.width / 2, 0);
	}

	public void showPackSelection() {
		if (activePack != null) {
			activePack.SetActive(false);
			gm.exitLevelSelection();
			activePack = null;
		}
		gm.openPackSelection ();
	}
}

