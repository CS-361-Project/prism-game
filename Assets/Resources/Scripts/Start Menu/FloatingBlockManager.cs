using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FloatingBlockManager : MonoBehaviour {


	List<FloatingBlock> floatingBlocks;
	GameObject blockFolder;

	// Use this for initialization
	void Start () {

		Vector3 center = new Vector3 (0F, 0F, 0F);
		transform.localPosition = center;
		blockFolder = new GameObject();
		blockFolder.name = "Blocks";
		blockFolder.transform.parent = transform;
		blockFolder.transform.localPosition = new Vector3(0, 0, 0);
		floatingBlocks = new List<FloatingBlock>();
		for (int i = 0; i < Camera.main.orthographicSize*4; i++) {
			Vector3 screen = Camera.main.ScreenToWorldPoint (new Vector3 (Random.Range (-Screen.width, Screen.width), Random.Range (-Screen.height, Screen.height), 0));
			addBlock (screen.x, screen.y, CustomColors.randomColor ());
		}
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void addBlock(float x, float y, Color c) {
		
		/*GameObject obj = new GameObject();
		FloatingBlock block = obj.AddComponent<FloatingBlock>();
		block.init(c, this, blockFolder.transform, Random.Range(-.005F,.005F));
		block.transform.localPosition = new Vector3(x, y, 0F);
		float randy = Random.Range (.5F, 1.5F);
		block.transform.localScale = new Vector3 (randy, randy, 0);


		floatingBlocks.Add(block);*/
	}
}
