using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyDoor : MonoBehaviour, IInteractable
{

    [SerializeField] private bool isOpen;
    private GridPosition gridPosition;

    private Animator animator;

    private Action onInteractComplete;

    private bool isActive;
    private float timer;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        Debug.Log("isactive:"+isActive);
        Debug.Log("isOpen:"+isOpen);
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetInteractableAtGridPosition(gridPosition, this);
        if (isOpen)
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }
    }
    private void Update() {
        if(!isActive) return;

        timer -= Time.deltaTime;
        if(timer <= 0f){
            isActive = false;
            onInteractComplete();
        }
    }
    public void Interact(Action onInteractComplete)
    {
       
        this.onInteractComplete = onInteractComplete;
        isActive = true;
        timer = .5f;
 
        if (isOpen)
        {
            CloseDoor();

        }
        else
        {
            OpenDoor();
        }
    }

    private void OpenDoor()
    {
        isOpen = true;
        animator.SetBool("IsOpen",isOpen);
       
        Pathfinding.Instance.SetIsWalkableGridPosition(gridPosition, true);
    }

    private void CloseDoor()
    {
       
        isOpen = false;
        animator.SetBool("IsOpen",isOpen);
      
        Pathfinding.Instance.SetIsWalkableGridPosition(gridPosition, false);
    }

    
}
