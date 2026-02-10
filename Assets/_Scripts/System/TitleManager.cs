using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using Firebase.Firestore;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField emailField;
    [SerializeField] private TMP_InputField pwField;
    public FirebaseAuth auth; // 인증을 위한 객체
    public FirebaseUser user; // 인증된 유저 정보

    FirebaseFirestore _database;

    void Awake()
    {
        _database = FirebaseFirestore.DefaultInstance;
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

    public void TryToFirestoreCreate()
    {
        DocumentReference docRef = _database.Collection("users").Document("alovelace");
        Dictionary<string, object> dict = new Dictionary<string, object>
        {
            {"First", "Ada" },
            {"Last", "Lovelace" },
            {"Born", 1815 }
        };

        docRef.SetAsync(dict).ContinueWithOnMainThread(task =>
        {
            Debug.Log("alovelace 유저 데이터 저장됨.");
        });
    }

    public void TryToFirestoreAdd()
    {
        DocumentReference docRef = _database.Collection("users").Document("aturing");
        Dictionary<string, object> dict = new Dictionary<string, object>
        {
            {"First", "Alan" },
            { "Middle", "Mathison" },
            {"Last", "Turing" },
            {"Born", 1912 }
        };

        docRef.SetAsync(dict).ContinueWithOnMainThread(task =>
        {
            Debug.Log("aturing 유저 데이터 저장됨.");
        });
    }

    public void TryToFirestoreRead() 
    {
        CollectionReference usersRef = _database.Collection("users");
        usersRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            QuerySnapshot snapshot = task.Result;
            foreach (DocumentSnapshot document in snapshot.Documents)
            {
                Debug.Log(string.Format("User: {0}", document.Id));
                Dictionary<string, object> documentDictionary = document.ToDictionary();
                Debug.Log(String.Format("First: {0}", documentDictionary["First"]));
                if (documentDictionary.ContainsKey("Middle"))
                {
                    Debug.Log(String.Format("Middle: {0}", documentDictionary["Middle"]));
                }

                Debug.Log(String.Format("Last: {0}", documentDictionary["Last"]));
                Debug.Log(String.Format("Born: {0}", documentDictionary["Born"]));
            }

            Debug.Log("Read all data from the users collection.");
        });
    }
}
