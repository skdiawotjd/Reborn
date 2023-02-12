using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;

public class AdventureGameManager : MonoBehaviour
{
    public static AdventureGameManager instance;
    public MGAdventureManager MGManager;
    public PoolManager pool;
    public Spawner spawner;
    public BattleManager battleManager;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {

    }

    public void SetMGAdventureManager(MGAdventureManager temMGAdventureManager)
    {
        MGManager = temMGAdventureManager;
    }
    public void SetPoolManager(PoolManager temPoolManager)
    {
        pool = temPoolManager;
    }
    public void SetSpawner(Spawner temSpawner)
    {
        spawner = temSpawner;
    }
    public void SetBattleManager(BattleManager temBattleManager)
    {
        battleManager = temBattleManager;
    }

    void Update()
    {
        
    }
}
