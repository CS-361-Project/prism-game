using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameData : MonoBehaviour {
	SaveData data;
	public static GameData Instance;
	public int totalMoves = 0;
	public int toggles = 0;
	// Use this for initialization
	void Start() {
		// TODO: load previous data from file into new SaveData object
	}

	void Awake () {
		if (Instance == null) {
			DontDestroyOnLoad(gameObject);
			Instance = this;
		}
		else if (Instance != this) {
			Destroy (gameObject);
		}
	}


	//Get and Set level status
	public void markLevelComplete(string pack, int level) {
		data.addLevel(pack, level, SaveData.COMPLETE);
	}

	public void markLevelPerfect(string pack, int level) {
		data.addLevel(pack, level, SaveData.PERFECT);
	}

	public int getLevelStatus(string pack, int level){
		data.getLevelStatus(pack, level);
	}


	//Get and Set Stats
	public int addMoves(int m){
		totalMoves += m;
	}

	public int getTotalMoves(){
		return totalMoves;
	}

	public void addToggles(int t){
		toggles += t;
	}

	public int getToggles(){
		return toggles;
	}



	class SaveData {
		public const int INCOMPLETE = 0;
		public const int COMPLETE = 1;
		public const int PERFECT = 2;




		Dictionary<LevelKey, int> completedLevels;
		public SaveData() {
			completedLevels = new Dictionary<LevelKey, int>();
		}
		public void addLevel(string pack, int level, int status) {
			LevelKey key = new LevelKey(pack, level);
			if (completedLevels.ContainsKey(key)) {
				completedLevels[key] = status;
			}
			else {
				completedLevels.Add(key, status);
			}
		}

		public int getLevelStatus(string pack, int level){
			LevelKey key = new LevelKey(pack, level);
			if (completedLevels.ContainsKey(key)) {
				return completedLevels[key];
			}
		}


	}

	class LevelKey {
		public string packName;
		public int levelNumber;
		public LevelKey(string pack, int level) {
			packName = pack;
			levelNumber = level;
		}
		public override int GetHashCode() {
			return (packName + levelNumber.ToString()).GetHashCode();
		}
		public override bool Equals(object obj) {
			LevelKey otherKey = obj as LevelKey;
			if (otherKey != null) {
				return otherKey.packName == packName && otherKey.levelNumber == levelNumber;
			}
			return false;
		}
	}
}