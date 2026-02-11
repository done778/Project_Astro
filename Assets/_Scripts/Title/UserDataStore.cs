using Firebase.Firestore;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[FirestoreData]
public class UserData
{
    [FirestoreProperty] public string uuid { get; set; }
    [FirestoreProperty] public string nickName { get; set; }
    [FirestoreProperty] public int win { get; set; }
    [FirestoreProperty] public int lose { get; set; }
}

// Firestore 유저 데이터

public class UserDataStore : MonoBehaviour
{
    private FirebaseFirestore _firestore;
    private const string COLLECTION_NAME = "users";

    public void Initialize()
    {
        _firestore = FirebaseFirestore.DefaultInstance;
        Debug.Log("[Firestore] Firestore initialized");
    }

    // 새 유저 데이터 생성
    public async Task CreateUserDataAsync(string uuid, string nickname)
    {
        var data = new Dictionary<string, object>
        {
            { "uuid", uuid },
            { "nickName", nickname },
            { "win", 0 },
            { "lose", 0 }
        };

        await _firestore
            .Collection(COLLECTION_NAME)
            .Document(uuid)
            .SetAsync(data);

        Debug.Log($"[Firestore] User data created for {nickname}");
    }

    // 유저 데이터 조회
    public async Task<UserData> GetUserDataAsync(string uuid)
    {
        var snapshot = await _firestore
            .Collection(COLLECTION_NAME)
            .Document(uuid)
            .GetSnapshotAsync();

        if (!snapshot.Exists)
        {
            Debug.LogWarning($"[Firestore] User data not found for UUID: {uuid}");
            return null;
        }

        var userData = snapshot.ConvertTo<UserData>();

        // TODO : UserDataManager에 적용 (싱글톤 패턴 사용 시)
        //if (UserDataManager.Instance != null)
        //{
        //    UserDataManager.Instance.ApplyFromFirestore(
        //        userData.uuid,
        //        userData.nickName,
        //        userData.win,
        //        userData.lose
        //    );
        //}

        return userData;
    }

    // 닉네임 중복 체크
    public async Task<bool> IsNicknameDuplicateAsync(string nickname)
    {
        var query = await _firestore
            .Collection(COLLECTION_NAME)
            .WhereEqualTo("nickName", nickname)
            .GetSnapshotAsync();

        return query.Count > 0;
    }

    // 유저 데이터 업데이트
    public async Task UpdateUserDataAsync(string uuid, Dictionary<string, object> updates)
    {
        await _firestore
            .Collection(COLLECTION_NAME)
            .Document(uuid)
            .UpdateAsync(updates);

        Debug.Log($"[Firestore] User data updated for UUID: {uuid}");
    }

    // 승률 기록 업데이트
    public async Task UpdateWinLoseAsync(string uuid, int winDelta, int loseDelta)
    {
        var docRef = _firestore.Collection(COLLECTION_NAME).Document(uuid);

        await _firestore.RunTransactionAsync(async transaction =>
        {
            var snapshot = await transaction.GetSnapshotAsync(docRef);
            var currentWin = snapshot.GetValue<int>("win");
            var currentLose = snapshot.GetValue<int>("lose");

            transaction.Update(docRef, new Dictionary<string, object>
            {
                { "win", currentWin + winDelta },
                { "lose", currentLose + loseDelta }
            });
        });
    }
}
