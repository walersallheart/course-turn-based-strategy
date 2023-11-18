using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public class Door : MonoBehaviour
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

    private void Start() {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetDoorAtGridPosition(gridPosition, this);

        if (isOpen) {
            OpenDoor();
        } else {
            CloseDoor();
        }   
    }

    private void Update() {
        if (!isActive) {
            return;
        }

        timer -= Time.deltaTime;

        if (timer <= 0) {
            onInteractComplete();
            isActive = false;
        }
    }

    public void Interact(Action onInteractComplete) {
        this.onInteractComplete = onInteractComplete;
        isActive = true;
        timer = .5f;

        if (isOpen) {
            CloseDoor();
        } else {
            OpenDoor();
        }
    }

    private void OpenDoor() {
        isOpen = true;
        animator.SetBool("IsOpen", isOpen);
        Pathfinding.Instance.SetIsWalkableGridPosition(gridPosition, true);
    }

    private void CloseDoor() {
        isOpen = false;
        animator.SetBool("IsOpen", isOpen);
        Pathfinding.Instance.SetIsWalkableGridPosition(gridPosition, false);
    }
}
