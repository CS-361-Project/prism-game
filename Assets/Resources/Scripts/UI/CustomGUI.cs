using UnityEngine;
using System.Collections.Generic;

public class CustomGUI : MonoBehaviour{
	List<GameObject> guiElements;
	public void addElement(GameObject obj, Vector2 screenPos) {
		guiElements.Add(obj);
		obj.transform.position = Camera.main.WorldToScreenPoint(screenPos);
	}
}