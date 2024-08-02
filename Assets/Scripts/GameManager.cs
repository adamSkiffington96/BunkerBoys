using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;


public class GameManager : MonoBehaviour
{
    public Transform UIOptions;

    private int currentScenarioIndex = 0;

    public TextMeshProUGUI promptText;

    public Transform ScalingParent;

    private Vector3 targetScale = Vector3.zero;

    public float expandPromptSpeed = 4f;

    private bool hoveringPrompt = false;

    private bool toggledPrompt = false;

    private Vector3 promptOriginPos;
    public Vector3 promptHiddenPosition = Vector3.zero;

    public float hidePromptSpeed = 10f;

    private float showPromptTimer = 0f;


    private void OnEnable()
    {
        //ScalingParent = promptText.gameObject.transform.parent;
        promptOriginPos = ScalingParent.localPosition;
    }

    public void NextScenario(int selectedButton)
    {
        foreach (Transform item in UIOptions) {
            item.gameObject.SetActive(false);
        }

        foreach(Transform item in transform)
            if(item.gameObject.name != "Restart")
                item.gameObject.SetActive(false);

        GameObject nextScenario = transform.GetChild(currentScenarioIndex).GetComponent<SingleChoice>().NextScenario[selectedButton];

        nextScenario.SetActive(true);

        currentScenarioIndex = nextScenario.transform.GetSiblingIndex();

        //print(" (scenarioIndex " + currentScenarioIndex + ")");
    }

    public void ChooseCheckpoint()
    {
        foreach (Transform item in UIOptions) {
            item.gameObject.SetActive(false);
        }

        foreach (Transform item in transform)
            if (item.gameObject.name != "Restart")
                item.gameObject.SetActive(false);

        //GameObject nextScenario = transform.GetChild(1).GetComponent<SingleChoice>().NextScenario[selectedButton];

        //nextScenario.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(true);

        currentScenarioIndex = 1;
    }



    public void ChangePrompt(string input)
    {
        if (input != "" && input != null)
        {
            ScalingParent.gameObject.SetActive(true);

            promptText.text = input;

            targetScale = Vector3.one;
        }
        else
        {
            targetScale = Vector3.zero;

            ScalingParent.gameObject.SetActive(false);
        }

        ScalingParent.localScale = Vector3.zero;

        toggledPrompt = true;
    }

    private void Update()
    {
        if (targetScale == Vector3.one) {
            ScalingParent.localScale = Vector3.Lerp(ScalingParent.localScale, targetScale, expandPromptSpeed * Time.deltaTime);

            //print("Attempting to set prompt scale to 0");
        }

        if (showPromptTimer > 0 || toggledPrompt == true) {
            ScalingParent.localPosition = Vector3.Lerp(ScalingParent.localPosition, promptOriginPos, hidePromptSpeed * Time.deltaTime);

            if (!hoveringPrompt)
            {
                if(showPromptTimer > 0)
                    showPromptTimer -= Time.deltaTime;
            }
                
        }
        else {
            ScalingParent.localPosition = Vector3.Lerp(ScalingParent.localPosition, promptHiddenPosition, hidePromptSpeed * Time.deltaTime);
        }

        print(hoveringPrompt);
    }

    public void SetPromptState(bool state)
    {
        hoveringPrompt = state;
        if(state == true)
        {
            showPromptTimer = 2f;
        }
    }

    public void TogglePrompt()
    {
        toggledPrompt = !toggledPrompt;
    }
}
