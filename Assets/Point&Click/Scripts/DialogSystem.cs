using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogSystem : MonoBehaviour
{
    public static DialogSystem Instance { get; private set; }

    [SerializeField] TMPro.TextMeshProUGUI messageText;
    [SerializeField] GameObject panel;

    private List<string> currentMessages = new List<string>();
    private int msgId = 0;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        panel.SetActive(false);
    }

    public void ShowMessages(List<string> messages, bool dialog, List<Actions> yesActions = null, List<Actions> noActions = null)
    {
        currentMessages = messages;

        panel.SetActive(true);

        StartCoroutine(ShowMultipleMessages());
    }

    IEnumerator ShowMultipleMessages()
    {
        messageText.text = currentMessages[msgId];

        while(msgId < currentMessages.Count)
        {
            if(Input.GetKeyDown(KeyCode.Space) || (Input.GetMouseButtonDown(0) && Extensions.IsMouseOverUI()))  
            {
                msgId++;

                if(msgId < currentMessages.Count)
                    messageText.text = currentMessages[msgId];
            }

            yield return null;
        }

        panel.SetActive(false);
        msgId = 0;
    }
}
