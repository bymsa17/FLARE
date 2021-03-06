﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private CharacterBehaviour player;
    public Transform canvasPause;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBehaviour>();
    }
	void Update ()
    {
        InputAxis();
        InputJump();
        InputRun();
        InputAttack();
        InputPause();
	}

    void InputAxis()
    {
        Vector2 axis = Vector2.zero;
        axis.x = Input.GetAxis("Horizontal");
        axis.y = Input.GetAxis("Vertical");
        // TODO: Le pasamos el axis al player
        player.SetAxis(axis);
    }
    void InputJump()
    {
        if(Input.GetButtonDown("Jump"))
        {
            Debug.Log("Jump");
            // TODO: darle la orden al player de saltar
            player.JumpStart();
        }
    }
    void InputRun()
    {
        if(Input.GetButtonDown("Run"))
        {
            Debug.Log("Run");
            // TODO: darle la orden al player de correr
            player.isRunning = true;
        }
        if(Input.GetButtonUp("Run"))
        {
            Debug.Log("Walk");
            // TODO: darle la orden al player de caminar
            player.isRunning = false;
        }
    }

    void InputAttack()
    {
        if(Input.GetButtonDown("Attack"))
        {
            Debug.Log("Attack");
            player.Attack();
        }
    }

    void InputPause()
    {
        if(Input.GetButtonDown("Pause"))
        {
            Debug.Log("Pause");
            player.Pause();
        }
    }

}
