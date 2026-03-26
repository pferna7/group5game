using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ShopTrigger : MonoBehaviour
{
    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !triggered)
        {
            triggered = true;
            StartCoroutine(LoadShop());
        }
    }

    private IEnumerator LoadShop()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("ShopScene");
    }

    private void OnGUI()
    {
        if (triggered)
        {
            GUIStyle style = new GUIStyle();
            style.fontSize = 40;
            style.fontStyle = FontStyle.Bold;
            style.normal.textColor = Color.yellow;
            style.alignment = TextAnchor.MiddleCenter;
            GUI.Label(new Rect(0, Screen.height / 2 - 30, Screen.width, 60),
                "You received 100 coins!", style);
        }
    }
}