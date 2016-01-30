using UnityEngine;
using System.Collections;

public class PlanetGravity : MonoBehaviour {
	public GameObject planet;	// 引力の発生する星
	public float GRAVITY;	// 万有引力係数

	void Start(){
		var firstVelocity = new Vector3 (Random.value, Random.value,Random.value);
		GetComponent<Rigidbody> ().velocity = firstVelocity * 10.0f;
	}

	void FixedUpdate () {
		// 星に向かう向きの取得
		var direction = planet.transform.position - transform.position;
		// 星までの距離の２乗を取得
		var distance = direction.magnitude;
		distance *= distance;
		// 万有引力計算
		var gravity = GRAVITY * planet.GetComponent<Rigidbody>().mass * GetComponent<Rigidbody>().mass / distance;

		// 力を与える
		GetComponent<Rigidbody>().AddForce(gravity * direction.normalized, ForceMode.Force);
	}
}