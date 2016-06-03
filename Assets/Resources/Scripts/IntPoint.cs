using System;
using UnityEngine;

public class IntPoint {
	public int x,y;
	public static IntPoint zero = new IntPoint(0, 0);

	public IntPoint(int x, int y) {
		this.x = x;
		this.y = y;
	}

	public override string ToString() {
		return "(" + x + ", " + y + ")";
	}

	public override int GetHashCode() {
		int hash = 23;
		hash = hash * 37 + x;
		hash = hash * 37 + y;
		return hash;
	}

	public override bool Equals(object obj) {
		IntPoint pt = obj as IntPoint;
		if (pt != null) {
			return pt == this;
		}
		else {
			return false;
		}
	}

	public void normalize() {
		if (x != 0 || y != 0) {
			x = x > y ? 1 : 0;
			y = 1 - x;
		}
	}

	public Vector2 getVector2() {
		return new Vector2(x, y);
	}

	public static IntPoint operator -(IntPoint a, IntPoint b) {
		return new IntPoint(a.x - b.x, a.y - b.y);
	}

	public static IntPoint operator +(IntPoint a, IntPoint b) {
		return new IntPoint(a.x + b.x, a.y + b.y);
	}

	public static bool operator ==(IntPoint a, IntPoint b) {
		return a.x == b.x && a.y == b.y;
	}

	public static bool operator !=(IntPoint a, IntPoint b) {
		return !(a.x == b.x && a.y == b.y);
	}
}