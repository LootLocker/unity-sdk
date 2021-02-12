﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using LootLocker.Requests;
using Newtonsoft.Json;
using System;
using LootLocker;

namespace LootLockerDemoApp
{
    public class SelectClassScreen : MonoBehaviour, ILootLockerStageOwner
    {
        public Transform parent;
        public GameObject characterClassPrefab;
        CreatePlayerRequest createPlayerRequest;
        public LootLockerLootLockerLoadout loadout;
        LootLockerSessionResponse sessionResponse;
        Guid guid;
        public Button button;
        Action failResponse;
        public PlayerDataObject playerDataObject;

        [Header("Easy Prefab Setup")]
        public bool isEasyPrefab;

        private void Awake()
        {
            StartEasyPrefab();
        }

        public void StartEasyPrefab()
        {
            if (isEasyPrefab)
            {
                SetUpEasyPrefab();
                ListAllCharacterClasses();
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() =>
                {
                    UpdateDefaultCharacterClass(() =>
                    {
                        Debug.Log("Updated the default class for the current player");
                    });
                });
            }
        }

        public void ListAllCharacterClasses()
        {
            //Loadouts are the default items that are associated with each character class. This is why we can use this to list all character classes and then display to user

            LootLockerSDKManager.GetCharacterLoadout((response) =>
            {
                if (response.success)
                {
                    foreach (LootLockerLootLockerLoadout loadout in response.loadouts)
                    {
                        GameObject selectionButton = Instantiate(characterClassPrefab, parent);
                        selectionButton.GetComponent<ClassSelectionButton>()?.Init(loadout);
                    }
                }
                else
                {

                }
                LoadingManager.HideLoadingScreen();
            });
        }

        public void SetUpEasyPrefab()
        {
            if (TexturesSaver.Instance == null)
            {
                GameObject saver = Resources.Load("EasyPrefabsResources/TextureSaver") as GameObject;
                Instantiate(saver);
            }

            if (LoadingManager.Instance == null)
            {
                GameObject loading = Resources.Load("EasyPrefabsResources/LoadingPrefab") as GameObject;
                Instantiate(loading);
            }

            if (PopupSystem.Instance == null)
            {
                GameObject popup = Resources.Load("EasyPrefabsResources/PopupPrefab") as GameObject;
                Instantiate(popup);
            }
        }


