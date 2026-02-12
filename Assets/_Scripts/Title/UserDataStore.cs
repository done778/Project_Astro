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

    public List<HeroDbModel> heroList = new List<HeroDbModel>();
}

[FirestoreData]
public class HeroDbModel
{
    [FirestoreProperty] public string heroId { get; set; }
    [FirestoreProperty] public int level { get; set; }
    [FirestoreProperty] public int exp { get; set; }
    [FirestoreProperty] public bool isUnlocked { get; set; }
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
        DocumentReference userDocRef = _firestore.Collection(COLLECTION_NAME).Document(uuid);

        var userData = new Dictionary<string, object>
        {
            { "uuid", uuid },
            { "nickName", nickname },
            { "win", 0 },
            { "lose", 0 },
        };

        try
        {
            // 1. 상기 기본 데이터 생성
            await userDocRef.SetAsync(userData);

            // 2. CSV 테이블의 HeroId를 전부 추가.(일단 직접 기입)
            string[] heroIdList = { "Hero_Knight", "Hero_Archer", "Hero_Priest" };

            foreach (string heroId in heroIdList)
            {
                var heroData = new Dictionary<string, object>
                {
                    { "isUnlock", false },
                    { "level", 1 },
                    { "exp", 0 }
                };

                // 서브 컬렉션에 영웅들 정보 생성
                await userDocRef.Collection("Hero").Document(heroId).SetAsync(heroData);
            }

            Debug.Log($"[Firestore] 유저 '{nickname}' 생성 및 기본 영웅 {heroIdList.Length}종 생성 완료");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[Firestore] 초기화 실패: {e.Message}");
        }
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

    // 영웅의 값 변화에 따른 이벤트성 업데이트
    public async Task UpdateHeroDataAsync(string uuid, string heroId, int level, int exp, bool isUnlock)
    {
        DocumentReference heroDocRef = _firestore
        .Collection(COLLECTION_NAME)
        .Document(uuid)
        .Collection("Hero")
        .Document(heroId);

        Dictionary<string, object> updates = new Dictionary<string, object>
        {
            { "level", level },
            { "exp", exp },
            { "isUnlock", isUnlock }
        };

        try
        {
            await heroDocRef.UpdateAsync(updates);
    
            Debug.Log($"[Firestore] 영웅 {heroId} 업데이트 완료 (Level: {level}, exp: {exp}, isUnlock = {isUnlock})");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[Firestore] 영웅 업데이트 실패: {e.Message}");
        }
    }

    // 영웅별 정보 조회(로그인 시, 게임 종료 시 사용)
    public async Task<List<HeroDbModel>> GetUserHeroDataAsync(string uuid)
    {
        List<HeroDbModel> heroList = new List<HeroDbModel>();

        // 경로: users/{uuid}/Hero (서브컬렉션)
        QuerySnapshot heroQuerySnapshot = await _firestore
            .Collection(COLLECTION_NAME)
            .Document(uuid)
            .Collection("Hero")
            .GetSnapshotAsync();

        if (heroQuerySnapshot.Count == 0)
        {
            Debug.LogWarning($"[Firestore] 유저 {uuid}에게 등록된 영웅 데이터가 없습니다.");
            return heroList;
        }

        foreach (DocumentSnapshot heroDoc in heroQuerySnapshot.Documents)
        {
            if (heroDoc.Exists)
            {
                HeroDbModel heroData = heroDoc.ConvertTo<HeroDbModel>();
                heroData.heroId = heroDoc.Id;
                heroList.Add(heroData);
            }
        }

        Debug.Log($"[Firestore] 유저 {uuid}로부터 {heroList.Count}개의 영웅 정보를 로드했습니다.");
        return heroList;
    }
}
