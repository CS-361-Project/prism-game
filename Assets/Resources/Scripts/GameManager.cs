using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class GameManager : MonoBehaviour {
	Board board, lastBoard;
	public SpriteRenderer background;
	public float transitionTime = 0.15f;
	public float holdMovementTime = 0.35f;
	MoveCounter moveCounter;
	SwipeDetector swipeDetector;
	MenuManager menuManager; 
	bool loadingLevel = false;
	float timeSinceLevelLoad = 0.0f;
	int currLevel = -1;
	string levelPack = "";

	//Sound Effects
	AudioSource audioSource;
	public AudioClip deathSound, endLevelSound, soundtrack;

	public enum FileSymbols {
		RedBlock = 'r',
		GreenBlock = 'g',
		BlueBlock = 'b',
		YellowBLock = 'y',
		MagentaBlock = 'm',
		CyanBlock = 'c',
		WhiteBlock = 'w',
		EmptyBlock = 'e',
		RedSwitch = 'R',
		GreenSwitch = 'G',
		BlueSwitch = 'B',
		HorizontalEnemy = 'h',
		VerticalEnemy = 'v'}

	;


	// Use this for initialization
	void Start() {
		moveCounter = GameObject.Find("MoveCounter").GetComponent<MoveCounter>();
		swipeDetector = new GameObject().AddComponent<SwipeDetector>();
		menuManager = GameObject.Find ("Menu Manager").GetComponent<MenuManager>();
		//menuManager.closeMenu((int)MenuManager.menus.ingameUI);

		//Initialize AudioSource
		audioSource = gameObject.AddComponent<AudioSource>();
		deathSound = Resources.Load("Audio/death", typeof(AudioClip)) as AudioClip;
		endLevelSound = Resources.Load<AudioClip>("Audio/Home2");

	}

	public bool loadLevel(String levelPack, int number) {
		this.levelPack = levelPack;
		currLevel = number;
		//menuManager.openMenu((int)MenuManager.menus.ingameUI);
		string levelFile = "Levels/" + levelPack + "/level" + number;
		background = Instantiate(Resources.Load<GameObject>("Prefabs/Background")).GetComponent<SpriteRenderer>();
		background.color = CustomColors.Green;
		lastBoard = board;
		GameObject boardObj = new GameObject();
		board = boardObj.AddComponent<Board>();
		if (loadLevelFromFile(levelFile, board)) {
			exitLevelSelection();
			timeSinceLevelLoad = 0.0f;
			loadingLevel = true;
			board.scaleComponents(0);
			board.scaleBackground(0);
			return true;
		}
		else {
			Destroy(background.gameObject);
			Destroy(boardObj);
			Destroy(lastBoard.gameObject);
			goToLevelSelection();
			return false;
		}
	}

	public void goToLevelSelection() {
		menuManager.openMenu((int)(MenuManager.menus.levelSelect));
		moveCounter.gameObject.SetActive(false);
	}

	public void exitLevelSelection() {
		menuManager.closeMenu ((int)MenuManager.menus.levelSelect);
		moveCounter.gameObject.SetActive(true);
	}

	public void openPauseMenu() {
		menuManager.openMenu ((int)MenuManager.menus.pauseMenu);
	}

	public void exitPauseMenu() {
		menuManager.closeMenu ((int)MenuManager.menus.pauseMenu);
	}

	public void exitPackSelection(){
		menuManager.closeMenu ((int)MenuManager.menus.packMenu);
		moveCounter.gameObject.SetActive(true);
	}

	public void openPackSelection(){
		menuManager.openMenu ((int)MenuManager.menus.packMenu);
		moveCounter.gameObject.SetActive(false);
	}

	// Update is called once per frame
	void Update() {
		bool moved = false;
		if (Input.GetKeyDown(KeyCode.Escape)) {
			if (menuManager.inLevel()) {
				openPauseMenu();
			}
			else if (board != null) {
				exitPauseMenu();
			}
		}
		if (menuManager.inLevel()) {
			if (board.getPlayer() == null) {
				// player is dead
				audioSource.PlayOneShot(deathSound);
				restartLevel();
			}
			if (loadingLevel) {
				timeSinceLevelLoad += Time.deltaTime;
				whileLoading(timeSinceLevelLoad);
			}
			else if (Input.GetKeyDown("r")) {
				restartLevel();
			}
			else if (board.checkLevelDone()) {
				audioSource.PlayOneShot(endLevelSound, .05f);
				nextLevel();
			}
			else if (board.checkIfKillPlayer()) {
				board.killPlayer();
			}
			else if (board.bgTransitioning || board.getPlayer().animating) {
				Vector2 dir = swipeDetector.getSwipeDirection();
				if (dir == Vector2.zero) {
					dir = getKeyPressDirection();
				}
				if (board.getPlayer().animating) {
					if (dir != Vector2.zero) {
						board.getPlayer().finishMovementImmedate();
						//Added AI
						List<Enemy> EnemyList = board.getEnemyList();
						if (board.getPlayer().moving) {
							foreach (Enemy x in EnemyList) {
								x.finishMovementImmedate();
							}
						}
						if (board.bgTransitioning) {
							board.finishBGTransitionImmediate();
						}
						if (board.getPlayer().move(dir)) {
							moveCounter.increment();
							foreach (Enemy x in EnemyList) {
								x.move(board.getPlayer().lastMovementTime());
							}
						}
					}
					else {
//						float t = board.getPlayer().timeSinceLastMovement() / transitionTime;
						//Added AI
						List<Enemy> EnemyList = board.getEnemyList();
						foreach (Enemy x in EnemyList) {
							if (x.moving) {
								float enemyProgress = x.timeSinceLastMovement() / transitionTime;
								x.whileMoving(enemyProgress);
							}
						}
						float playerProgress = board.getPlayer().timeSinceLastMovement() / transitionTime;
						board.getPlayer().whileMoving(playerProgress);
					}
				}
				if (board.bgTransitioning) {
					board.whileBGTransitioning(board.timeSinceLastColorChange() / transitionTime);
				}
			}
			else {
				Vector2 dir1 = getKeyPressDirection();
				if (dir1 != Vector2.zero) {
					moved = board.getPlayer().move(dir1);
					if (moved) {
						moveCounter.increment();
					}
				}
				else {
					Vector2 dir2 = getKeyHoldDirection();
					if (board.getPlayer().timeSinceLastMovement() >= holdMovementTime &&
					    dir2 != Vector2.zero) {
						moved = board.getPlayer().move(dir2);
						if (moved) {
							moveCounter.increment();
						}
					}
				}
			}
			//Check if Player moved
			if (moved) {
				List<Enemy> EnemyList = board.getEnemyList();
				for (int i = EnemyList.Count - 1; i >= 0; i--) {
					Enemy x = EnemyList[i];
					x.move(board.getPlayer().lastMovementTime());
					if (x.markedForDeath) {
						board.killEnemy(x);
					}
				}
			}
		}
	}

	public void whileLoading(float t) {
		if (lastBoard == null) {
			if (t < transitionTime) {
				board.whileLoading(t / transitionTime);
			}
			else {
				board.whileLoading(1);
				loadingLevel = false;
			}
		}
		else if (t < transitionTime) {
			lastBoard.whileUnloading(t / transitionTime);
		}
		else if (t < 2 * transitionTime) {
			float progress = (t - transitionTime) / transitionTime;
			board.whileLoading(progress);
		}
		else {
			if (lastBoard != null) {
				Destroy(lastBoard.gameObject);
			}
			board.whileLoading(1);
			loadingLevel = false;
		}
	}

	public Vector2 getKeyPressDirection() {
		Vector2 result = Vector2.zero;
		if (Input.GetKeyDown(KeyCode.UpArrow)) {
			result = Vector2.up;
		}
		else if (Input.GetKeyDown(KeyCode.DownArrow)) {
			result = Vector2.down;
		}
		else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
			result = Vector2.left;
		}
		else if (Input.GetKeyDown(KeyCode.RightArrow)) {
			result = Vector2.right;
		}
		return result;
	}

	public Vector2 getKeyHoldDirection() {
		Vector2 result;
		if (Input.GetKey(KeyCode.UpArrow)) {
			result = Vector2.up;
		}
		else if (Input.GetKey(KeyCode.DownArrow)) {
			result = Vector2.down;
		}
		else if (Input.GetKey(KeyCode.LeftArrow)) {
			result = Vector2.left;
		}
		else if (Input.GetKey(KeyCode.RightArrow)) {
			result = Vector2.right;
		}
		else {
			result = Vector2.zero;
		}
		return result;
	}

	public void restartLevel() {
		loadLevel(levelPack, currLevel);
	}

	public void nextLevel() {
		loadLevel(levelPack, currLevel + 1);
	}

	public bool loadLevelFromFile(string fileName, Board board) {
		try {
			TextAsset file = Resources.Load<TextAsset>(fileName);
			string[] lines = file.text.Split("\n"[0]);
			int lineNumber = 0;
			int width = 0;
			int height = 0;
			Color bgColor = Color.black;
			foreach (string line in lines) {
				string[] words = line.Split(' ');
				if (words.Length > 0) {
					if (lineNumber == 0) {
						if (words.Length >= 2) {
							width = int.Parse(words[0]);
							height = int.Parse(words[1]);
							board.init(width, height, background);
						}
						else {
							print("Error reading file.");
							return false;
						}
					}
					else if (lineNumber == 1) {
						int colorNumber = int.Parse(words[0]);
						bgColor = CustomColors.colors[colorNumber];
					}
					else if (lineNumber < height + 2) {
						if (words.Length >= width) {
							for (int i = 0; i < width; i++) {
								int x = (height - 1) - (lineNumber - 2);
								int y = i;
								char c = words[i].ToCharArray()[0];
								addBlock(x, y, c);
							}
						}
						else {
							print("Error reading file...");
							return false;
						}
					}
					lineNumber++;
				}
				board.setBackground(bgColor);
				moveCounter.reset();
			}
		}
		catch (Exception e) {
			print("Error reading file: " + e.ToString());
			print(e.StackTrace);
			return false;
		}
		return true;
	}

	void addBlock(int y, int x, char c) {
		switch (c) {
			case (char)FileSymbols.EmptyBlock:
				board.addEmptyBlock(x, y);
				break;
			case (char)FileSymbols.RedBlock:
				board.addBlock(x, y, CustomColors.Red);
				break;
			case (char)FileSymbols.GreenBlock:
				board.addBlock(x, y, CustomColors.Green);
				break;
			case (char)FileSymbols.BlueBlock:
				board.addBlock(x, y, CustomColors.Blue);
				break;
			case (char)FileSymbols.MagentaBlock:
				board.addBlock(x, y, CustomColors.Magenta);
				break;
			case (char)FileSymbols.YellowBLock:
				board.addBlock(x, y, CustomColors.Yellow);
				break;
			case (char)FileSymbols.CyanBlock:
				board.addBlock(x, y, CustomColors.Cyan);
				break;
			case (char)FileSymbols.WhiteBlock:
				board.addBlock(x, y, CustomColors.White);
				break;
			case (char)FileSymbols.RedSwitch:
				board.addLever(x, y, CustomColors.Red);
				break;
			case (char)FileSymbols.GreenSwitch:
				board.addLever(x, y, CustomColors.Green);
				break;
			case (char)FileSymbols.BlueSwitch:
				board.addLever(x, y, CustomColors.Blue);
				break;
			case (char)FileSymbols.HorizontalEnemy:
				board.addEmptyBlock(x, y);
				board.addHorizontalEnemy(x, y);
				break;
			case (char)FileSymbols.VerticalEnemy:
				board.addEmptyBlock(x, y);
				board.addVerticalEnemy(x, y);
				break;
		}
	}
}
