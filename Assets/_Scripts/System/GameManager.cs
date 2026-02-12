using UnityEngine;

public enum GameState { Ready,Play} //추후 상태 추가가능
public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameState _currentState = GameState.Ready;

    //다른데서 참조할 게임시작여부
    public bool IsGameStarted => _currentState == GameState.Play;

    public void ChangeState(GameState newState)
    {
        if(_currentState == newState) return;

        _currentState = newState;

        switch (_currentState)
        {
            case GameState.Ready:
                //증강 선택 UI같은거 띄우기
                break;
            case GameState.Play:
                //모든 로직 가동
                break;
        }
    }

    public void OnAugmentSelectionComplete() //초기 증강 선택 완료 버튼에서 호출할 메서드
    {
        ChangeState(GameState.Play);
    }
}
