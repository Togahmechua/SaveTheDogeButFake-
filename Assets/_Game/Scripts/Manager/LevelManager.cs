using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private static LevelManager ins;
    public static LevelManager Ins => ins;
    public Level level;
    public List<Level> levelList = new List<Level>();
    public int curMap;
    public LineCtrl lineRendererObj;
    public bool timesUp;

    private List<Level> curLevelList = new List<Level>();

    private void Awake()
    {
        LevelManager.ins = this;
        OnInit();
    }

    public void OnInit()
    {
        curMap = PlayerPrefs.GetInt("CurrentMap", 0);
    }

    public void StartGame()
    {
        level.playerCtrl.rb.simulated = true;
        level.beehive.isActive = true;
    }

    public void RestartGame()
    {
        level.playerCtrl.rb.simulated = false;
        level.beehive.isActive = false;
    }

    public void LoadMapByID(int id)
    {
        if (level != null)
        {
            DespawnMap();
        }

        foreach (Level lv in levelList)
        {
            if (lv.id == id)
            {
                level = SimplePool.Spawn<Level>(levelList[id]);
                lineRendererObj.gameObject.SetActive(true);
                lineRendererObj.transform.position = new Vector3(0f, 0f, -2f);
                lineRendererObj.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                RestartGame();
                curLevelList.Add(level);
            }
        }
    }

    public void DespawnMap()
    {
        if (level != null)
        {
            foreach (Level lv in curLevelList)
            {
                SimplePool.Despawn(lv);
            }
            lineRendererObj.gameObject.SetActive(false);
            level.beehive.DeleteBee();
            curLevelList.Clear();
            level = null;
            timesUp = false;
        }
    }
}
