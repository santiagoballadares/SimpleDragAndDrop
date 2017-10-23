using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectOnInput : MonoBehaviour {
	public EventSystem eventSystem;
	public GameObject selectedObject;

	public enum DirectionType { Horizontal, Vertical }
	public DirectionType directionType = DirectionType.Horizontal;
	private string direction;

	private bool buttonSelected;

	void Awake () {
		if (directionType == DirectionType.Horizontal) {
			direction = "Horizontal";
		} else if (directionType == DirectionType.Vertical) {
			direction = "Vertical";
		}
	}
	
	void Update () {
		if (Input.GetAxisRaw (direction) != 0 && buttonSelected == false) {
			eventSystem.SetSelectedGameObject (selectedObject);
			buttonSelected = true;
		}
	}

	private void onDisable () {
		buttonSelected = false; 
	}
}
