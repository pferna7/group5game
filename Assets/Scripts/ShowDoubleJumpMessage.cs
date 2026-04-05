using UnityEngine;
using TMPro;
using System.Collections;

public class ShowDoubleJumpMessage : MonoBehaviour
{
    public TextMeshProUGUI doubleJumpText;
    public float delayBeforeShow = 1f;
    public float displayTime = 3f;

    private Coroutine currentRoutine;

    private void Awake()
    {
        if (doubleJumpText != null)
        {
            doubleJumpText.enabled = false;
        }
    }

    public void ShowMessageNow()
    {
        if (doubleJumpText == null)
        {
            Debug.LogWarning("Double jump text is not assigned.");
            return;
        }

        if (currentRoutine != null)
        {
            StopCoroutine(currentRoutine);
        }

        currentRoutine = StartCoroutine(ShowMessageRoutine());
    }

    private IEnumerator ShowMessageRoutine()
    {
        doubleJumpText.enabled = false;

        yield return new WaitForSeconds(delayBeforeShow);

        doubleJumpText.enabled = true;

        yield return new WaitForSeconds(displayTime);

        doubleJumpText.enabled = false;

        currentRoutine = null;
    }
}