using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Board : MonoBehaviour {
	//AI specific changes
	List<TraversalAI> Ai_List;
	//List<TrackerAI> Track_AI_List;

	int width, height;
	public Block[,] blocks;
	List<Block> solidBlocks;
	SpriteRenderer background;
	GameObject emptyBlockFolder, blockFolder, switchFolder, enemyFolder;
	public bool bgTransitioning = false;
	Color oldBG, newBG;
	PlayerMovement player;
	Exit exit;
	float lastColorChange = -1.0f;
	// Use this for initialization
	public void init(int w, int h, SpriteRenderer bgRender) {
		Vector3 center = new Vector3((float)w / 2.0f - .5f, (float)h / 2.0f - .5f, 0);
		transform.position = -center;
		background = bgRender;
		bgRender.transform.parent = transform;
		oldBG = background.color;
		newBG = background.color;
		width = w;
		height = h;
		blocks = new Block[w, h];

		name = "Board";
		emptyBlockFolder = new GameObject();
		emptyBlockFolder.name = "Empty Blocks";
		emptyBlockFolder.transform.parent = transform;
		emptyBlockFolder.transform.localPosition = new Vector3(0, 0, 0);
		blockFolder = new GameObject();
		blockFolder.name = "Blocks";
		blockFolder.transform.parent = transform;
		blockFolder.transform.localPosition = new Vector3(0, 0, 0);
		switchFolder = new GameObject();
		switchFolder.name = "Switches";
		switchFolder.transform.parent = transform;
		switchFolder.transform.localPosition = new Vector3(0, 0, 0);
		enemyFolder = new GameObject();
		enemyFolder.name = "AI Folder";
		enemyFolder.transform.parent = transform;

		solidBlocks = new List<Block>();
		Ai_List = new List<TraversalAI>();
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				addEmptyBlock(x, y);
			}
		}
		initPlayer();
		//Track_AI_List = new List<TrackerAI> ();
		addTraversalAI();
		//addTrackerAI();
		initExit();
	}

	public void initPlayer() {
		player = Instantiate(Resources.Load<GameObject>("Prefabs/Player")).GetComponent<PlayerMovement>();
		player.init(this);
	}

	public List<TraversalAI> get_TravAI() {

		return Ai_List;
	}



	//AI specific functions
	void addTraversalAI() {
		TraversalAI enemy;
		enemy = Instantiate(Resources.Load<GameObject>("Prefabs/Traversal AI")).GetComponent<TraversalAI>();

		enemy.transform.parent = enemyFolder.transform;

		enemy.init(this);
		Ai_List.Add(enemy);
		enemy.name = "Traversal AI " + Ai_List.Count;

	}

	//	void addTrackerAI(){
	//		TrackerAI enemy;
	//		enemy = Instantiate(Resources.Load<GameObject>("Prefabs/Tracker AI")).GetComponent<TrackerAI>();
	//
	//		enemy.transform.parent = AIFolder.transform;
	//
	//		enemy.init (board, player);
	//
	//		Track_AI_List.Add (enemy);
	//		enemy.name = "Tracker AI " + Track_AI_List.Count;
	//
	//	}

	public void initExit() {
		exit = Instantiate(Resources.Load<GameObject>("Prefabs/Exit")).GetComponent<Exit>();
		exit.init(this);
	}

	public void addLever(int x, int y, Color c) {
		LeverBlock b;
		if (blocks[x, y] == null) {
			GameObject obj = new GameObject();
			b = obj.AddComponent<LeverBlock>();
			b.init(c, background.color, this, switchFolder.transform);
			b.transform.localPosition = new Vector3(x, y, 0);
			blocks[x, y] = b;
		}
		else {
			GameObject obj = blocks[x, y].gameObject;
			DestroyImmediate(blocks[x, y]);
			blocks[x, y] = obj.AddComponent<LeverBlock>();
			blocks[x, y].init(c, background.color, this, switchFolder.transform);
		}
	}

	public void addBlock(int x, int y, Color c) {
		if (blocks[x, y] == null) {
			GameObject obj = new GameObject();
			blocks[x, y] = obj.AddComponent<Block>();
			blocks[x, y].init(c, background.color, this, blockFolder.transform);
			blocks[x, y].transform.localPosition = new Vector3(x, y, 0);
		}
		else {
			GameObject obj = blocks[x, y].gameObject;
			DestroyImmediate(blocks[x, y]);
			blocks[x, y] = obj.AddComponent<Block>();
			blocks[x, y].init(c, background.color, this, blockFolder.transform);
		}
		solidBlocks.Add(blocks[x, y]);
	}

	public void addEmptyBlock(int x, int y) {
		if (blocks[x, y] == null) {
			GameObject obj = new GameObject();
			EmptyBlock b = obj.AddComponent<EmptyBlock>();
			b.init(background.color, background.color, this, emptyBlockFolder.transform);
			b.transform.localPosition = new Vector3(x, y, 0);
			blocks[x, y] = b;
		}
		else {
			GameObject obj = blocks[x, y].gameObject;
			DestroyImmediate(blocks[x, y]);
			blocks[x, y] = obj.AddComponent<EmptyBlock>();
			blocks[x, y].init(background.color, background.color, this, emptyBlockFolder.transform);
		}
	}

	public void moveExit(int x, int y) {
		exit.moveTo(x, y);
	}

	public Block getBlock(int x, int y) {
		return blocks[x, y];
	}

	public Vector3 getBlockPosition(int x, int y) {
		return blocks[x, y].transform.position;
	}

	public bool getBlockPassable(int x, int y) {
		if (onBoard(x, y)) {
			return blocks[x, y].isPassable();
		}
		else {
			return false;
		}
	}

	public void onBackgroundChange() {
		foreach (Block b in blocks) {
			if (b != null) {
				b.onBackgroundChange(background.color);
			}
		}
	}

	public void setBackground(Color bgColor) {
		background.color = bgColor;
		oldBG = bgColor;
		lastColorChange = Time.time;
		onBackgroundChange();

	}

	public void startBGTransition(Color bgColor) {
		newBG = bgColor;
		bgTransitioning = true;
		lastColorChange = Time.time;
	}

	public void finishBGTransitionImmediate() {
		whileBGTransitioning(1.0f);
	}

	public void whileBGTransitioning(float t) {
		if (t >= 1) {
			background.color = newBG;
			player.onBackgroundTransition(oldBG, newBG, t);
			foreach (Block b in blocks) {
				b.onBackgroundChange(newBG);
			}
			oldBG = newBG;
			bgTransitioning = false;
		}
		else {
			background.color = Color.Lerp(oldBG, newBG, t);
			foreach (Block b in solidBlocks) {
				b.onBGTransition(oldBG, newBG, t);
			}
			player.onBackgroundTransition(oldBG, newBG, t);
		}
	}

	public float timeSinceLastColorChange() {
		return Time.time - lastColorChange;
	}

	public Color getBackgroundColor() {
		return background.color;
	}

	public int getWidth() {
		return width;
	}

	public int getHeight() {
		return height;
	}

	public PlayerMovement getPlayer() {
		return player;
	}

	public Color nextBGColor() {
		return newBG;
	}

	bool passableWithNewBG(int x, int y) {
		if (onBoard(x, y)) {
			return blocks[x, y].passableWithBG(newBG);
		}
		else {
			return false;
		}
	}

	public bool onBoard(int x, int y) {
		return (x >= 0 && x < width) && (y >= 0 && y < height);
	}

	public bool getBlockPassableAfterTransition(int x, int y) {
		if (bgTransitioning) {
			return passableWithNewBG(x, y);
		}
		else {
			return getBlockPassable(x, y);
		}

	}

	//checks if the player has moved onto a block that has an AI and kills the player
	public bool checkIfKillPlayer() {
		//find out where the player is moving to
		int x = player.getPlayerDestination()[0];
		int y = player.getPlayerDestination()[1];

		return blocks[x, y].hasEnemy;


	}

	public void killPlayer() {
		Destroy(player.gameObject);
	}
}
