using System.Collections.Generic;
using UnityEngine;

public class UserDataManager : Singleton<UserDataManager>
{
    private UserDbModel _profileModel;
    private RecordModel _recordModel;
    private WalletModel _walletModel;
    private List<HeroDbModel> _heroesModel = new List<HeroDbModel>();

    public void SetAllUserData(UserDbModel profile, RecordModel record, WalletModel wallet, List<HeroDbModel> heroes)
    {
        _profileModel = profile;
        _recordModel = record;
        _walletModel = wallet;
        _heroesModel = heroes;

        Debug.Log($"[UserDataManager] 캐싱 완료: {profile.nickName}님 환영합니다.");
    }

    public UserDbModel ProfileModel => _profileModel;
    public RecordModel RecordModel => _recordModel;
    public WalletModel WalletModel => _walletModel;
    public List<HeroDbModel> HeroesModel => _heroesModel;
}
