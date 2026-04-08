using UnityEngine;

public class SaturnVictoryTrigger : MonoBehaviour
{
    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !triggered)
        {
            triggered = true;
            Time.timeScale = 0f; // freeze the game
        }
    }

    private void OnGUI()
    {
        if (triggered)
        {
            // dim background
            GUI.color = new Color(0, 0, 0, 0.7f);
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
            GUI.color = Color.white;

            GUIStyle title = new GUIStyle();
            title.fontSize = 70;
            title.fontStyle = FontStyle.Bold;
            title.normal.textColor = Color.yellow;
            title.alignment = TextAnchor.MiddleCenter;
            GUI.Label(new Rect(0, Screen.height / 2 - 100, Screen.width, 100),
                "YOU BEAT THE GAME!", title);

            GUIStyle subtitle = new GUIStyle();
            subtitle.fontSize = 30;
            subtitle.normal.textColor = Color.white;
            subtitle.alignment = TextAnchor.MiddleCenter;
            GUI.Label(new Rect(0, Screen.height / 2, Screen.width, 50),
                "Thanks for playing!", subtitle);

            // restart button
            GUIStyle btn = new GUIStyle(GUI.skin.button);
            btn.fontSize = 25;
            if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 + 80, 200, 60),
                    "Play Again", btn))
            {
                Time.timeScale = 1f;
                UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
            }
        }
    }
}