using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

public class StateMgr {

    QFSMLite QFS_StateMgr;

    private GridArray gridArray = null;
    KeyCode[] key = { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5 };

    private float timeCt;
    bool elliminateflag = false;

    void Shoot(params object[] param)
    {

    }

    void Eliminate(params object[] param)
    {
        //本次是否有消除操作
        bool isEliminate = false;

        List<List<Stone>> lt = gridArray.CalculateArray();

        foreach (List<Stone> t in lt)
        {
            var type = t[0].type;
            var len = t.Count;

            if (t[0].size == 1 && len <= 2) { continue; } //
            if (t[0].size == 2 && len <= 1) { continue; } //

            //
            foreach (Stone s in t)
            {
                gridArray.ClearStonPosition(s);
                s.Eliminate();
                isEliminate = true;
            }
        }

        if (isEliminate)
        {
            QFS_StateMgr.AddTranslation("stat_eliminate", "init", "stat_supplyAndDrop", SupplyAndDrop);
        }
        else
        {
            QFS_StateMgr.AddTranslation("stat_eliminate", "init", "stat_shot", Shoot);
        }
    }

    void SupplyAndDrop(params object[] param)
    {
        gridArray.DropFillEmpty();
        gridArray.FeedNewSton();

        QFS_StateMgr.AddTranslation("stat_supplyAndDrop", "init", "stat_eliminate", Eliminate);
    }

    public void init () {

        timeCt = 0.0f;
        gridArray = Battlefield.Instance.GetGridArray();

        QFS_StateMgr = new QFSMLite();

        // 添加状态
        QFS_StateMgr.AddState("stat_none");
        QFS_StateMgr.AddState("stat_shot");
        QFS_StateMgr.AddState("stat_eliminate");
        QFS_StateMgr.AddState("stat_supplyAndDrop");

        QFS_StateMgr.AddTranslation("stat_none", "init", "stat_shot", Shoot);

        QFS_StateMgr.Start("stat_shot");
    }

    public void update(float dt)
    {
        timeCt += dt;
        if (GetCurrentState() == "stat_shot")
        {
            if (gridArray == null) return;

            for (var i = 0; i < key.Length; i++)
            {
                if ( Input.GetKeyDown(key[i])&&elliminateflag == false)
                {
                    //删除本次击中的格子
                    Stone s = gridArray.GetNearBottomStone(i);
                    if (s != null)
                    {
                        gridArray.ClearStonPosition(s);
                        s.Eliminate();
                        elliminateflag = true;
                        timeCt = 0.0f;
                    }
                }
            }

            //过一定时间才填充。
            if (elliminateflag && timeCt > 0.2f)
            {
                // 添加跳转
                QFS_StateMgr.AddTranslation("stat_shot", "init", "stat_supplyAndDrop", SupplyAndDrop);
                elliminateflag = false;
                timeCt = 0.0f;
            }

            QFS_StateMgr.HandleEvent("init", "abc");
        }
        else if (GetCurrentState() == "stat_eliminate")
        {
            QFS_StateMgr.HandleEvent("init", "def");
        }
        else if (GetCurrentState() == "stat_supplyAndDrop")
        {
            QFS_StateMgr.HandleEvent("init", "efg");
        }
    }

    public void changeState(string state)
    {

    }

    public string GetCurrentState()
    {
        return QFS_StateMgr.State;//_CurrentState.getStateName();

    }

}