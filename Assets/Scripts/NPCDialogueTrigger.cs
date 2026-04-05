using UnityEngine;

public class NPCDialogueTrigger : MonoBehaviour
{
    [TextArea(2, 5)]
    public string[] dialogueLines;

    public bool triggerOnlyOnce = true;

    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        if (triggerOnlyOnce && hasTriggered)
            return;

        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.StartDialogue(dialogueLines);
            hasTriggered = true;
        }
    }
}