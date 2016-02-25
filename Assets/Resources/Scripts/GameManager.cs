using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class GameManager : MonoBehaviour {
	Board board;
	public SpriteRenderer background;
	public int level = 6;
	string levelFile;
	float transitionTimer = 0.0f;
	public float transitionTime = 0.15f;
	public float holdMovementTime = 0.35f;
	MoveCounter moveCounter;

	public enum FileSymbols {
		RedBlock = 'r',
		GreenBlock = 'g',
		BlueBlock = 'b',
		YellowBLock = 'y',
		MagentaBlock = 'm',
		CyanBlock = 'c',
		WhiteBlock= 'w',
		EmptyBlock = 'e',
		RedSwitch = 'R',
		GreenSwitch = 'G',
		BlueSwitch = 'B'
	};

	// Use this for initialization
	void Start () {
		moveCounter = GameObject.Find("MoveCounter").GetComponent<MoveCounter>();
		levelFile = "Assets/Resources/Levels/Level" + level + ".txt";
		background = Instantiate(Resources.Load<GameObject>("Prefabs/Background")).GetComponent<SpriteRenderer>();
		background.color = CustomColors.Green;
		GameObject boardObj = new GameObject();
		board = boardObj.AddComponent<Board>();
		loadLevelFromFile(levelFile, board);
	}

	// Update is called once per frame
	void Update() {
		if (board.bgTransitioning || board.getPlayer().moving) {
			transitionTimer += Time.deltaTime;
			if (board.bgTransitioning) {
				board.whileBGTransitioning (transitionTimer / transitionTime);
			} 
			if (board.getPlayer().moving){
				board.getPlayer().whileMoving (transitionTimer / transitionTime);
			}
			if (transitionTimer / transitionTime >= 1.0f) {
				transitionTimer = 0.0f;
			}
		}
		else {
			if (Input.GetKeyDown("1")) {
				board.setBackground(CustomColors.Red);
			}
			if (Input.GetKeyDown("2")) {
				board.setBackground(CustomColors.Green);
			}
			if (Input.GetKeyDown("3")) {
				board.setBackground(CustomColors.Blue);
			}
			if (Input.GetKeyDown("4")) {
				board.setBackground(CustomColors.Magenta);
			}
			if (Input.GetKeyDown("5")) {
				board.setBackground(CustomColors.Yellow);
			}
			if (Input.GetKeyDown("6")) {
				board.setBackground(CustomColors.Cyan);
			}
			bool moved = false;
			if (board.getPlayer().readyToMove()) {
				if (Input.GetKeyDown(KeyCode.UpArrow)) {
					moved = board.getPlayer().move(Vector2.up);
				}
				else if (Input.GetKeyDown(KeyCode.DownArrow)) {
					moved = board.getPlayer().move(Vector2.down);
				}
				else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
					moved = board.getPlayer().move(Vector2.left);
				}
				else if (Input.GetKeyDown(KeyCode.RightArrow)) {
					moved = board.getPlayer().move(Vector2.right);
				}
				else {
					moved = false;
				}
			}
			if (!moved && board.getPlayer().timeSinceLastMovement() >= holdMovementTime) {
				if (Input.GetKey(KeyCode.UpArrow)) {
					moved = board.getPlayer().move(Vector2.up);
				}
				else if (Input.GetKey(KeyCode.DownArrow)) {
					moved = board.getPlayer().move(Vector2.down);
				}
				else if (Input.GetKey(KeyCode.LeftArrow)) {
					moved = board.getPlayer().move(Vector2.left);
				}
				else if (Input.GetKey(KeyCode.RightArrow)) {
					moved = board.getPlayer().move(Vector2.right);
				}
				else {
					moved = false;
				}
			}
			if (moved) {
				moveCounter.increment();
			}
		}
	}

	public void loadLevelFromFile(string fileName, Board board) {
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
		}
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
