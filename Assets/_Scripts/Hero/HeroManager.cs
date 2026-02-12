using System.Collections.Generic;

public class HeroManager : Singleton<HeroManager>
{
    private Dictionary<string, HeroStatusHandler> _activeHeros = new Dictionary<string, HeroStatusHandler>();

    public void SyncWithDB(string heroId, int level)
    {
        if(_activeHeros.TryGetValue(heroId, out var handler))
        {
            handler.Initialize(level);
        }
    }
}
