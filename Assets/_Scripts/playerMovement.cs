using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using UnityEditor.Tilemaps;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    //[Range(0, .3f)][SerializeField] private float myMovementSmoothing = .05f;
    public Transform scanAreaCentre;
    public SpriteRenderer spriteRenderer;
    public Sprite horizontalSprite;
    public Sprite verticalSprite;
    public Rigidbody2D myRigidbody;
    private int isRight=1;
    private int isUp=1;
    public float attackCentreDist = 1;
    [Range(0, 40)][SerializeField] public float mySpeed = 10;
    //private Vector2 currentVelocity = Vector2.zero;
    //private Vector2 targetVelocity = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        // start script is only run once
        gameObject.name = "Hanson";
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space) == true)
        //{
        //    targetVelocity = Vector2.up * flapStrength;
        //    myRigidbody.velocity = Vector2.SmoothDamp(myRigidbody.velocity, targetVelocity, ref currentVelocity, myMovementSmoothing);
        //}
        float horizontalValue = Input.GetAxisRaw("Horizontal");
        float verticalValue = Input.GetAxisRaw("Vertical");

        Vector2 currentVelocity = Vector2.zero;
        if (horizontalValue != 0)
        {   
            currentVelocity += mySpeed * Vector2.right * horizontalValue;
            if (horizontalValue * isRight < 0)
            {
                flipDir("Horizontal");
                isRight = -isRight;
            }
            if(spriteRenderer.sprite != horizontalSprite)
            {
                changeSprite("Horizontal");
            }
        }
        else
        {
            if (verticalValue != 0)
            {
                currentVelocity += mySpeed * Vector2.up * verticalValue;
                if (verticalValue * isUp < 0)
                {
                    flipDir("Vertical");
                    isUp = -isUp;
                }
                if (spriteRenderer.sprite != verticalSprite)
                {
                    changeSprite("Vertical");
                }
            }
        }

        myRigidbody.velocity = currentVelocity;

    }

    public IEnumerator AttackAnimate(float distance, float attackAnimateTime,float attackScaleRatio = 1.5f)
    {
        Vector2 attackScale = spriteRenderer.transform.localScale;
        attackScale.x *= attackScaleRatio;
        attackScale.y /= attackScaleRatio;
        Vector2 originalScale = spriteRenderer.transform.localScale;
        spriteRenderer.transform.Translate(distance,0,0);
        spriteRenderer.transform.localScale = attackScale;
        yield return new WaitForSeconds(attackAnimateTime);
        spriteRenderer.transform.Translate(-distance, 0, 0);
        spriteRenderer.transform.localScale = originalScale;
    }
    private void flipDir(string direction)
    {
        Vector2 localScale = gameObject.transform.localScale;
        if(direction == "Horizontal")
        {
            localScale.x = -localScale.x;
        }
        if(direction == "Vertical")
        {
            localScale.y = -localScale.y;
        }
        gameObject.transform.localScale= localScale;
    }

    private void changeSprite(string direction)
    {
        Vector2 localScale = gameObject.transform.localScale;//hard code to avoided inverted sprite when moving.
        if (direction == "Horizontal")
        {
            //keep the vertical scale to positive when moving horizontally
            //if (localScale.y < 0)
            //{
            //   localScale.y = Mathf.Abs(localScale.y);
            //   gameObject.transform.localScale = localScale;
            //}

            // Change direction of attack centre
            scanAreaCentre.position = gameObject.transform.position + attackCentreDist * Vector3.right * localScale.x;
            spriteRenderer.sprite = horizontalSprite;
        }
        if (direction == "Vertical")
        {
            // Change direction of attack centre
            scanAreaCentre.position = gameObject.transform.position + attackCentreDist * Vector3.up * localScale.y;
            spriteRenderer.sprite = verticalSprite;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.LogWarning("Player collided with something");
    }
}
