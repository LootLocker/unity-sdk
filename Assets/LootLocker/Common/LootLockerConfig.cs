﻿#if UNITY_EDITOR
using System.IO;
#endif
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LootLocker
{
    public class LootLockerConfig : LootLockerGenericConfig
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
                if(_current == null)
                {
                    _current = Get();
                }

                return _current;
            } 
        }
    }
}
