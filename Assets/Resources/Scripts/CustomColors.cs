using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class CustomColors {
	public static Color Red = HexToColor("FF6978");
	public static Color Green = HexToColor("94EA54");
	public static Color Blue = HexToColor("3885F9");
	public static Color Yellow = HexToColor("F4EE36");
	public static Color Magenta = HexToColor("C85ED8");
	public static Color Cyan = HexToColor("4CEDDE");
	public static Color Black = HexToColor("858F91");
	public static Color White = HexToColor("FFF8F4");

	public static Color[] colors = {Black, Red, Green, Yellow, Blue, Magenta, Cyan, White};

	public static Color randomColor() {
		return colors[UnityEngine.Random.Range(0, Mathf.Min(6, colors.Length))];
	}

	public static Color addColor(Color a, Color b) {
		int indexA = indexOf(a);
		int indexB = indexOf(b);
		if (indexA == -1 || indexB == -1) {
			return Black;
		}
		return colors[indexA | indexB];
	}

	public static Color subColor(Color a, Color b) {
		int indexA = indexOf(a);
		int indexB = indexOf(b);
		if (indexA == -1 || indexB == -1) {
			return Black;
		}
		return colors[indexA & ~indexB];
	}

	static int indexOf(Color c) {
		int result = -1;
		for (int i = 0; i < colors.Length; i++) {
			if (colors[i] == c) {
				result = i;
				break;
			}
		}
		return result;
	}

	// True if color a contains color b
	public static bool contains(Color a, Color b) {
		int indexA = indexOf(a);
		int indexB = indexOf(b);
		if (indexA == -1 || indexB == -1) {
			GameManager.print("Invalid color! a: " + a + " b: " + b);
			return false;
		}
		else {
			return (indexA & indexB) == indexB;
		}
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

	static Color HexToColor(string hex) {
		byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
		byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
		byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
		return new Color32(r, g, b, 255);
	}
};