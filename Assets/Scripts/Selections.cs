using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Selections : MonoBehaviour
{
    public GameObject TransfSelector;

    //public Button myButton;

    public float expandSpeed = 8f;

    private Transform managerObj;


    public void SelectButton()
    {
        HideSelector();

        //transform.parent.parent.GetComponent<SingleChoice>().ActivateNextObject(transform.GetSiblingIndex());

        managerObj = transform.root.GetChild(2).GetChild(0);

        int selectedButtonIndex = transform.GetSiblingIndex();

        //print("Pressed button " + selectedButtonIndex + ", Name: ");

        managerObj.GetComponent<GameManager>().NextScenario(selectedButtonIndex);
    }

    public void ShowSelector()
    {
        TransfSelector.SetActive(true);
    }

    public void HideSelector()
    {
        TransfSelector.transform.localScale = new Vector3(0, 1, 0);

        TransfSelector.SetActive(false);
    }

    private void Update()
    {
        if(TransfSelector.activeSelf)
            TransfSelector.transform.localScale = Vector3.Lerp(TransfSelector.transform.localScale, Vector3.one, expandSpeed * Time.deltaTime);
    }
}
