using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSolver {
	Board board;
	Player player;
	Exit exit;
	List<ToyEnemy> enemyList;
	int width, height;

	public BoardSolver(Board b) {
		board = b;
		player = board.getPlayer();
		exit = board.getExit();
		width = board.getWidth();
		height = board.getHeight();
		enemyList = new List<ToyEnemy>();
		foreach (Enemy e in board.getEnemyList()) {
			IntPoint pos = e.getPos();
			IntPoint dir = e.getDirection();
			enemyList.Add(new ToyEnemy(pos.x, pos.y, dir.x, dir.y));
		}
	}

	public List<IntPoint> solveLevel(){
		enemyList = new List<ToyEnemy>();
		Dictionary<DistanceDictKey, int>[,] distanceGrid = new Dictionary<DistanceDictKey, int>[width, height];
		foreach (Enemy e in board.getEnemyList()) {
			IntPoint pos = e.getPos();
			IntPoint dir = e.getDirection();
			enemyList.Add(new ToyEnemy(pos.x, pos.y, dir.x, dir.y));
		}
		bool foundPath = false;
		int targetX = exit.x;
		int targetY = exit.y;
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				for (int c = 0; c < CustomColors.colors.Length; c++) {
					distanceGrid[x, y] = new Dictionary<DistanceDictKey, int>();
				}
			}
		}
		List<QueueEntry> queue = new List<QueueEntry>();
		IntPoint playerPos = player.getPos();
//		GameManager.print("Player is at (" + playerPos.x + ", " + playerPos.y + ")");
		int bgColor = CustomColors.indexOf(board.getNextBGColor());
