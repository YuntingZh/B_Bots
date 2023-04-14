using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI characterNameText;
    public TextMeshProUGUI dialogueText;
    public Image characterImage;
    public Button startButton;

    public Image kryloxImage;
    public Image blotImage;

    private Dialogue[] dialogue = new Dialogue[]
    {
        new Dialogue("Krylox", "What...where am I? What's going on?", true),
        new Dialogue("Blot", "Krylox, you're finally online. Listen, we've got a problem. The authorities have discovered our safe house and are closing in. We need to evacuate now.", false),
        new Dialogue("Krylox", "What?! But...how did they find us?", true),
        new Dialogue("Blot", "I don't know, but there's no time to waste. We need to meet up at the rendezvous point and get out of here before it's too late. You're our only hope, Krylox. Don't let us down.", false),
        new Dialogue("Krylox", "I won't. I'll do whatever it takes to get us out of here.", true)
    };

    private int currentDialogueIndex = 0;

    private void Start()
    {
        // Set the initial dialogue to Krylox's first line
        SetDialogue(dialogue[0]);
    }

    private void Update()
    {
        // Check for input to advance the dialogue
        if (Input.GetMouseButtonDown(0))
        {
            currentDialogueIndex++;

            // If we've reached the end of the dialogue, show the start button
            if (currentDialogueIndex >= dialogue.Length)
            {
                startButton.gameObject.SetActive(true);
            }
            else
            {
                SetDialogue(dialogue[currentDialogueIndex]);
            }
        }
    }

    private void SetDialogue(Dialogue currentDialogue)
    {
        characterNameText.text = currentDialogue.characterName;
        dialogueText.text = currentDialogue.dialogue;
        characterImage.sprite = currentDialogue.isKrylox ? kryloxImage.sprite : blotImage.sprite;
    }

    private class Dialogue
    {
        public string characterName;
        public string dialogue;
        public bool isKrylox;

        public Dialogue(string characterName, string dialogue, bool isKrylox)
        {
            this.characterName = characterName;
            this.dialogue = dialogue;
            this.isKrylox = isKrylox;
        }
    }
}
