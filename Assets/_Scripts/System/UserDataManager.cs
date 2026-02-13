using UnityEngine;

public class UserDataManager : Singleton<UserDataManager>
{
    private UserDbModle _userData;

    public void ApplyFromFirestore(UserDbModle data)
    {
        _userData = data;
    }
}
