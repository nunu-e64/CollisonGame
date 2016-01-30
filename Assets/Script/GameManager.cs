using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public Text timerText;
	private float timer = 0.0f;
	private float addDeltaTimer = 0.0f;
	public float ADD_DELTA_TIME;

	private GameObject[] enemies;

	public enum GameStatus{
		PLAY,
		END
	}
	public GameStatus gameStatus = GameStatus.PLAY;

	public Text vanishCounterText;
	private int vanishCounter = 0;
	public Text maxChainText;
	private int maxChain = 0;

	public string[] tagList;
	public int MIN_CHAIN_NUM;



	// Use this for initialization
	IEnumerator Start () {
		enemies = Resources.LoadAll <GameObject> ("Enemy");

		for (int i = 0; i < 10; i++) {
			yield return new WaitForSeconds(0.1f);
			foreach (var enemy in enemies) {
				var positon = new Vector3 (Random.Range (-6, 6), Random.Range (-15, 15), 0);
				var obj = GameObject.Instantiate (enemy, positon, Quaternion.identity) as GameObject;
				obj.transform.parent = transform;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		CheckChain ();

		if (gameStatus == GameStatus.PLAY) {
			timer += Time.deltaTime;
			addDeltaTimer += Time.deltaTime;
		}

		if (addDeltaTimer > ADD_DELTA_TIME) {
			AddNewEnemy ();
			addDeltaTimer = 0.0f;
		}

		timerText.text = ((int)timer).ToString() + ":" + ((int)((timer - (int)timer) * 10)).ToString();
		timerText.color = (gameStatus==GameStatus.PLAY ? new Color(1, 0, 0) : new Color(0, 0, 1));
		vanishCounterText.text = "Vanish:" + vanishCounter.ToString ();
		maxChainText.text = "MaxChain:" + maxChain.ToString ();
	}

	void AddNewEnemy(){
		int additionalNum = 5 * (int)(timer / ADD_DELTA_TIME); 
		for (int i = 0; i < additionalNum; i++) { 
			int enemyIndex = Random.Range (0, enemies.Length);
			var positon = new Vector3 (Random.Range(-20, 20), Random.Range (-25, 25), 0);
			positon.x += positon.x > 0 ? 10 : -10;
			positon.y += positon.y > 0 ? 15 : -15;
			var obj = GameObject.Instantiate (enemies[enemyIndex], positon, Quaternion.identity) as GameObject;
			obj.transform.parent = transform;
		}
	}

	void CheckChain(){	//Check Chaind gems in every frame;

		foreach (string tag in tagList) {
			
			foreach (var enemyObj in GameObject.FindGameObjectsWithTag(tag)) {
				var enemy = enemyObj.GetComponent<Enemy> ();
				if (!enemy.check) {
					var chainList = new List<Enemy>();
					bool vanish = false;
					enemy.CheckChain(tag, chainList, ref vanish);	//start recursive check

					foreach (var chainedEnemy in chainList) {
						if (chainList.Count >= MIN_CHAIN_NUM) {
							if (vanish) {
								chainedEnemy.Vanish ();
								vanishCounter++;
								if (chainList.Count > maxChain) maxChain = chainList.Count;
							} else {
								chainedEnemy.InvokeChainEffect ();
							}
						} else {
							chainedEnemy.ResetChainEffect ();
						}
					}
				}
			}				

		}
	}
		
}
	