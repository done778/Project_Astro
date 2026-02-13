using Fusion;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MatchMakingRunner : SimulationBehaviour, IPlayerJoined
{
    private Button _cancelBtn;
    private NetworkRunner _networkRunner;

    public void Initialize(Button btn, NetworkRunner runner)
    {
        _cancelBtn = btn;
        _networkRunner = runner;
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
        Debug.Log("OnConnectedToServer 실행됨.");
        _cancelBtn.interactable = true;
    }

    public void PlayerJoined(PlayerRef player) 
    {
        Debug.Log($"플레이어 입장: {player.PlayerId}");

        if (_networkRunner.ActivePlayers.Count() == 2)
        {
            Debug.Log("플레이어 2명 입장! 매칭 완료");
            SceneManager.LoadScene("Stage");
        }
    }
}
