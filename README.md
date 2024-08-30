
# Bunker Boys

A text adventure with a highly-repeatable template system


## Screenshots

![App Screenshot](https://i.imgur.com/jrkuBpN.jpeg)


## Features

- Isometric view
- Choices matter, with infinite trees of choices
- Fullscreen mode
- Post-apocalyptic theme
- Unique graphical style


## Snippets

</details>

<details>
<summary><code>GameManager.cs</code></summary>

```
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
        promptOriginPos = ScalingParent.localPosition;
    }

    public void NextScenario(int selectedButton)
    {
        // Load the next scenario after maing a choice
        foreach (Transform item in UIOptions) {
            item.gameObject.SetActive(false);
        }

        foreach(Transform item in transform)
            if(item.gameObject.name != "Restart")
                item.gameObject.SetActive(false);

        GameObject nextScenario = transform.GetChild(currentScenarioIndex).GetComponent<SingleChoice>().NextScenario[selectedButton];

        nextScenario.SetActive(true);

        currentScenarioIndex = nextScenario.transform.GetSiblingIndex();
    }

    public void ChooseCheckpoint()
    {
        // Restart the game on button press

        foreach (Transform item in UIOptions) {
            item.gameObject.SetActive(false);
        }

        foreach (Transform item in transform)
            if (item.gameObject.name != "Restart")
                item.gameObject.SetActive(false);

        transform.GetChild(1).gameObject.SetActive(true);

        currentScenarioIndex = 1;
    }

    public void ChangePrompt(string input)
    {
        // Initialize prompt window and change text

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
        // Set the scale and position of the panels
        //  - this is just for show, and just makes the ui fancier

        if (targetScale == Vector3.one) {
            ScalingParent.localScale = Vector3.Lerp(ScalingParent.localScale, targetScale, expandPromptSpeed * Time.deltaTime);
        }

        if (showPromptTimer > 0 || toggledPrompt == true) {
            ScalingParent.localPosition = Vector3.Lerp(ScalingParent.localPosition, promptOriginPos, hidePromptSpeed * Time.deltaTime);

            if (!hoveringPrompt) {
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
        // Determine if hovering over the prompt window

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

```
</details>
</details>

<details>
<summary><code>SingleChoice.cs</code></summary>

```
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
        // Upon activation of a choice object, this script activates:
        // - Player is presented with new prompt text and their choices

        Manager = transform.parent.gameObject.GetComponent<GameManager>();

        Manager.ChangePrompt(promptText);

        for (int i = 0; i < choiceOptions.Length; i++)
        {
            if (!OptionsUI.GetChild(i).gameObject.activeSelf)
                OptionsUI.GetChild(i).gameObject.SetActive(true);

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

```
</details>
</details>

<details>
<summary><code>Selections.cs</code></summary>

```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Selections : MonoBehaviour
{
    public GameObject TransfSelector;

    public float expandSpeed = 8f;

    private Transform managerObj;


    public void SelectButton()
    {
        // Make a selection and a new choice
        // Selector is shown/hidden on mouse hover

        HideSelector();

        managerObj = transform.root.GetChild(2).GetChild(0);

        int selectedButtonIndex = transform.GetSiblingIndex();

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

```
</details>
</details>

<details>
<summary><code>BunkerCam.cs</code></summary>

```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class BunkerCam : MonoBehaviour
{
    private Vector3 scrollInput;

    private Vector3 mouseInput;

    public float moveSpeed = 8f;

    public float smoothing = 4f;

    public float zoomSpeed = 4f;

    private bool autoPanCamera = true;

    public Vector3[] panningWaypoints;

    public float minChangeWaypointDistance = 10f;

    public float panningSpeed = 2f;

    private int currentWaypointIndex = 0;

    private Vector3 panningVelocity;


    private void OnEnable()
    {
        scrollInput = Vector3.one * Input.GetAxis("Mouse ScrollWheel");
        mouseInput = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    }

    void Update()
    {
        // Toggle manual/panning camera

        if (Input.GetKeyDown(KeyCode.Space)) {
            autoPanCamera = !autoPanCamera;
        }

        if (Input.GetMouseButtonDown(0)) {
            mouseInput = Vector3.zero;
        }

        scrollInput = Vector3.Lerp(scrollInput, new Vector3(0, 0, 1000) * Input.GetAxis("Mouse ScrollWheel"), 8 * Time.deltaTime);


        if (!autoPanCamera)
        {
            // Manual camera control (click and drag)

            if (PointerOnScreen())
                mouseInput = Vector3.Lerp(mouseInput, new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0), smoothing * Time.deltaTime);

            Vector3 mouseDragAmount = Vector3.zero;
            if (Input.GetMouseButton(0)) {
                mouseDragAmount = (mouseInput * moveSpeed) + scrollInput;
            }

            transform.localPosition += (mouseDragAmount - scrollInput);
        }
        else
        {
            AutoCam();

            transform.localPosition -= scrollInput;
        }
    }

    private void AutoCam()
    {
        // Auto pan camera through preset waypoints

        Vector3 finalWaypoint = panningWaypoints[currentWaypointIndex];
        finalWaypoint.z = transform.localPosition.z;

        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, finalWaypoint, ref panningVelocity, panningSpeed * Time.deltaTime);

        if ((finalWaypoint - transform.localPosition).magnitude < minChangeWaypointDistance)
        {
            currentWaypointIndex++;

            if (currentWaypointIndex >= panningWaypoints.Length)
            {
                currentWaypointIndex = 0;
            }
        }
    }

    private bool PointerOnScreen()
    {
        Rect screenRect = new Rect(0, 0, Screen.width, Screen.height);
        if (!screenRect.Contains(Input.mousePosition))
            return false;
        else
            return true;
    }
}

```
</details>
</details>

<details>
<summary><code>PromptHide.cs</code></summary>

```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PromptHide : MonoBehaviour
{
    public GameManager Manager;

    public void PointerEnter()
    {
        Manager.SetPromptState(true);
    }

    public void PointerExit()
    {
        Manager.SetPromptState(false);
    }

    public void TogglePrompt()
    {
        Manager.TogglePrompt();
    }
}

```
</details>



