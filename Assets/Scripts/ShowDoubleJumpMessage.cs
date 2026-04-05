using UnityEngine;
using TMPro;
using System.Collections;

public class ShowDoubleJumpMessage : MonoBehaviour
{
    public TextMeshProUGUI doubleJumpText;
    public float displayTime = 3f;

    private void Start()
    {
        StartCoroutine(ShowMessage());
    }

    private IEnumerator ShowMessage()
    {
        if (doubleJumpText != null)
        {
            doubleJumpText.gameObject.SetActive(true);
            yield return new WaitForSeconds(displayTime);
            doubleJumpText.gameObject.SetActive(false);
        }
    }
}