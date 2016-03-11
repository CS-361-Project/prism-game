using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;

public class LevelPackManager : MonoBehaviour {
	GameObject packPanel;
	GameObject levelPanel;
	GameObject levelContainer;
	GameObject activePack;
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
			GameObject g = Instantiate(Resources.Load<GameObject>("Prefabs/LevelPanelContainer"));
			g.transform.SetParent(levelContainer.transform, false);

			g.GetComponent<LevelButtonManager>().init(pack);
			addLevelPack(g, pack);
			g.SetActive(false);
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
			//packPanel.SetActive(false);
			gm.exitPackSelection();
			gm.goToLevelSelection();
			//levelPanel.SetActive(true);
		}
		obj.SetActive(true);
		activePack = obj;
		levelScroll.content = obj.GetComponent<RectTransform>();
		StartCoroutine(ResizeOnNextUpdate(obj));
	}

	IEnumerator ResizeOnNextUpdate(GameObject obj) {
		yield return 0;
		RectTransform gt = obj.GetComponent<RectTransform>();
		print("Size: " + gt.sizeDelta);
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

