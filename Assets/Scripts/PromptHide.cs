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
