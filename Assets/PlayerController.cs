using UnityEngine;
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

	private int count;
	// Use this for initialization
	void Start ()
	{
		speed = BASESPEED + level;
		rb = GetComponent<Rigidbody> ();
		count = 0;
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
				speed += 2;
				break;
			}
		case TOUCHTYPE.DOWN:
			{
				float newspeed = speed - 2;
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
			rb.transform.position = rb.transform.position - new Vector3(3.0f, 0.0f, 0.0f);
			break;
		}
		case TOUCHTYPE.RIGHT:
		{
			rb.transform.position = rb.transform.position + new Vector3(3.0f, 0.0f, 0.0f);
			break;
		}
		}

		rb.velocity = new Vector3 (0.0f, 0.0f, 1.0f) * speed;

	}

	TOUCHTYPE checkTouch ()
	{
		count++;
		if (count == 10) {
			return TOUCHTYPE.UP;
		} else if (count == 100) {
			return TOUCHTYPE.DOWN;
		}
		return TOUCHTYPE.NONE;
	}
}
