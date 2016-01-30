using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public Text timerText;
	private float timer;
	public float MOVE_TIME;
	public float WAIT_TO_VANISH_TIME;

	private GameObject[] enemies;
	private List<GameObject> vanishEnemies;

	public enum GameStatus{
		MOVE,
		WAIT_TO_VANISH,
		VANISH,
		END
	}
	public GameStatus gameStatus = GameStatus.MOVE;
	private int vanishCounter = 0;

	public string[] tagList;
	public int MIN_CHAIN_NUM;



	// Use this for initialization
	IEnumerator Start () {
		enemies = Resources.LoadAll <GameObject> ("Enemy");
		vanishEnemies = new List<GameObject> ();
		timer = MOVE_TIME;

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
		if (gameStatus == GameStatus.MOVE || gameStatus == GameStatus.WAIT_TO_VANISH) timer -= Time.deltaTime;

		if (timer < 0) {
			timer = 0.0f;

			if (gameStatus == GameStatus.MOVE) {
				gameStatus = GameStatus.WAIT_TO_VANISH;
				timer = WAIT_TO_VANISH_TIME;
				CheckChain ();
			} else if (gameStatus == GameStatus.WAIT_TO_VANISH) {
				gameStatus = GameStatus.VANISH;
				VanishEnemy ();
				AddNewEnemy();
				gameStatus = GameStatus.MOVE;
				timer = MOVE_TIME;
			}
		}

		timerText.text = ((int)timer).ToString() + ":" + ((int)((timer - (int)timer) * 10)).ToString();
		timerText.color = (gameStatus==GameStatus.MOVE ? new Color(1, 0, 0) : new Color(0, 0, 1));
	}

	void AddNewEnemy(){
		int enemyIndex;
		for (int i = 0; i < vanishCounter; i++) { 
			enemyIndex = Random.Range (0, enemies.Length);
			var positon = new Vector3 (Random.Range (-4, 4), Random.Range (10, 20), 0);
			var obj = GameObject.Instantiate (enemies[enemyIndex], positon, Quaternion.identity) as GameObject;
			obj.transform.parent = transform;
		}
	}

	void CheckChain(){	//Check Chaind gems in every frame;
		var chainEnemyListList = new List<List<Enemy>> ();

		foreach (string tag in tagList) {
			
			foreach (var enemyObj in GameObject.FindGameObjectsWithTag(tag)) {
				var enemy = enemyObj.GetComponent<Enemy> ();
				if (!enemy.check) {
					var chainList = new List<Enemy>();
					bool vanish = false;
					enemy.CheckChain(tag, chainList, ref vanish);	//start recursive check
					Debug.Log (tag + ":" + chainList.Count.ToString());
					chainEnemyListList.Add (chainList);
				}
			}				

			// apply checkchain result
			foreach (var chainList in chainEnemyListList) {
				foreach (var chainedEnemy in chainList) {
					if (chainList.Count >= MIN_CHAIN_NUM) {
						chainedEnemy.InvokeChainEffect ();
					} else {
						chainedEnemy.ResetChainEffect ();
					}
				}
			}
		}
	}
		
	void VanishEnemy(){
		foreach (var vanishEnemy in vanishEnemies) {
			Destroy (vanishEnemy);
		}
		
		vanishCounter = vanishEnemies.Count;
		vanishEnemies.Clear();
	}

}
	