//		GameManager.print("Current background color is: " + bgColor);
//		foreach (ToyEnemy e in enemyList) {
//			GameManager.print("Enemy at (" + e.x + ", " + e.y + ") facing (" + e.dx + ", " + e.dy + ")");
//		}
		queue.Add(new QueueEntry(0, playerPos.x, playerPos.y, bgColor, copyEnemyList(enemyList)));
		if (!board.getBlock(playerPos.x, playerPos.y).name.Equals("Lever")) {
			distanceGrid[playerPos.x, playerPos.y].Add(new DistanceDictKey(bgColor, copyEnemyList(enemyList)), 0);
		}
		while (queue.Count > 0) {
			QueueEntry firstEntry = queue[0];
			queue.RemoveAt(0);
			int x = firstEntry.x;
			int y = firstEntry.y;
			bool playerDead = false;
			List<ToyEnemy> enemies = firstEntry.enemies;
			if (!board.onBoard(x, y)) {
				continue;
			}
			if (x == targetX && y == targetY) {
				foundPath = true;
				break;
			}

			DistanceDictKey key = new DistanceDictKey(firstEntry.color, copyEnemyList(firstEntry.enemies));

			Block currBlock = board.getBlock(x, y);
			if (firstEntry.distance > 0) {
				if (board.getBlock(x, y).name == "Lever") {
					LeverBlock lever = (LeverBlock)currBlock;
					int leverColor = CustomColors.indexOf(lever.leverColor);
					if ((firstEntry.color & leverColor) == leverColor) {
						firstEntry.color = firstEntry.color & ~leverColor;
					}
					else {
						firstEntry.color = firstEntry.color | leverColor;
					}
				}
				for (int i=enemies.Count - 1; i>= 0; i--) {
					ToyEnemy e = enemies[i];
					if (!e.canPassThrough(board, e.x, e.y, firstEntry.color)) {
						enemies.RemoveAt(i);
					}
					else {
						e.move(board, firstEntry.color);
						if (e.x == firstEntry.x && e.y == firstEntry.y) {
							playerDead = true;
							distanceGrid[firstEntry.x, firstEntry.y].Remove(key);
							break;
						}
					}
				}
				if (playerDead) {
					continue;
				}
			}
			int newBGColor = firstEntry.color;
			DistanceDictKey newKey = new DistanceDictKey(newBGColor, copyEnemyList(firstEntry.enemies));
			for (int i = -1; i <= 1; i++) {
				for (int j = -1; j <= 1; j++) {
					if ((i == 0 || j == 0) && !(i == 0 && j == 0) && board.onBoard(x + i, y + j)) {
						if (distanceGrid[x + i, y + j].ContainsKey(newKey)) {
							continue;
						}
						else {
							playerDead = false;
							foreach (ToyEnemy e in firstEntry.enemies) {
								if (e.x == firstEntry.x && e.y == firstEntry.y) {
									playerDead = true;
									break;
								}
							}
							if (playerDead) {
								continue;
							}
							Block neighbor = board.getBlock(x + i, y + j);
							switch (neighbor.name) {
								case "Block":
									if (newBGColor == CustomColors.indexOf(neighbor.getBaseColor())) {
										queue.Add(new QueueEntry(firstEntry.distance + 1,
											x + i, y + j, newBGColor, copyEnemyList(firstEntry.enemies)));
										distanceGrid[x + i, y + j].Add(newKey, firstEntry.distance + 1);
									}
									break;
								case "Lever":
									queue.Add(new QueueEntry(firstEntry.distance + 1,
										x + i, y + j, newBGColor, copyEnemyList(firstEntry.enemies)));
									distanceGrid[x + i, y + j].Add(newKey, firstEntry.distance + 1);
									break;
								case "Empty Block":
									queue.Add(new QueueEntry(firstEntry.distance + 1,
										x + i, y + j, newBGColor, copyEnemyList(firstEntry.enemies)));
									distanceGrid[x + i, y + j].Add(newKey, firstEntry.distance + 1);
									if (x + i == targetX && y + j == targetY) {
										foundPath = true;
									}
									break;
							}
						}
					}
				}
			}
		}
		if (foundPath) {
			return reconstructPath(distanceGrid);
		}
		else {
			return new List<IntPoint>();
		}
	}

	List<IntPoint> reconstructPath(Dictionary<DistanceDictKey, int>[,] distanceGrid) {
		List<IntPoint> path = new List<IntPoint>();
		int minRemainingDistance = 100000;
		int x = exit.x;
		int y = exit.y;
//		int numColors = distanceGrid.GetLength(2);
		DistanceDictKey currKey = null;

		path.Add(new IntPoint(x, y));
		foreach (DistanceDictKey key in distanceGrid[x, y].Keys) {
			int value = distanceGrid[x, y][key];
			if (value < minRemainingDistance) {
				minRemainingDistance = value;
				currKey = key;
			}
		}
//		for (int color = 0; color < numColors; color++) {
//			if (distanceGrid[x, y, color] < minRemainingDistance && distanceGrid[x, y, color] > -1) {
//				bgColor = color;
//				minRemainingDistance = distanceGrid[x, y].;
//			}
//		}
		while (minRemainingDistance > 0) {
			bool foundNextStep = false;
			for (int i = -1; i <= 1; i++) {
				for (int j = -1; j <= 1; j++) {
					if ((i == 0 || j == 0) && !(i == 0 && j == 0) && board.onBoard(x + i, y + j)) {
						Dictionary<DistanceDictKey, List<DistanceDictKey>> translateDict = new Dictionary<DistanceDictKey, List<DistanceDictKey>>();
						foreach (DistanceDictKey key in distanceGrid[x + i, y + j].Keys) {
							List<ToyEnemy> enemies = copyEnemyList(key.enemies);
							if (minRemainingDistance > 1) {
								for (int k = enemies.Count - 1; k >= 0; k--) {
									ToyEnemy e = enemies[k];
									if (!e.canPassThrough(board, e.x, e.y, key.color)) {
										enemies.RemoveAt(k);
									}
									else {
										e.move(board, key.color);
									}
								}
							}
							DistanceDictKey newKey = new DistanceDictKey(key.color, enemies);
							if (!translateDict.ContainsKey(newKey)) {
								translateDict.Add(newKey, new List<DistanceDictKey>());
							}
							translateDict[newKey].Add(key);
						}
						int newBGColor = currKey.color;
						if (minRemainingDistance > 1) {
							if (board.getBlock(x + i, y + j).name == "Lever") {
								LeverBlock lever = (LeverBlock)board.getBlock(x + i, y + j);
								int leverColor = CustomColors.indexOf(lever.leverColor);
								if ((currKey.color & leverColor) == leverColor) {
									newBGColor = currKey.color & ~leverColor;
								}
								else {
									newBGColor = currKey.color | leverColor;
								}
							}
						}
						DistanceDictKey trialKey = new DistanceDictKey(newBGColor, currKey.enemies);
						if (translateDict.ContainsKey(trialKey)) {
							foreach(DistanceDictKey translation in translateDict[trialKey]) {
								if (distanceGrid[x + i, y + j][translation] == minRemainingDistance - 1) {
									currKey = translation;
									x = x + i;
									y = y + j;
									minRemainingDistance--;
									path.Add(new IntPoint(x, y));
									foundNextStep = true;
									break;
								}
							}
							if (foundNextStep) {
								break;
							}
						}
					}
				}
				if (foundNextStep) {
					break;
				}
			}
			if (!foundNextStep) {
				break;
			}
		}
		path.Reverse();
//		print("Before");
//		printList<IntPoint>(path);
		if (path[0] != board.getPlayer().getPos()) {
			path.Insert(0, board.getPlayer().getPos());
		}
//		print("After");
//		printList<IntPoint>(path);
		return path;
	}

	void print(string s) {
		GameManager.print(s);
	}

	void printList<T>(List<T> list) {
		string s = "";
		foreach (T item in list) {
			s += ", " + item;
		}
		print(s);
	}

	List<ToyEnemy> copyEnemyList(List<ToyEnemy> enemies) {
		List<ToyEnemy> result = new List<ToyEnemy>();
		foreach (ToyEnemy e in enemies) {
			result.Add(new ToyEnemy(e));
		}
		return result;
	}

	class ToyEnemy {
		public int x, y, dx, dy;

		public override int GetHashCode() {
			int hash = 23;
			hash = 37 * hash + x;
			hash = 37 * hash + y;
			hash = 37 * hash + dx;
			hash = 37 * hash + dy;
			return hash;
		}

		public override bool Equals(object obj) {
			ToyEnemy enemy = obj as ToyEnemy;
			if (enemy != null) {
				return enemy.x == x && enemy.y == y && enemy.dx == dx && enemy.dy == dy;
			}
			return false;
		}

		public ToyEnemy(int xPos, int yPos, int xDir, int yDir) {
			x = xPos;
			y = yPos;
			dx = xDir;
			dy = yDir;
		}
		public ToyEnemy(ToyEnemy e) {
			x = e.x;
			y = e.y;
			dx = e.dx;
			dy = e.dy;
		}
		public void move(Board b, int bgColor) {
			if (canPassThrough(b, x + dx, y + dy, bgColor)) {
				x += dx;
				y += dy;
			}
			else {
				dx = -dx;
				dy = -dy;
				if (canPassThrough(b, x + dx, y + dy, bgColor)) {
					x += dx;
					y += dy;
				}
				else {
					dx = -dx;
					dy = -dy;
				}
			}
		}
		public bool canPassThrough(Board b, int x, int y, int bgColor) {
			if (b.onBoard(x, y)) {
				return b.getBlock(x, y).getBaseColor() == CustomColors.colors[bgColor];
			}
			else {
				return false;
			}
		}
	}

	class DistanceDictKey {
		public int color;
		public List<ToyEnemy> enemies;

		public DistanceDictKey(int c, List<ToyEnemy> e) {
			color = c;
			enemies = e;
		}

		public override int GetHashCode() {
			int hash = 23;
			foreach (ToyEnemy e in enemies) {
				hash = hash * 37 + e.GetHashCode();
			}
			hash = hash * 37 + color;
			return hash;
		}

		public override bool Equals(object obj) {
			DistanceDictKey key = obj as DistanceDictKey;
			if (key != null) {
				if (key.enemies.Count == enemies.Count) {
					bool result = true;
					for (int i = 0; i < enemies.Count; i++) {
						if (!key.enemies[i].Equals(enemies[i])) {
							result = false;
						}
					}
					return result;
				}
			}
			return false;
		}
	}

	class QueueEntry {
		public int distance, x, y, color;
		public List<ToyEnemy> enemies;
		public QueueEntry(int distance, int x, int y, int color, List<ToyEnemy> enemies) {
			this.distance = distance;
			this.x = x;
			this.y = y;
			this.color = color;
			this.enemies = enemies;
		}
	}
}