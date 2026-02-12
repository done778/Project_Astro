using UnityEngine;

public class UserDataManager : Singleton<UserDataManager>
{
    private UserData _userData;

    public void ApplyFromFirestore(UserData data)
    {
        _userData = data;
    }
}
