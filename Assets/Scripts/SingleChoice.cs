using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SingleChoice : MonoBehaviour
{
    public string[] choiceOptions;

    [TextArea(15, 20)]
    public string promptText;

    public GameObject[] NextScenario;

    public TextMeshProUGUI promptUI;

    public Transform OptionsUI;

    private GameManager Manager;


    private void OnEnable()
    {
        Manager = transform.parent.gameObject.GetComponent<GameManager>();

        Manager.ChangePrompt(promptText);

        for (int i = 0; i < choiceOptions.Length; i++)
        {
            if (!OptionsUI.GetChild(i).gameObject.activeSelf)
                OptionsUI.GetChild(i).gameObject.SetActive(true);

            //print(OptionsUI.GetChild(i).name);

            OptionsUI.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = choiceOptions[i];

            if(OptionsUI.childCount < choiceOptions.Length)
            {
                if (i < choiceOptions.Length - 1)
                {
                    GameObject scenario = Instantiate(OptionsUI.GetChild(i).gameObject, OptionsUI);
                }
            }
        }
    }

    public void Restart()
    {
        foreach (Transform item in transform.parent.GetChild(0))
        {
            item.gameObject.SetActive(false);
        }

        OptionsUI.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = choiceOptions[1];
        promptUI.text = promptText;
    }
}
