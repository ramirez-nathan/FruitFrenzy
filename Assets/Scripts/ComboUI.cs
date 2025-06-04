using System.Collections;
using TMPro;
using UnityEngine;

public class ComboUI : MonoBehaviour
{
    public TextMeshProUGUI comboText;
    public TextMeshProUGUI comboNum;
    private void Awake()
    {
        StartCoroutine(Lifespan());
    }
    public void SetCombo(int comboCount)
    {
        string ComboText = $"{comboCount} Fruit Combo!";
        string ComboNum = $"+{comboCount}";
        comboText.text = ComboText;
        comboNum.text = ComboNum;
    }
    private IEnumerator Lifespan()
    {
        yield return new WaitForSeconds(1.0f);
        Destroy(gameObject);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
