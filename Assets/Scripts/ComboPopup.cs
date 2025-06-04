using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class ComboPopup : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI comboText;
    public TextMeshProUGUI pointsText;
    public Image backgroundImage;

    [Header("Animation Settings")]
    public float displayDuration = 2f;
    public float fadeInTime = 0.3f;
    public float fadeOutTime = 0.5f;
    public float scaleEffect = 1.2f;
    public Color comboColor = Color.green;

    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        rectTransform = GetComponent<RectTransform>();

        // Start invisible
        canvasGroup.alpha = 0f;
    }

    public void ShowCombo(int comboSize, int totalPoints)
    {
        // Set text
        if (comboText != null)
        {
            comboText.text = $"+{comboSize} COMBO";
            comboText.color = comboColor;
        }

        if (pointsText != null)
        {
            pointsText.text = $"+{totalPoints}";
        }

        // Start animation sequence
        StartCoroutine(AnimatePopup());
    }

    private IEnumerator AnimatePopup()
    {
        // Fade in with scale effect
        float elapsedTime = 0f;
        Vector3 startScale = Vector3.one * 0.5f;
        Vector3 targetScale = Vector3.one * scaleEffect;

        while (elapsedTime < fadeInTime)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / fadeInTime;

            // Ease out animation
            float easedProgress = 1f - Mathf.Pow(1f - progress, 3f);

            canvasGroup.alpha = easedProgress;
            rectTransform.localScale = Vector3.Lerp(startScale, targetScale, easedProgress);

            yield return null;
        }

        // Hold at full visibility
        canvasGroup.alpha = 1f;
        rectTransform.localScale = targetScale;

        // Wait for display duration
        yield return new WaitForSeconds(displayDuration - fadeInTime - fadeOutTime);

        // Scale back to normal size
        elapsedTime = 0f;
        while (elapsedTime < 0.2f)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / 0.2f;
            rectTransform.localScale = Vector3.Lerp(targetScale, Vector3.one, progress);
            yield return null;
        }

        // Fade out
        elapsedTime = 0f;
        while (elapsedTime < fadeOutTime)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / fadeOutTime;

            canvasGroup.alpha = 1f - progress;

            yield return null;
        }

        // Destroy popup
        Destroy(gameObject);
    }
}