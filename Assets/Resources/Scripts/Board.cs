using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Board : MonoBehaviour {
	int width, height;
	public Block[,] blocks;
	SpriteRenderer background;
	GameObject emptyBlockFolder, blockFolder, switchFolder;
	// Use this for initialization
	public void init(int w, int h, SpriteRenderer bgRender) {
		Vector3 center = new Vector3((float)w / 2.0f - .5f, (float)h / 2.0f - .5f, 0);
		transform.position = -center;
		background = bgRender;
		bgRender.transform.parent = transform;
		width = w;
		height = h;
		blocks = new Block[w,h];
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
		onBackgroundChange();
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
		Block b;
		if (blocks[x, y] == null) {
			GameObject obj = new GameObject();
			b = obj.AddComponent<Block>();
			b.init(c, background.color, this, blockFolder.transform);
			b.transform.localPosition = new Vector3(x, y, 0);
			blocks[x, y] = b;
		}
		else {
			GameObject obj = blocks[x, y].gameObject;
			DestroyImmediate(blocks[x, y]);
			blocks[x, y] = obj.AddComponent<Block>();
			blocks[x, y].init(c, background.color, this, blockFolder.transform);
		}
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

	public Block getBlock(int x, int y) {
		return blocks[x, y];
	}

	public Vector3 getBlockPosition(int x, int y) {
		return blocks[x, y].transform.position;
	}

	public bool getBlockPassable(int x, int y) {
		if (x >= 0 && x < width && y >= 0 && y < height) {
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
		onBackgroundChange();
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
}
