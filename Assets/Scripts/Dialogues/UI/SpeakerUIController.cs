using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using TMPro;

public class SpeakerUIController : MonoBehaviour
{
    public Image portrait;
    public TMP_Text fullName;
    public TMP_Text dialog;
    //public Mood mood;

    private DialogueCharacter speaker;
    public DialogueCharacter Speaker
    {
        get { return speaker; }
        set
        {
            speaker = value;
            // portrait.sprite = speaker.portrait;
            fullName.text = speaker.fullName;
        }
    }

    public string Dialog
    {
        get { return dialog.text; }
        set { dialog.text = value; }
    }

    /*public Mood Mood
    {
        set
        {
            Sprite sprite;
            if (value == Mood.Angry)
            {
                sprite = speaker.portraitAngry;
            }
            else
            {
                sprite = speaker.portrait;
            }

            portrait.sprite = sprite;
        }
    }*/

    public bool HasSpeaker()
    {
        return speaker != null;
    }

    public bool SpeakerIs(DialogueCharacter character)
    {
        return speaker == character;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}