        public void UpdateScreenData(ILootLockerStageData stageData)
        {
            if (stageData != null)
            {
                createPlayerRequest = stageData as CreatePlayerRequest;
                playerDataObject.SavePlayer(createPlayerRequest.playerName);
                LoadingManager.ShowLoadingScreen();
                failResponse = () => { StagesManager.instance.GoToStage(StagesManager.StageID.Player, null); };
                //Starting session first before character is chosen
                StartSession(() =>
                {

                    foreach (Transform tr in parent)
                        Destroy(tr.gameObject);


                    LootLockerSDKManager.ListCharacterTypes((response) =>
                    {
                        if (response.success)
                        {
                            int index = 0;
                            foreach (LootLockerCharacter_Types types in response.character_types)
                            {
                                index++;
                                Action<LootLockerCharacterLoadoutResponse> tempListResponse = (listReponse) =>
                                {
                                    if (index == response.character_types.Length)
                                    {
                                        LootLockerSDKManager.GetCharacterLoadout((getLoadOutResponse) =>
                                        {
                                            if (getLoadOutResponse.success)
                                            {
                                                foreach (LootLockerLootLockerLoadout loadout in getLoadOutResponse.loadouts)
                                                {
                                                    GameObject selectionButton = Instantiate(characterClassPrefab, parent);
                                                    selectionButton.GetComponent<ClassSelectionButton>()?.Init(loadout);
                                                }
                                            }
                                            else
                                            {
                                                StagesManager.instance.GoToStage(StagesManager.StageID.CreatePlayer, null);
                                            }
                                            LoadingManager.HideLoadingScreen();
                                        });
                                    }
                                };
                                LootLockerSDKManager.CreateCharacter(types.id.ToString(), types.name, types.is_default, tempListResponse);
                            }
                        }
                        else
                        {
                            StagesManager.instance.GoToStage(StagesManager.StageID.CreatePlayer, null);
                        }
                        LoadingManager.HideLoadingScreen();
                    });


                });
                //if we are creating a new character then we want to set character details once it is created
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() =>
                {
                    UpdateDefaultCharacterClass(() =>
                    {
                        if (!isEasyPrefab)
                        {
                            LocalPlayer localPlayer = new LocalPlayer { playerName = createPlayerRequest.playerName, uniqueID = guid.ToString(), characterClass = loadout?.character };
                            List<LocalPlayer> localPlayers = JsonConvert.DeserializeObject<List<LocalPlayer>>(PlayerPrefs.GetString("localplayers"));
                            localPlayers.Add(localPlayer);
                            PlayerPrefs.SetString("localplayers", JsonConvert.SerializeObject(localPlayers));
                            LootLockerConfig.current.deviceID = localPlayer.uniqueID;
                           // playerDataObject._playerClass = loadout.character.type.ToString();
                        //Character has been set, we can now load the home page
                        StagesManager.instance.GoToStage(StagesManager.StageID.Home, sessionResponse);
                        }
                        else
                        {
                            Debug.Log("Updated the default class for the current player");
                        }
                    });
                });
            }
            else
            {
                failResponse = () => { StagesManager.instance.GoToStage(StagesManager.StageID.Settings, null); };

                foreach (Transform tr in parent)
                    Destroy(tr.gameObject);

                LootLockerSDKManager.GetCharacterLoadout((response) =>
                {
                    if (response.success)
                    {
                        foreach (LootLockerLootLockerLoadout loadout in response.loadouts)
                        {
                            GameObject selectionButton = Instantiate(characterClassPrefab, parent);
                            selectionButton.GetComponent<ClassSelectionButton>()?.Init(loadout);
                        }
                    }
                    else
                    {
                        StagesManager.instance.GoToStage(StagesManager.StageID.Settings, null);
                    }
                    LoadingManager.HideLoadingScreen();
                });
                //if we are just updating the character class for player, then after it is completed. We want to return to the inventory screen
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() =>
                {
                    UpdateDefaultCharacterClass(() =>
                    {
                    //Character has been set, we can now load inventory
                    StagesManager.instance.GoToStage(StagesManager.StageID.Settings, null);
                    });
                });
            }
        }

        public void UpdateDefaultCharacterClass(Action onCompletedUpdate)
        {
            ////now that we have a new player created, we need to set the default character of this player to the one that was selected

            //LootLockerSDKManager.UpdateCharacter(loadout.character.id.ToString(), playerDataObject._playerName, true, (updateResponse) =>
            //{
            //    if (updateResponse.success)
            //    {
            //        playerDataObject._playerClass = loadout.character.type;
            //        Debug.Log("Updated character info successfully: " + updateResponse.text);
            //        onCompletedUpdate?.Invoke();
            //        LoadingManager.HideLoadingScreen();
            //    }
            //    else
            //    {
            //        failResponse?.Invoke();
            //        Debug.LogError("Failed to update character info: " + updateResponse.text);
            //        LoadingManager.HideLoadingScreen();
            //    }

            //});
        }

        public void StartSession(Action OnCompletedSessionStart)
        {
            guid = Guid.NewGuid();

            LoadingManager.ShowLoadingScreen();
            //Starting a new session using the new id that has been created
            LootLockerSDKManager.StartSession(guid.ToString(), (response) =>
            {
                if (response.success)
                {
                    playerDataObject.SaveSession(response);
                    sessionResponse = response;
                    Debug.Log("Session success: " + response.text);
                    OnCompletedSessionStart?.Invoke();
                }
                else
                {
                    failResponse?.Invoke();
                    Debug.LogError("Session failure: " + response.text);
                    LoadingManager.HideLoadingScreen();
                }

            });
        }

    }
}
