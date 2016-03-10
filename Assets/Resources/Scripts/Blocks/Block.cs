using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Block : MonoBehaviour {
	protected BlockModel blockModel;
	protected Color baseColor;
	public List<Enemy> enemyList;

	public virtual void init(Color c, Color bgColor, Board b, Transform parent) {
		transform.parent = parent;
		blockModel = Instantiate(Resources.Load<GameObject>("Prefabs/Block")).GetComponent<BlockModel>();
		baseColor = c;
		blockModel.init(transform, baseColor);
		name = "Block";
		onBackgroundChange(bgColor);
		enemyList = new List<Enemy>();
	}

	public virtual void onBackgroundChange(Color bgColor) {
		if (bgColor == baseColor) {
			blockModel.setActive(false);
		}
		else {
			blockModel.setActive(true);
		}
	}


	//turn into two functions
	public void setEnemy(Enemy x) {
		//check if the enemy was already in the list
		if (enemyList.Contains(x)) {
			enemyList.Remove(x);
		}
		else {
			enemyList.Add(x);
			//if this makes it so there are more than two enemies than alert both enemies. 
		}

	}

	public bool hasEnemy(){
		if (enemyList.Count >= 1) {
			return true;
		}
		else {
			return false;
		}

	}

	public virtual void onBGTransition(Color from, Color to, float progress) {
		if (to == baseColor) {
			if (progress >= 1) {
				blockModel.setActive(false);
			}
			else {
				blockModel.setTransitionColor(true, progress);
			}
		}
		else
		if (from == baseColor) {
			if (progress >= 1) {
				blockModel.setActive(true);
			}
			else {
				blockModel.setTransitionColor(false, progress);
			}
		}
	}

	public virtual bool isPassable() {
		return !blockModel.isActive();
	}

	public void setColor(Color c, Color bgColor) {
		baseColor = c;
		blockModel.setColor(c);
		onBackgroundChange(bgColor);
	}

	public virtual bool passableWithBG(Color newBGColor) {
		return newBGColor == baseColor;
	}

	public Color getBaseColor(){
		return baseColor;
	}
}
