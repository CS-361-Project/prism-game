using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class GameManager : MonoBehaviour {
	Board board;
	public SpriteRenderer background;
	public float transitionTime = 0.15f;
	public float holdMovementTime = 0.35f;
	MoveCounter moveCounter;
	GameObject levelSelection, packSelection;
	bool inLevel = false;
	bool inLevelSelection = false;
	bool loadingLevel = false;
	float timeSinceLevelLoad = 0.0f;
	int currLevel = -1;
	string levelPack = "";

	//Sound Effects
	AudioSource audioSource;
	public AudioClip deathSound;

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
		BlueSwitch = 'B'}

	;

	// Use this for initialization
	void Start() {
		moveCounter = GameObject.Find("MoveCounter").GetComponent<MoveCounter>();
		moveCounter.gameObject.SetActive(false);
		levelSelection = GameObject.Find("Level Selection");
		if (levelSelection == null) {
			print("Unable to find level selection");
		}

		//Initialize AudioSource
		audioSource = gameObject.AddComponent<AudioSource>();
		deathSound = Resources.Load("Audio/death", typeof(AudioClip)) as AudioClip;
	}

	public bool loadLevel(String levelPack, int number) {
		this.levelPack = levelPack;
		currLevel = number;
		moveCounter.gameObject.SetActive(true);
		string levelFile = "Assets/Resources/Levels/" + levelPack + "/level" + number + ".txt";
		background = Instantiate(Resources.Load<GameObject>("Prefabs/Background")).GetComponent<SpriteRenderer>();
		background.color = CustomColors.Green;
		if (board != null) {
			DestroyImmediate(board.gameObject);
		}
		GameObject boardObj = new GameObject();
		board = boardObj.AddComponent<Board>();
		if (loadLevelFromFile(levelFile, board)) {
			exitLevelSelection();
			timeSinceLevelLoad = 0.0f;
			loadingLevel = true;
			if (number == 0) {
				board.addTraversalAI();
			}
			loadingLevel = true;
			return true;
		}
		else {
			goToLevelSelection();
			Destroy(boardObj);
			Destroy(background);
			return false;
		}
	}

	void goToLevelSelection() {
		inLevel = false;
		inLevelSelection = true;
		levelSelection.SetActive(true);
		moveCounter.gameObject.SetActive(false);
	}

	void exitLevelSelection() {
		inLevel = true;
		inLevelSelection = false;
		levelSelection.SetActive(false);
		moveCounter.gameObject.SetActive(true);
	}

	// Update is called once per frame
	void Update() {
		bool moved = false;
		if (Input.GetKeyDown(KeyCode.Escape)) {
			if (inLevel) {
				goToLevelSelection();
			}
			else if (board != null) {
				exitLevelSelection();
			}
		}
		if (inLevel) {
			if (board.getPlayer() == null) {
				// player is dead
				audioSource.PlayOneShot(deathSound);
				restartLevel();
			}
			if (loadingLevel) {
				timeSinceLevelLoad += Time.deltaTime;
				whileLoading(timeSinceLevelLoad);
			}
			else if (board.checkLevelDone()) {
				nextLevel();
			}
			else if (board.checkIfKillPlayer()) {
				board.killPlayer();
			}
			else if (board.bgTransitioning || board.getPlayer().animating) {
				Vector2 dir = getKeyPressDirection();
				if (board.getPlayer().animating) {
					if (dir != Vector2.zero) {
						board.getPlayer().finishMovementImmedate();
						//Added AI
						List<TraversalAI> AI_list = board.getTraversalAIList();
						if (board.getPlayer().moving) {
							foreach (TraversalAI x in AI_list) {
								x.finishMovementImmedate();
							}
						}
						if (board.bgTransitioning) {
							board.finishBGTransitionImmediate();
						}
						if (board.getPlayer().move(dir)) {
							moveCounter.increment();
							foreach (TraversalAI x in AI_list) {
								x.move();
							}
						}
					}
					else {
						float t = board.getPlayer().timeSinceLastMovement() / transitionTime;
						//Added AI
						List<TraversalAI> AI_list = board.getTraversalAIList();
						if (board.getPlayer().moving) {
							foreach (TraversalAI x in AI_list) {
								x.whileMoving(t);
							}
						}
						board.getPlayer().whileMoving(t);
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
				List<TraversalAI> AIList = board.getTraversalAIList();
				for (int i=AIList.Count - 1; i>= 0; i--) {
					TraversalAI x = AIList[i];
					x.move();
					if (x.markedForDeath) {
						board.killEnemy(x);
					}
				}
			}
		}
	}

	public void whileLoading(float t) {
		if (t >= holdMovementTime) {
			loadingLevel = false;
		}
		// TODO: Load level animation
	}

	public Vector2 getKeyPressDirection() {
		Vector2 result;
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
		else {
			result = Vector2.zero;
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
			StreamReader file = new StreamReader(fileName);
			int lineNumber = 0;
			int width = 0;
			int height = 0;
			Color bgColor = Color.black;
			using (file) {
				string line;
				while ((line = file.ReadLine()) != null) {
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
				}
				background.transform.localScale = new Vector3((float)width / 4f, (float)height / 4f, 1);
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
		}
	}
}
