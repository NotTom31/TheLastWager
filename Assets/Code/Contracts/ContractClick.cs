using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContractClick : MonoBehaviour
{
    //private bool isOpen;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.gameState == GameState.PLAY)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    ContractClick contract = hit.collider.GetComponent<ContractClick>();
                    if (contract != null)
                    {
                        MenuManager.Instance.OpenOneContract();
                        string[] b = GetComponent<Contract>().blurbs;

                        int i = 0;
                        MenuManager.Instance.contractClause1.text = string.Empty;
                        MenuManager.Instance.contractClause2.text = string.Empty;
                        MenuManager.Instance.contractClause3.text = string.Empty;
                        MenuManager.Instance.contractClause4.text = string.Empty;

                        foreach (string s in b)
                        {
                            Debug.Log(s + "tom");
                            MenuManager.Instance.UpdateContractText(i, s);
                            i++;
                        }
                        //isOpen = true;
                        //CardManager.Instance.SelectCard(contract);
                    }
                }
            }
        }
    }
}
