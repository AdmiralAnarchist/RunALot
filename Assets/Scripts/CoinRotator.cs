using UnityEngine;
using System.Collections;

public class CoinRotator : MonoBehaviour {
	// Update is called once per frame
	void Update () {
		transform.Rotate (new Vector3 (10, 45, 0) * Time.deltaTime);
	}
}
