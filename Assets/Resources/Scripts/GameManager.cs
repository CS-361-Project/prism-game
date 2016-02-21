using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	Board board;
	public SpriteRenderer background;
	PlayerMovement player;

	public static class CustomColors {
		public static Color Red = Color.red;
		public static Color Green = Color.green;
		public static Color Blue = Color.blue;
		public static Color Yellow = Color.yellow;
		public static Color Magenta = Color.magenta;
		public static Color Cyan = Color.cyan;
		public static Color Black = Color.black;
		public static Color White = Color.white;
		public static Color Brown = new Color(0.39607f, 0.26274f, 0.12941f);

		public static Color[] colors = { Red, Green, Blue, Yellow, Magenta, Cyan, Black, White, Brown};

		public static Color randomColor() {
			return colors[Random.Range(0, Mathf.Min(6, colors.Length))];
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
			print("Closest color to " + c + " was " + result);
			return result;
		}
	};
	// Use this for initialization
	void Start () {
		background = GameObject.Find("Background").GetComponent<SpriteRenderer>();
		background.color = CustomColors.Green;
		GameObject boardObj = new GameObject();
		board = boardObj.AddComponent<Board>();
		board.init(11, 10, background);
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
}
