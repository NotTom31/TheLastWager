using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{

    private TheLastWagerInputActions input;

    private InputAction Select;

    private void Awake()
    {
        input = new TheLastWagerInputActions();
    }

    private void OnEnable()
    {
        input.Player.Select.Enable();
        input.Player.Select.performed += OnSelect;
    }

    private void OnDisable()
    {
        input.Player.Select.Disable();
    }

    private void OnSelect(InputAction.CallbackContext context)
    {
        Debug.Log("Select");

        if(GameManager.Instance.gameState == GameState.DIALOGUE)
        {
            //do dialogue progression here
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Card card = hit.collider.GetComponent<Card>();
            ContractClick contract = hit.collider.GetComponent<ContractClick>();
            if (card != null)
            {
                CardManager.Instance.SelectCard(card);
            }
            if (contract != null)
            {
                contract.ClickContract();
            }
        }
    }

    private void FixedUpdate()
    {
        
    }
}