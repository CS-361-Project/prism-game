using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System;


public class GameData : MonoBehaviour {
	SaveData data;
	public static GameData Instance;
	public int totalMoves = 0;
	public int toggles = 0;
	public string saveFile;

	// Use this for initialization
	void Start() {
		// TODO: load previous data from file into new SaveData object
		saveFile = Application.persistentDataPath+"/GameData.txt";
		data = new SaveData();
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
		return data.getLevelStatus(pack, level);
	}


	//Get and Set Stats
	public void addMoves(int m){
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

	public void serialize(){
		StreamWriter writer = null;
		if (!File.Exists(saveFile)) {
			writer = new StreamWriter(File.Create(saveFile));
		}
		else {
			writer = new StreamWriter(File.OpenWrite(saveFile));
		}
		serializeDic(writer, data.completedLevels);
		writer.Close();
	}

	public void deserialize(){
		try {
			StreamReader reader = new StreamReader(saveFile);
			deserializeDic(reader, data.completedLevels);
			reader.Close();
		} catch (FileNotFoundException){
			print("Sam");
		}
	
	}


	public static void serializeDic(TextWriter writer, IDictionary dictionary)
	{
		List<Entry> entries = new List<Entry>(dictionary.Count);
		foreach (LevelKey key in dictionary.Keys)
		{

			entries.Add(new Entry(key.convertToString(), dictionary[key]));
		}
		XmlSerializer serializer = new XmlSerializer(typeof(List<Entry>));
		serializer.Serialize(writer, entries);

	}

	public static void deserializeDic(TextReader reader, IDictionary dictionary)
	{
		dictionary.Clear();
		XmlSerializer serializer = new XmlSerializer(typeof(List<Entry>));
		List<Entry> list = (List<Entry>)serializer.Deserialize(reader);
		foreach (Entry entry in list)
		{
			LevelKey key = convertToLevelKey(entry.Key);
			dictionary.Add(key, entry.Value);
		}
	}

	public class Entry
	{
		public string Key;
		public object Value;
		public Entry()
		{
		}

		public Entry(string key, object value)
		{
			Key = key;
			Value = value;
		}
	}



	[Serializable]
	class SaveData {
		public const int INCOMPLETE = 0;
		public const int COMPLETE = 1;
		public const int PERFECT = 2;

		//TODO decide if we shoudl take out his from SaveData entirely


		public Dictionary<LevelKey, int> completedLevels;
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
			}else {
				//TODO change this to a better value
				return 0;
			}

		}


	}


	static LevelKey convertToLevelKey(string entry){
		//parse string on space
		char[] delimchars = {';'};
		string[] words =entry.Split(delimchars);
		//print (words[0]);
		//print (words[1]);
		int x = Int32.Parse(words[1]);
		LevelKey key = new LevelKey(words[0], x);
		return key;
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

		public string convertToString() {
			//string levelNum = levelNumber.ToString()
			string LevelKey = packName + ";" + levelNumber.ToString();
			return LevelKey;
		}


	
	}
}