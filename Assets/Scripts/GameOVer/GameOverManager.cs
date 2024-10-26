using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    public Image fadeImage; 
    public TextMeshProUGUI gameOverText; 
    public float fadeDuration = 1.0f;
    public Button restartButton;

    private void Start()
    {
        fadeImage.color = new Color(0, 0, 0, 0);
        gameOverText.gameObject.SetActive(false); 
        restartButton.gameObject.SetActive(false);
    }

    public void FadeToBlack()
    {
        Debug.Log("Fading to black");
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        Color fadeColor = fadeImage.color;
        
        while (elapsedTime < fadeDuration)
        {
            fadeColor.a = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
            fadeImage.color = fadeColor;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        fadeColor.a = 1;
        fadeImage.color = fadeColor;

        // display GAME OVER text
        fadeImage.raycastTarget = false;
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        
        // Pause the game
        Time.timeScale = 0f;
    }

    public void ResetGame() {
        Debug.Log("Resetting game");
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        gameOverText.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        fadeImage.color = new Color(0, 0, 0, 0);
    }
}
