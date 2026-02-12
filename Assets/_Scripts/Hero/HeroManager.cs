using System.Collections.Generic;
using UnityEngine;

public class HeroManager : Singleton<HeroManager>
{
    private Dictionary<string, HeroStatusHandler> _activeHeroes = new Dictionary<string, HeroStatusHandler>();

    // 1. 씬에 있는 핸들러들 딕셔너리에 등록
    public void RegisterHeroHandler(string heroId, HeroStatusHandler handler)
    {
        if (!_activeHeroes.ContainsKey(heroId))
        {
            _activeHeroes.Add(heroId, handler);
        }
    }

    // 2. Firestore에서 가져온 리스트 한번에 주입 (데이터 브릿지)
    public void InitAllHeroes(List<HeroDbModel> dbHeroList)
    {
        foreach (var dbData in dbHeroList)
        {
            if (_activeHeroes.TryGetValue(dbData.heroId, out var handler))
            {
                handler.Initialize(dbData.level, dbData.exp, dbData.isUnlocked);
            }
        }
        Debug.Log($"[HeroManager] {dbHeroList.Count}종의 영웅 데이터 동기화 완료.");
    }

    // 3. 특정 영웅만 레벨업 시 사용
    public void SyncWithDB(string heroId, int level)
    {
        if (_activeHeroes.TryGetValue(heroId, out var handler))
        {
            handler.Initialize(level, handler.Exp, true); // 레벨만 갱신
        }
    }
}
