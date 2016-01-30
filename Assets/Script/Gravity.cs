

using UnityEngine;
using System.Collections;

public class Gravity : MonoBehaviour {
	private GameObject planet;       // 引力の発生する星
private float GRAVITY = 9.8f * 5; // 加速度の大きさ

	void Start(){

		planet = GameObject.FindGameObjectWithTag("Planet");

	}

	void FixedUpdate () {
		// 星に向かう向きの取得
		var direction = planet.transform.position - transform.position;
		direction.Normalize();

		// 加速度与える
		GetComponent<Rigidbody>().AddForce(GRAVITY * direction, ForceMode.Acceleration);
	}
}