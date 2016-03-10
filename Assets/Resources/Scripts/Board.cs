using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Board : MonoBehaviour {
	//AI specific changes
	List<Enemy> enemyList;

	int width, height;
	public Block[,] blocks;
	List<Block> solidBlocks;
	SpriteRenderer background;
	GameObject emptyBlockFolder, blockFolder, switchFolder, enemyFolder;
	public bool bgTransitioning = false;
	Color oldBG, newBG;
	Vector3 bgSize;
	Player player;
	Exit exit;
	float lastColorChange = -1.0f;
	public float boardSize = 2.0f;


	// Use this for initialization
	public void init(int w, int h, SpriteRenderer bgRender) {
		Vector3 dim = 1.6f * Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
		if (Screen.width < Screen.height) {
			boardSize = (float)dim.x / (float)w;
		}
		else {
			boardSize = (float)dim.y / (float)h;
		}
		Vector3 center = new Vector3((float)w / 2.0f - .5f, (float)h / 2.0f - .5f, 0) * boardSize;
		transform.localPosition = -center;
		transform.localScale = new Vector3(boardSize, boardSize, 1);

		width = w;
		height = h;
		blocks = new Block[w, h];

		background = bgRender;
		bgSize = new Vector3((float)width / 4f, (float)height / 4f, 1);
		background.transform.localScale = new Vector3(bgSize.x, bgSize.y, bgSize.z);
		background.transform.parent = transform;
		oldBG = background.color;
		newBG = background.color;


		name = "Board";
		emptyBlockFolder = new GameObject();
		emptyBlockFolder.name = "Empty Blocks";
		emptyBlockFolder.transform.parent = transform;
		emptyBlockFolder.transform.localPosition = new Vector3(0, 0, 0);
		emptyBlockFolder.transform.localScale = new Vector3(1, 1, 1);
		blockFolder = new GameObject();
		blockFolder.name = "Blocks";
		blockFolder.transform.parent = transform;
		blockFolder.transform.localPosition = new Vector3(0, 0, 0);
		blockFolder.transform.localScale = new Vector3(1, 1, 1);
		switchFolder = new GameObject();
		switchFolder.name = "Switches";
		switchFolder.transform.parent = transform;
		switchFolder.transform.localPosition = new Vector3(0, 0, 0);
		switchFolder.transform.localScale = new Vector3(1, 1, 1);
		enemyFolder = new GameObject();
		enemyFolder.name = "Enemy Folder";
		enemyFolder.transform.parent = transform;
		enemyFolder.transform.localScale = new Vector3(1, 1, 1);

		solidBlocks = new List<Block>();
		enemyList = new List<Enemy>();
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				addEmptyBlock(x, y);
			}
		}
		initPlayer();
		//Track_AI_List = new List<TrackerAI> ();
//		addTraversalAI();
		//addTrackerAI();
		initExit();
	}

	public void initPlayer() {
		player = Instantiate(Resources.Load<GameObject>("Prefabs/Player")).GetComponent<Player>();
		player.init(this);
	}

	public List<Enemy> getEnemyList() {
		return enemyList;
	}

	public bool checkLevelDone() {
		int x, y, oldX, oldY;
		x = player.getPos()[0];
		y = player.getPos()[1];
		oldX = player.getOldPos()[0];
		oldY = player.getOldPos()[1];
		if (player.moving) {
			return (exit.x == oldX && exit.y == oldY);
		}
		else {
			return (exit.x == x && exit.y == y);
		}
	}

	public void addHorizontalEnemy(int x, int y) {
		HorizontalEnemy enemy = Instantiate(Resources.Load<GameObject>("Prefabs/HorizontalEnemy")).GetComponent<HorizontalEnemy>();
		enemy.transform.parent = enemyFolder.transform;
		enemy.init(this, x, y);
		enemyList.Add(enemy);
		enemy.name = "Enemy" + enemyList.Count;
	}

	public void addVerticalEnemy(int x, int y) {
		VerticalEnemy enemy = Instantiate(Resources.Load<GameObject>("Prefabs/HorizontalEnemy")).GetComponent<VerticalEnemy>();
		enemy.transform.parent = enemyFolder.transform;
		enemy.init(this, x, y);
		enemyList.Add(enemy);
		enemy.name = "Enemy" + enemyList.Count;
	}

	public void killEnemy(Enemy enemy) {
		enemyList.Remove(enemy);
		enemy.onKill();
		Destroy(enemy.gameObject);
	}

	//check to see if it is on another stack of enemies 
//	public bool onStack(){
//
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
		if (onBoard(x, y)) {
			return blocks[x, y];
		}
		else {
			return null;
		}
	}

	public void setHasEnemy(int x, int y, Enemy enemy) {
		if (onBoard(x, y)) {
			blocks[x, y].setEnemy(enemy);
		}
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
		newBG = bgColor;
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

	public void setBackgroundCosmetic(Color bgColor) {
		background.color = bgColor;
		oldBG = bgColor;
		newBG = bgColor;
	}

	public void whileBGTransitioning(float t) {
		if (t >= 1) {
			t = 1.0f;
			player.onBackgroundTransition(oldBG, newBG, t);
			exit.onBackgroundTransition(oldBG, newBG, t);
			background.color = newBG;
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
			exit.onBackgroundTransition(oldBG, newBG, t);
		}
	}

	public void scaleComponents(float t) {
		foreach (Block b in blocks) {
			b.transform.localScale = new Vector3(t, t, 1);
		}
		if (player != null) {
			player.transform.localScale = new Vector3(player.size * t, player.size * t, 1);
		}
		foreach (Enemy ai in enemyList) {
			ai.transform.localScale = new Vector3(ai.size * t, ai.size * t, 1);
		}
		exit.transform.localScale = new Vector3(exit.size * t, exit.size * t, 1);
	}

	// Scale around origin
	public void scaleBackground(float t) {
		background.transform.localScale = Vector3.Lerp(Vector3.zero, bgSize, t);
	}

	public void whileLoading(float t) {
		scaleBackground(t);
		scaleComponents(t);
	}

	public void whileUnloading(float t) {
		float progress = 1 - t;
		scaleComponents(progress);
		scaleBackground(progress);
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

	public Player getPlayer() {
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
		int x = player.getPos()[0];
		int y = player.getPos()[1];

		return blocks[x, y].hasEnemy();


	}

	public void killPlayer() {
		Destroy(player.gameObject);

	}

	public float getDistanceBetweenBlocks() {
		return transform.localScale.x;
	}
}
