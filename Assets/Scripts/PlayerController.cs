using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

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
        LONGPRESS
    }
    ;

    #region Privates
    /// <summary>
    /// Private Properties
    /// </summary>
    private float BASESPEED = 10;
    private float BASEJUMP = 600;

    private float speed;
    private Rigidbody rb;

    private int score;

    private float oldTouchTime;
    private float longPressTime;
    #endregion

    #region Publics
    /// <summary>
    /// Public Properties
    /// </summary>
    public float level;
    public Vector2 oldTouchPosition;
    public Text scoreText;
    #endregion

    /// <summary>
    /// When the game starts, this is called
    /// </summary>
    void Start()
    {
        speed = BASESPEED + level;
        rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Fixed because of forces. Sets the new velocity and score each frame.
    /// </summary>
    void FixedUpdate()
    {

        TOUCHTYPE touch = checkTouch();
        Vector3 acceleration = checkAcceleration();
        switch (touch)
        {
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
                    if (newspeed >= BASESPEED + level)
                    {
                        speed = newspeed;
                    }
                    break;
                }
            case TOUCHTYPE.SINGLETAP:
                {
                    if (speed == 0)
                    {
                        speed = BASESPEED + level;
                    }
                    else if (rb.gameObject.transform.position.y < 0.51)
                    {
                        rb.AddForce(new Vector3(0.0f, 1.0f, 0.0f) * BASEJUMP);
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
                    if (rb.transform.position.x > 0.0f)
                    {
                        rb.transform.position = new Vector3(0.0f, rb.transform.position.y, rb.transform.position.z);
                    }
                    else
                    {
                        rb.transform.position = new Vector3(-3.0f, rb.transform.position.y, rb.transform.position.z);
                    }
                    break;
                }
            case TOUCHTYPE.RIGHT:
                {
                    if (rb.transform.position.x < 0.0f)
                    {
                        rb.transform.position = new Vector3(0.0f, rb.transform.position.y, rb.transform.position.z);
                    }
                    else
                    {
                        rb.transform.position = new Vector3(3.0f, rb.transform.position.y, rb.transform.position.z);
                    }
                    break;
                }
        }

        scoreText.text = "Score: " + score.ToString();
        rb.velocity = new Vector3(acceleration.x, 0.0f, 1.0f) * speed;

    }

    /// <summary>
    /// Check the acceleration of the device and handle it accordingly
    /// </summary>
    /// <returns>Vector3 with accelerationdata</returns>
    private Vector3 checkAcceleration()
    {
        Vector3 rawAcceleration = Input.acceleration;
        print("Acceleration: x = " + rawAcceleration.x + " | y = " + rawAcceleration.y + " | z = " + rawAcceleration.z);
        return rawAcceleration;
    }

    /// <summary>
    /// Check for touches on the screen and handle them accordingly
    /// </summary>
    /// <returns>A touchtype (see enum)</returns>
    TOUCHTYPE checkTouch()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            print(touch.phase.ToString());
            if (touch.phase == TouchPhase.Began)
            {
                oldTouchTime = Time.time;
                oldTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                Vector2 touchDeltaPosition = touch.position - oldTouchPosition;
                print("x: " + touchDeltaPosition.x + " | y: " + touchDeltaPosition.y + " | time: " + (Time.time - oldTouchTime));
                if (Time.time - oldTouchTime > 0.03)
                {
                    TOUCHTYPE returnType = TOUCHTYPE.NONE;
                    if (touchDeltaPosition.x < -70)
                    {
                        returnType = TOUCHTYPE.LEFT;
                    }
                    else if (touchDeltaPosition.x > 70)
                    {
                        returnType = TOUCHTYPE.RIGHT;
                    }
                    else if (touchDeltaPosition.y > 70)
                    {
                        returnType = TOUCHTYPE.UP;
                    }
                    else if (touchDeltaPosition.y < -70)
                    {
                        returnType = TOUCHTYPE.DOWN;
                    }
                    else if (Time.time - oldTouchTime > 1)
                    {
                        returnType = TOUCHTYPE.LONGPRESS;
                    }
                    else
                    {
                        returnType = TOUCHTYPE.SINGLETAP;
                    }
                    oldTouchTime = Time.time;
                    return returnType;
                }
            }
        }
        return TOUCHTYPE.NONE;
    }

    /// <summary>
    /// When you collide with a trigger
    /// </summary>
    /// <param name="col">the object you collide with</param>
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Coin"))
        {
            col.gameObject.SetActive(false);
            score += 10;
        }
    }
}
