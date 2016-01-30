using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour {

	public bool check = false;
	private bool chaining = false;
	private float vanishTimer = 0.0f;
	public float VANISH_TIME;

	// Use this for initialization
	void Start () {
	}

	void Update(){
		if (chaining) {
			vanishTimer += Time.deltaTime;
		}
	}

	public void CheckChain(string tag, List<Enemy> list, ref bool vanish) {
		check = true;
		list.Add (this);
		vanish = (vanishTimer >= VANISH_TIME) || vanish;

		foreach (var enemy in GameObject.FindGameObjectsWithTag(tag)) {
			if (!enemy.GetComponent<Enemy> ().check) {
				if ((transform.position - enemy.transform.position).sqrMagnitude * 4 <
				    (transform.localScale.x + enemy.transform.localScale.x) * (transform.localScale.x + enemy.transform.localScale.x)) {
				
					enemy.GetComponent<Enemy> ().CheckChain (tag, list, ref vanish);
				}
			}
		}
	}

	void LateUpdate() {
		check = false;
	}

	public void InvokeChainEffect(){
		chaining = true;
		Renderer enemyRednderer = gameObject.GetComponent<Renderer> ();
		enemyRednderer.material.EnableKeyword ("_EMISSION");
		enemyRednderer.material.SetColor ("_EmissionColor", new Color (0.5f, 0.5f, 0.5f));
	}

	public void ResetChainEffect(){
		chaining = false;
		vanishTimer = 0.0f;
		Renderer enemyRednderer = gameObject.GetComponent<Renderer> ();
		enemyRednderer.material.EnableKeyword ("_EMISSION");
		enemyRednderer.material.SetColor ("_EmissionColor", new Color (0f, 0f, 0f));
	}
}
