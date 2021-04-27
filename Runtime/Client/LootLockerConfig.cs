﻿using LootLocker.Requests;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace LootLocker
{

    public class LootLockerConfig : ScriptableObject
    {

#if UNITY_EDITOR
        static LootLockerConfig()
        {
            ProjectSettingsBuildProcessor.OnBuild += OnProjectSettingsBuild;
        }

        private static void OnProjectSettingsBuild(List<ScriptableObject> list, List<string> names)
        {
            list.Add(Get());
            names.Add("LootLockerConfig");
        }
#endif

        private static LootLockerConfig settingsInstance;

        public string SettingsPath
        {
            get
            {
#if UNITY_EDITOR
                return $"{ProjectSettingsConsts.ROOT_FOLDER}/{SettingName}.asset";
#else
                return $"{ProjectSettingsConsts.PACKAGE_NAME}/{SettingName}";
#endif
            }
        }

        public virtual string SettingName { get { return "LootLockerConfig"; } }

        public static LootLockerConfig Get()
        {
            if (settingsInstance != null)
            {
                return settingsInstance;
            }

            LootLockerConfig tempInstance = CreateInstance<LootLockerConfig>();
#if UNITY_EDITOR
            string path = tempInstance.SettingsPath;

            if (!File.Exists(path))
            {
                settingsInstance = CreateInstance<LootLockerConfig>();
                ProjectSettingsHelper.Save(settingsInstance, path);
            }
            else
            {
                settingsInstance = ProjectSettingsHelper.Load<LootLockerConfig>(path);
            }

            settingsInstance.hideFlags = HideFlags.HideAndDontSave;
            return settingsInstance;
#else
            settingsInstance = Resources.Load<LootLockerConfig>(tempInstance.SettingsPath);
            return settingsInstance;
#endif
        }

        public static bool CreateNewSettings(string apiKey, string gameVersion, platformType platform, bool onDevelopmentMode)
        {
            settingsInstance = CreateInstance<LootLockerConfig>();
            settingsInstance.apiKey = apiKey;
            settingsInstance.game_version = gameVersion;
            settingsInstance.platform = platform;
            settingsInstance.developmentMode = onDevelopmentMode;

            return true;
        }

#if UNITY_EDITOR
        public void EditorSave()
        {
            ProjectSettingsHelper.Save(settingsInstance, SettingsPath);
        }
#endif

        private static LootLockerConfig _current;

        public static LootLockerConfig current
        {
            get
            {
                if (_current == null)
                {
                    _current = Get();
                }

                return _current;
            }
        }

        public string apiKey;
        [HideInInspector]
        public string token;
        [HideInInspector]
        public int gameID;
        public string game_version = "1.0";
        [HideInInspector]
        public string deviceID = "defaultPlayerId";
        public platformType platform;
        public enum platformType { Android, iOS, Steam, Windows, GoG, Xbox, PlayStationNetwork, EpicStore, NintendoSwitch, Web ,Other }

        public bool developmentMode;
        [HideInInspector]
        public string url = "https://api.lootlocker.io/game/v1";
        [HideInInspector]
        public string adminUrl = "https://api.lootlocker.io/admin";
        [HideInInspector]
        public string playerUrl = "https://api.lootlocker.io/player";
        [HideInInspector]
        public string userUrl = "https://api.lootlocker.io/game";
        public enum DebugLevel { All, ErrorOnly, NormalOnly, Off }
        public DebugLevel currentDebugLevel;
        public bool allowTokenRefresh = true;

        public void UpdateToken(string _token, string _player_identifier)
        {
            token = _token;
            deviceID = _player_identifier;
        }

    }
}