using UnityEngine;
using TMPro;

public class MenuUIManager : MonoBehaviour
{
    public TMP_InputField nameInputField;

    // Make sure this is `public`
    public void OnStartPressed()
    {
        Debug.Log($"[MenuUIManager] OnStartPressed() called; input='{nameInputField.text}'");
        string name = string.IsNullOrWhiteSpace(nameInputField.text)
                      ? "Player"
                      : nameInputField.text.Trim();
        GameManager.Instance.playerName = name;
        GameManager.Instance.StartGame();
    }
}