using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	private enum TOUCHTYPE
	{
		NONE,
		UP,
		DOWN,
		LEFT,
		RIGHT,
		SINGLETAP,
		LONGPRESS}
	;

	private float BASESPEED = 10;
	private float BASEJUMP = 600;
	private float speed;
	public float level;
	private Rigidbody rb;

	private Touch oldTouch;
	private bool touched;
	private float oldTouchTime;

	public Text scoreText;
	private int score;
	// Use this for initialization
	void Start ()
	{
		speed = BASESPEED + level;
		rb = GetComponent<Rigidbody> ();
		touched = false;
		oldTouchTime = Time.time;
		//rb.velocity = new Vector3 (0.0f, 0.0f, 1.0f) * speed;
		//rb.AddForce(new Vector3(0.0f, 0.0f, 1.0f) * speed);
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{

		TOUCHTYPE touch = checkTouch ();
		switch (touch) {
		case TOUCHTYPE.NONE:
			{
				break;
			}
		case TOUCHTYPE.UP:
			{
				speed += 10;
				break;
			}
		case TOUCHTYPE.DOWN:
			{
				float newspeed = speed - 10;
				if (newspeed >= BASESPEED + level) {
					speed = newspeed;
				}
				break;
			}
		case TOUCHTYPE.SINGLETAP:
			{
				if (speed == 0) {
					speed = BASESPEED + level;
				} else {
					rb.AddForce (new Vector3 (0.0f, 1.0f, 0.0f) * BASEJUMP);
				}
				break;
			}
		case TOUCHTYPE.LONGPRESS:
			{
				speed = 0;
				break;
			}
		case TOUCHTYPE.LEFT:
		{
			if(rb.transform.position.x != -3.0f){
				rb.transform.position = rb.transform.position - new Vector3(3.0f, 0.0f, 0.0f);
			}
			break;
		}
		case TOUCHTYPE.RIGHT:
		{
			if(rb.transform.position.x != 3.0f)
			{
				rb.transform.position = rb.transform.position + new Vector3(3.0f, 0.0f, 0.0f);
			}
			break;
		}
		}

		scoreText.text = "Score: " + score.ToString();
		rb.velocity = new Vector3 (0.0f, 0.0f, 1.0f) * speed;

	}

	TOUCHTYPE checkTouch ()
	{
		if (Input.touchCount > 0) {
			Touch[] touches = Input.touches;
			Touch t = touches[0];
			if(Time.time - oldTouchTime >= 0.1){
				oldTouchTime = Time.time;
				if(t.phase == TouchPhase.Stationary){
					//return TOUCHTYPE.SINGLETAP;
				} else if (t.phase == TouchPhase.Moved) {
					if(t.deltaPosition.x < -10){
						return TOUCHTYPE.LEFT;
					} else if (t.deltaPosition.x > 10){
						return TOUCHTYPE.RIGHT;
					} else if (t.deltaPosition.y > 10){
						return TOUCHTYPE.UP;
					} else if (t.deltaPosition.y < -10){
						return TOUCHTYPE.DOWN;
					} else {
						//return TOUCHTYPE.SINGLETAP;
					}
				}
			}
		}
		return TOUCHTYPE.NONE;
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.CompareTag ("Coin")) {
			col.gameObject.SetActive(false);
			score+=10;
		}
	}
}
