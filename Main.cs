global using static AutoSavePoints.Logger;
using ModLoader.Framework;
using ModLoader.Framework.Attributes;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using VTOLAPI;

/*
 * Expectation:
 * Port the old Auto Save Points mod to the VTOL VR Mod Loader Steam Edition™
 * 
 * Reality:
 * Only took about a day after real documentation showed up about how to do this, I attempted to steal code from the Persistant Save mod but that was fruitless as I don't do any
 * Hamrony patching, real waste of time there.
 * 
 * End Goal:
 * Once setttings become real again (I can steal more code) I can add that back.
 * 
 * Future Plans:
 * None.
 * 
 * Changelog:
 * 12/11/2024: Ported the old mod to VTOL VR Mod Loader Steam Edition™ and removed user settings.
 */

namespace AutoSavePoints
{
    [ItemId("Mattidg.AutoSavePoints")]
    public class Main : VtolMod
    {
        private int missionsCompletedAmount;
        public string ModFolder;

        List<MissionObjective> missionsList = new List<MissionObjective>();
        List<MissionObjective> missionsListUnique = new List<MissionObjective>();
        HashSet<MissionObjective> addedListeners = new HashSet<MissionObjective>();

        private void Awake()
        {
            ModFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Log($"Awake at {ModFolder}");

            VTAPI.SceneLoaded += SceneLoaded;
        }

        private void AddMissions()
        {
            missionsList = new List<MissionObjective>(VTAPI.FindObjectsOfType<MissionObjective>());
            SetListeners();
        }

        private void SetListeners()
        {
            missionsListUnique = missionsList.Distinct().ToList();

            foreach (MissionObjective objective in missionsListUnique)
            {
                if (!addedListeners.Contains(objective))
                {
                    objective.OnComplete.AddListener(() => MissionAutoSave());
                    addedListeners.Add(objective);
                }
            }

            missionsListUnique.Clear();
            missionsList.Clear();
        }

        private void MissionAutoSave()
        {
            missionsCompletedAmount++;

            if (missionsCompletedAmount == 0)
            {
                Log("LMAOXD you can't divide by 0 dawg.");
            }
            else if (missionsCompletedAmount % 2 == 0)
            {
                QuicksaveManager quickSaveInstance;
                quickSaveInstance = QuicksaveManager.instance;
                quickSaveInstance.Quicksave();
                missionsCompletedAmount = 0;
            }
        }

        private void TimeAutoSave()
        {
            QuicksaveManager quickSaveInstance;
            quickSaveInstance = QuicksaveManager.instance;
            quickSaveInstance.Quicksave();
        }

        private void SceneLoaded(VTScenes scene)
        {
            CancelInvoke("AddMissions");

            CancelInvoke("TimeAutoSave");

            InvokeRepeating("AddMissions", 60, 30);

            InvokeRepeating("TimeAutoSave", 60, 300);

            missionsCompletedAmount = 0;

            missionsList.Clear();
        }

        public override void UnLoad()
        {
            // Destroy any objects
        }
    }
}