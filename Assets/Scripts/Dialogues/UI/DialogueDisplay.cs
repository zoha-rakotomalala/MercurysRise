using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueDisplay : MonoBehaviour
{
    public Conversation conversation;
    public GameObject speakerLeft;
    public GameObject speakerRight;

    private SpeakerUIController speakerUILeft;
    private SpeakerUIController speakerUIRight;

    private int activeLineIndex = 0;

    private void Start()
    {
        speakerUILeft = speakerLeft.GetComponent<SpeakerUIController>();
        speakerUIRight = speakerRight.GetComponent<SpeakerUIController>();

        speakerUILeft.Speaker = conversation.speakerLeft;
        speakerUIRight.Speaker = conversation.speakerRight;

        // Start the level by displaying the conversation
        DisplayLine();
        activeLineIndex += 1;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && activeLineIndex!=-1)
        {
            AdvanceConversation();
        }
    }

    private void AdvanceConversation() { 
        if (activeLineIndex < conversation.lines.Length)
        {
            DisplayLine();
            activeLineIndex += 1;
        }
        else
        {
            speakerUILeft.Hide();
            speakerUIRight.Hide();
            activeLineIndex= -1;
        }
    }

    private void DisplayLine()
    {
        Line line = conversation.lines[activeLineIndex];
        DialogueCharacter character = line.character;

        if (speakerUILeft.SpeakerIs(character))
        {
            SetDialog(speakerUILeft, speakerUIRight, line.text);
        }
        else
        {
            SetDialog(speakerUIRight, speakerUILeft, line.text);
        }
    }

    private void SetDialog(SpeakerUIController activeSpeakerUI, SpeakerUIController inactiveSpeakerUI, string text)
    {
        activeSpeakerUI.dialog.text = text;
        activeSpeakerUI.Show();
        inactiveSpeakerUI.Hide();
    }

}
