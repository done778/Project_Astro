using Fusion;
using UnityEngine;
using UnityEngine.UI;

public class MatchMakingRunner : SimulationBehaviour
{
    private Button _cancelBtn;

    public void SetCancelButton(Button btn)
    {
        _cancelBtn = btn;
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
        Debug.Log("OnConnectedToServer 실행됨.");
        _cancelBtn.interactable = true;
    }
}
