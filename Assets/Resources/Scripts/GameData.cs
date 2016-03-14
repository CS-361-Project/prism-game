using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameData : MonoBehaviour {
	SaveData data;
	// Use this for initialization
	void Start() {
		// TODO: load previous data from file into new SaveData object
	}

	public void markLevelComplete(string pack, int level) {
		data.addLevel(pack, level, SaveData.COMPLETE);
	}

	public void markLevelPerfect(string pack, int level) {
		data.addLevel(pack, level, SaveData.PERFECT);
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