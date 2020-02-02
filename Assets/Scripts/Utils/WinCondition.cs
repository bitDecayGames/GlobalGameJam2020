using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinCondition : MonoBehaviour {
    public int WinAmount = 100000;
    public TextMeshProUGUI WinDisplayText;

    private Wallet wallet;
    private int winConditionDisplayCount = 0;

    private void Start() {
        wallet = FindObjectOfType<Wallet>();
    }

    private void Update() {
        if (wallet.TotalMoney >= WinAmount + (WinAmount * winConditionDisplayCount)) {
            ShowWinCondition();
        }
    }

    public void ShowWinCondition() {
        Debug.Log("Win Condition has been met!!!");
        winConditionDisplayCount += 1;
        WinDisplayText.text = "You made " + MoneyConverter.IntToMoney(wallet.TotalMoney) + " dollars!!!";
        MarkAllChildrenActive(true);
    }

    public void OnClickQuickButton() {
        SceneManager.LoadScene("Credits");
    }

    public void OnClickKeepPlaying() {
        MarkAllChildrenActive(false);
    }

    private void MarkAllChildrenActive(bool isActive) {
        foreach (Transform child in transform) {
            child.gameObject.SetActive(isActive);
        }
    }
}