using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;

    private string[] currentLines;
    private int currentLineIndex;
    private bool dialogueActive = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }
    }

    private void Update()
    {
        if (!dialogueActive)
            return;

        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space))
        {
            ShowNextLine();
        }
    }

    public void StartDialogue(string[] lines)
    {
        if (lines == null || lines.Length == 0)
            return;

        currentLines = lines;
        currentLineIndex = 0;
        dialogueActive = true;

        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(true);
        }

        if (dialogueText != null)
        {
            dialogueText.text = currentLines[currentLineIndex];
        }
    }

    public void ShowNextLine()
    {
        currentLineIndex++;

        if (currentLineIndex >= currentLines.Length)
        {
            EndDialogue();
            return;
        }

        if (dialogueText != null)
        {
            dialogueText.text = currentLines[currentLineIndex];
        }
    }

    public void EndDialogue()
    {
        dialogueActive = false;

        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }
    }

    public bool IsDialogueActive()
    {
        return dialogueActive;
    }
}