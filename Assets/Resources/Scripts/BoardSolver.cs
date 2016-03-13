using System;
using System.Collections;
using System.Collections.Generic;


public class BoardSolver {
	Board board;
	Player player;
	Exit exit;
	int width, height;
	public BoardSolver(Board b) {
		board = b;
		player = board.getPlayer();
		exit = board.getExit();
		width = board.getWidth();
		height = board.getHeight();
	}
	public List<IntPoint> solveLevel(){
		int[,,] distanceGrid = new int[width, height, CustomColors.colors.Length];
		bool foundPath = false;
		int targetX = exit.x;
		int targetY = exit.y;
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				for (int c = 0; c < CustomColors.colors.Length; c++) {
					distanceGrid[x, y, c] = -1;
				}
			}
		}
		List<QueueEntry> queue = new List<QueueEntry>();
		int[] playerPos = player.getPos();
		queue.Add(new QueueEntry(0, playerPos[0], playerPos[1], CustomColors.indexOf(board.nextBGColor())));
		while (queue.Count > 0) {
			QueueEntry firstEntry = queue[0];
			queue.RemoveAt(0);
			int x = firstEntry.x;
			int y = firstEntry.y;
			if (!board.onBoard(x, y)) {
				continue;
			}
			if (x == targetX && y == targetY) {
				foundPath = true;
				break;
			}
			Block currBlock = board.getBlock(x, y);
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
			int color = firstEntry.color;
			for (int i = -1; i <= 1; i++) {
				for (int j = -1; j <= 1; j++) {
					if ((i == 0 || j == 0) && !(i == 0 && j == 0) && board.onBoard(x + i, y + j) ) {
						if (distanceGrid[x + i, y + j, color] > -1) {
							continue;
						}
						else {
							Block neighbor = board.getBlock(x + i, y + j);
							switch (neighbor.name) {
								case "Block":
									if (color == CustomColors.indexOf(neighbor.getBaseColor())) {
										queue.Add(new QueueEntry(firstEntry.distance + 1,
											x + i, y + j, color));
										distanceGrid[x + i, y + j, color] = firstEntry.distance + 1;
									}
									break;
								case "Lever":
									queue.Add(new QueueEntry(firstEntry.distance + 1,
										x + i, y + j, color));
									distanceGrid[x + i, y + j, color] = firstEntry.distance + 1;
									break;
								case "Empty Block":
									queue.Add(new QueueEntry(firstEntry.distance + 1,
										x + i, y + j, color));
									distanceGrid[x + i, y + j, color] = firstEntry.distance + 1;
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

	List<IntPoint> reconstructPath(int[,,] distanceGrid) {
		List<IntPoint> path = new List<IntPoint>();
		int minRemainingDistance = 100000;
		int x = exit.x;
		int y = exit.y;
		int numColors = distanceGrid.GetLength(2);
		int bgColor = 0;

		path.Add(new IntPoint(x, y));
		for (int color = 0; color < numColors; color++) {
			if (distanceGrid[x, y, color] < minRemainingDistance && distanceGrid[x, y, color] > -1) {
				bgColor = color;
				minRemainingDistance = distanceGrid[x, y, color];
			}
		}
		while (minRemainingDistance > 0) {
			bool foundNextStep = false;
			for (int i = -1; i <= 1; i++) {
				for (int j = -1; j <= 1; j++) {
					if ((i == 0 || j == 0) && !(i == 0 && j == 0) && board.onBoard(x + i, y + j)) {
						int newBGColor = bgColor;
						if (board.getBlock(x + i, y + j).name == "Lever") {
							LeverBlock lever = (LeverBlock)board.getBlock(x + i, y + j);
							int leverColor = CustomColors.indexOf(lever.leverColor);
							if ((newBGColor & leverColor) == leverColor) {
								newBGColor = newBGColor & ~leverColor;
							}
							else {
								newBGColor = newBGColor | leverColor;
							}
						}
						if (distanceGrid[x + i, y + j, newBGColor] == minRemainingDistance - 1) {
							bgColor = newBGColor;
							x = x + i;
							y = y + j;
							minRemainingDistance--;
							path.Add(new IntPoint(x, y));
							foundNextStep = true;
							break;
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
		return path;
	}

	class QueueEntry {
		public int distance, x, y, color;
		public QueueEntry(int distance, int x, int y, int color) {
			this.distance = distance;
			this.x = x;
			this.y = y;
			this.color = color;
		}
	}
}