global using static AutoSavePoints.Logger;
using ModLoader.Framework;
using ModLoader.Framework.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using Valve.Newtonsoft.Json;
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
 * Once settings become real again (I can steal more code) I can add that back.
 * 
 * Future Plans:
 * None.
 * 
 * Changelog:
 * 06/18/2025: Added a settings file for User Settings.
 * 12/11/2024: Ported the old mod to VTOL VR Mod Loader Steam Edition™ and removed user settings.
 */

namespace AutoSavePoints
{
    [ItemId("Mattidg.AutoSavePoints")]
    public class Main : VtolMod
    {
        //User Variables
        private bool showSaveWarning = false;
        private int autoSaveTime = 300;
        private int autoSaveMissions = 2;

        private int missionsCompletedAmount;
        public string ModFolder;

        List<MissionObjective> missionsList = new List<MissionObjective>();
        List<MissionObjective> missionsListUnique = new List<MissionObjective>();
        HashSet<MissionObjective> addedListeners = new HashSet<MissionObjective>();

        private void Awake()
        {
            ModFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Log($"Awake at {ModFolder}");

            var settings = LoadSettings();
            showSaveWarning = settings.showSaveWarning;
            autoSaveTime = settings.autoSaveTime;
            autoSaveMissions = settings.autoSaveMissions;

            VTAPI.SceneLoaded += SceneLoaded;
        }

        private AutoSavePointsSettings LoadSettings()
        {
            string settingsPath = Path.Combine(ModFolder, "AutoSavePointsSettings.json");

            try
            {
                if (!File.Exists(settingsPath))
                {
                    var defaultSettings = new AutoSavePointsSettings();
                    File.WriteAllText(settingsPath, JsonSerializer.Serialize(defaultSettings, new JsonSerializerOptions { WriteIndented = true }));
                    Log("Default settings file created.");
                    return defaultSettings;
                }

                string json = File.ReadAllText(settingsPath);
                return JsonSerializer.Deserialize<AutoSavePointsSettings>(json) ?? new AutoSavePointsSettings();
            }
            catch (Exception ex)
            {
                Log($"Failed to load settings: {ex.Message}");
                return new AutoSavePointsSettings();
            }
        }

        private void AddMissions()
        {
            missionsList.Clear();
            missionsList.AddRange(VTAPI.FindObjectsOfType<MissionObjective>());

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
        }

        private void MissionAutoSave()
        {
            missionsCompletedAmount++;

            if (missionsCompletedAmount == 0)
            {
                Log("LMAOXD you can't divide by 0 dawg.");
            }
            else if (missionsCompletedAmount % autoSaveMissions == 0)
            {
                QuicksaveManager.instance.Quicksave();
                missionsCompletedAmount = 0;
            }

            if (showSaveWarning)
            {
                TutorialLabel.instance.DisplayLabel(FlightSceneManager.instance.playerActor.designation.ToString() + "AutoSavePoints Mission: Auto-saved after " + autoSaveMissions + " missions completed", null, 7);
            }
        }

        private void TimeAutoSave()
        {
            QuicksaveManager.instance.Quicksave();

            if (showSaveWarning)
            {
                TutorialLabel.instance.DisplayLabel(FlightSceneManager.instance.playerActor.designation.ToString() + "AutoSavePoints: Auto-saved after " + autoSaveTime / 60 + " minutes", null, 7);
            }
        }

        private void SceneLoaded(VTScenes scene)
        {
            CancelInvoke("AddMissions");

            CancelInvoke("TimeAutoSave");

            InvokeRepeating("AddMissions", 60, 30);

            InvokeRepeating("TimeAutoSave", 60, autoSaveTime);

            missionsCompletedAmount = 0;

            missionsList.Clear();
        }

        public override void UnLoad()
        {
            // Destroy any objects
        }
    }
}