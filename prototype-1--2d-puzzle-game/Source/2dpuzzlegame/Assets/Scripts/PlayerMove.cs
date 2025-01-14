﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerMove : MonoBehaviour
{
    
    public float factor = 0.01f;
    public float jumpAmount = 0.5f;
    public int keynumber = 0;
    public int MainKeyNumber = 0;
    public int maxNumber;
    public int maxMainKey;
    

    public SpriteRenderer spriteRenderer;
    public Rigidbody2D rb;

    public GameObject clones;
    public CloneMove[] cloneMoves;

    private bool canJump;

    private Vector3 moveVector;
    public EventSystemCustom ev;
    public Transform teleportDoorDes;
  


    void Start()
    {
        cloneMoves = clones.GetComponentsInChildren<CloneMove>();
        canJump = true;
        maxMainKey = GameObject.FindGameObjectsWithTag(TagNames.MainKey.ToString()).Length;
        moveVector = new Vector3(1 * factor, 0, 0);
        maxNumber = GameObject.FindGameObjectsWithTag(TagNames.KeyItem.ToString()).Length;
       

    }

    void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += moveVector;

            MoveClones(moveVector, true);

            spriteRenderer.flipX = false;

        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= moveVector;

            MoveClones(moveVector, false);

            spriteRenderer.flipX = true;
        }

        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            rb.AddForce(transform.up * jumpAmount, ForceMode2D.Impulse);
            JumpClones(jumpAmount);
        }


        // This was added to answer a question.
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Destroy(this.gameObject);
        }

     

        // This is too dirty. We must decalare/calculate the bounds in another way. 
        /*if (transform.position.x < -0.55f) 
        {
            transform.position = new Vector3(0.51f, transform.position.y, transform.position.z);
        }
        else if (transform.position.x > 0.53f)
        {
            transform.position = new Vector3(-0.53f, transform.position.y, transform.position.z);
        }*/
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TagNames.DeathZone.ToString()))
        {
            Debug.Log("DEATH ZONE");
            ev.OnDeathZone.Invoke();
        }
        
        if (collision.gameObject.CompareTag(TagNames.CollectableItem.ToString()))
        {
            collision.gameObject.SetActive(false);
            Debug.Log("POTION!");
        }
      
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(TagNames.StickyPlatform.ToString()))
        {
            Debug.LogWarning("sticky");
            canJump = false;
        }

        if (collision.gameObject.CompareTag(TagNames.ExitDoor.ToString()))
        {
            Debug.Log("exit door");
        }



    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TagNames.KeyItem.ToString()))
        {
            if (Input.GetKey(KeyCode.E))
            {
                collision.gameObject.SetActive(false);
                keynumber += 1;
                Debug.Log("eat key with keypress E"+ keynumber);
                ev.OnEatKey.Invoke();
            }
        }
        if (collision.gameObject.CompareTag(TagNames.DoorItem.ToString()))
        {
            if (Input.GetKey(KeyCode.E))
            {
                if (keynumber==maxNumber)
                {
                    ev.OnWin.Invoke();
                }
                
            }
        }
        if (collision.gameObject.CompareTag(TagNames.MainKey.ToString()))
        {
            if (Input.GetKey(KeyCode.E))
            {
                collision.gameObject.SetActive(false);
                MainKeyNumber += 1;
            }
        }
        if (collision.gameObject.CompareTag(TagNames.DoorSource.ToString()))
        {
            if (Input.GetKey(KeyCode.E))
            {
                if (MainKeyNumber == maxMainKey)
                {
                    transform.position = teleportDoorDes.position;
                    Debug.Log("teleport!");
                }
                
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(TagNames.StickyPlatform.ToString()))
        {
            Debug.LogWarning("sticky no more bruh");
            canJump = true;
        }
    }

    public void MoveClones(Vector3 vec, bool isDirRight)
    {
        foreach (var c in cloneMoves)
            c.Move(vec, isDirRight);
    }

    public void JumpClones(float amount)
    {
        foreach (var c in cloneMoves)
            c.Jump(amount);
    }
}
