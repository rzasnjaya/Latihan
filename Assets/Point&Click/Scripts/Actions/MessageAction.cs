using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageAction : Actions
{
    [Multiline(5)]
    [SerializeField] List<string> message;
    [SerializeField] bool enableDialog;
    [SerializeField] string yesText, noText;
    [SerializeField] List<Actions> yesActions, noActions;

    public override void Act()
    {
        DialogSystem.Instance.ShowMessages(message, enableDialog, yesActions, noActions, yesText, noText);
    }
}