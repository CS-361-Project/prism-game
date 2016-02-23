using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class GameManager : MonoBehaviour {
	Board board;
	public SpriteRenderer background;
	PlayerMovement player;
	const string levelFile = "Assets/Resources/Levels/Level1.txt";

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

	public static class CustomColors {
		public static Color Red = Color.red * .8f;
		public static Color Green = Color.green * .8f;
		public static Color Blue = Color.blue * .8f;
		public static Color Yellow = Color.yellow * .8f;
		public static Color Magenta = Color.magenta * .8f;
		public static Color Cyan = Color.cyan * .8f;
		public static Color Black = Color.black;
		public static Color White = Color.white;
		public static Color Brown = new Color(0.39607f, 0.26274f, 0.12941f);

		public static Color[] colors = { Red, Green, Blue, Yellow, Magenta, Cyan, Black, White, Brown};

		public static Color randomColor() {
			return colors[UnityEngine.Random.Range(0, Mathf.Min(6, colors.Length))];
		}

		public static Color addColor(Color a, Color b) {
			return getClosestColor(a + b);
		}

		public static Color subColor(Color a, Color b) {
			return getClosestColor(a - b);
		}

		// True if color a contains color b
		public static bool contains(Color a, Color b) {
			return ((b.r > .25 && Mathf.Abs(a.r - b.r) <= .25) ||
			    (b.g > .25 && Mathf.Abs(a.g - b.g) <= .25) ||
			    (b.b > .25 && Mathf.Abs(a.b - b.b) <= .25));
		}

		public static Color getClosestColor(Color c) {
			Color result = Black;
			if (c.r < 0) {
				c.r = 0;
			}
			if (c.g < 0) {
				c.g = 0;
			}
			if (c.b < 0) {
				c.b = 0;
			}
			if (c.a < 0) {
				c.a = 0;
			}
			float bestDistance = float.MaxValue;
			for (int i = 0; i < colors.Length; i++) {
				Color curr = colors[i];
				float distance = (Mathf.Abs(c.r - curr.r) + Mathf.Abs(c.g - curr.g) + Mathf.Abs(c.b - curr.b));
				if (distance < bestDistance) {
					bestDistance = distance;
					result = curr;
				}
			}
			return result;
		}
	};
	// Use this for initialization
	void Start () {
		background = GameObject.Find("Background").GetComponent<SpriteRenderer>();
		background.color = CustomColors.Green;
		GameObject boardObj = new GameObject();
		board = boardObj.AddComponent<Board>();
		loadLevelFromFile(levelFile, board);
		player = Instantiate(Resources.Load<GameObject>("Prefabs/Player")).GetComponent<PlayerMovement>();
		player.init(board);
	}

	// Update is called once per frame
	void Update() {
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
		if (player.readyToMove()) {
			if (Input.GetKey(KeyCode.UpArrow)) {
				player.move(Vector2.up);
			}
			else if (Input.GetKey(KeyCode.DownArrow)) {
				player.move(Vector2.down);
			}
			else if (Input.GetKey(KeyCode.LeftArrow)) {
				player.move(Vector2.left);
			}
			else if (Input.GetKey(KeyCode.RightArrow)) {
				player.move(Vector2.right);
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
					board.setBackground(bgColor);
				}
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
