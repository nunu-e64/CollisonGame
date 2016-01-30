using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

	private List<GameObject> collisionList;
	private bool replaced = false;
	public GameManager gameManager;


	// Use this for initialization
	void Start () {
		collisionList = new List<GameObject>();	
	}
	
	// Update is called once per frame
	void Update () {

		if (gameManager.gameStatus==GameManager.GameStatus.MOVE && Input.GetMouseButton (0)) {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit = new RaycastHit ();

			if (Physics.Raycast (ray, out hit)) {
				foreach (var enemy in collisionList) {
					if (enemy == hit.collider.gameObject) {
						Vector3 oldPlayerPos = transform.position;
						transform.position = enemy.transform.position;
						enemy.transform.position = oldPlayerPos;
						replaced = true;
						break;
					}
				}
			}

			if (replaced) {
				replaced = false;
				collisionList.Clear ();
			}
		}
	}

//	void OnCollisionStay(Collision other){
	void OnTriggerStay(Collider other) {
	if (other.gameObject.layer ==  LayerMask.NameToLayer("Enemy")) {
			if (!collisionList.Contains (other.gameObject)) {
				collisionList.Add (other.gameObject);
			}
		}
	}
}
