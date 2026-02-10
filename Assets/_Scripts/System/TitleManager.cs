using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField emailField;
    [SerializeField] private TMP_InputField pwField;
    public FirebaseAuth auth; // 인증을 위한 객체
    public FirebaseUser user; // 인증된 유저 정보

    void Start()
    {
        Init();
    }

    private void Init()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().
            ContinueWithOnMainThread(task =>
            {
                if (task.Result != DependencyStatus.Available)
                {
                    Debug.LogError($"Firebase Depenedency Error : {task.Result}");
                    return;
                }
                auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
            });
    }

    public void TryToLogin()
    {
        string email = emailField.text;
        string pw = pwField.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(pw))
        {
            Debug.Log("아이디 또는 비밀번호가 비어있음.");
            return;
        }

        auth.SignInWithEmailAndPasswordAsync(email, pw).
            ContinueWithOnMainThread(task => 
            {
                if(task.IsFaulted)
                {
                    Debug.Log("로그인 오류");
                    return;
                }
                if(task.IsCanceled)
                {
                    Debug.Log("로그인 취소");
                    return;
                }

                user = task.Result.User;
            });
    }

    public void TryToSignUp()
    {
        string email = emailField.text;
        string pw = pwField.text;

        auth.CreateUserWithEmailAndPasswordAsync(email, pw).
            ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.Log("회원가입 오류");
                    return;
                }
                if (task.IsCanceled)
                {
                    Debug.Log("회원가입 취소");
                    return;
                }

                user = task.Result.User;
            });
    }
}
