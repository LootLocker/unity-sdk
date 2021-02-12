﻿using LootLocker.Requests;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;
using LootLocker;

namespace LootLockerDemoApp
{
    public class ReadMessageScreen : MonoBehaviour, ILootLockerStageOwner
    {
        public Image messageImage;
        public Text messageTitle, messageSummary, messageBody;
        [Header("Easy Prefab Setup")]
        public bool isEasyPrefab;
        public GameObject messageScreen;

        public void StartEasyPrefab(ILootLockerStageData stageData)
        {
            GetComponent<ScreenOpener>()?.Open();
            UpdateScreenData(stageData);
        }


        public void UpdateScreenData(ILootLockerStageData stageData)
        {
            LoadingManager.ShowLoadingScreen();
            LootLockerGMMessage selectedMessage = stageData as LootLockerGMMessage;
            if (!string.IsNullOrEmpty(selectedMessage.image))
            {
                messageImage.gameObject.SetActive(true);
                _ = DownloadImage(selectedMessage.image, messageImage);
            }
            else
            {
                messageImage.gameObject.SetActive(false);
                LoadingManager.HideLoadingScreen();
            }
            messageSummary.text = selectedMessage.summary ?? "";
            messageBody.text = selectedMessage.body ?? "";
            messageTitle.text = selectedMessage.title ?? "";
        }

        #region Image Download Handling

        public async Task<bool> DownloadImage(string MediaUrl, Image targetImage)
        {

            targetImage.sprite = null;

            try
            {
                Texture2D downloadedTexture = await GetRemoteTexture(MediaUrl, targetImage);
                targetImage.sprite = Sprite.Create(downloadedTexture, new Rect(0, 0, downloadedTexture.width, downloadedTexture.height), Vector2.zero);
                LoadingManager.HideLoadingScreen();
                return true;
            }
            catch (Exception ex)
            {

                Debug.LogError("Couldn't download image. " + ex);
                LoadingManager.HideLoadingScreen();
                return false;
            }
        }

        public void Back()
        {
            if (!isEasyPrefab)
                StagesManager.instance.GoToStage(StagesManager.StageID.Messages, null);
            else
            {
                GetComponent<ScreenCloser>()?.Close();
                messageScreen?.GetComponent<MessagesScreen>()?.StartEasyPrefab();
            }
        }

        public async Task<Texture2D> GetRemoteTexture(string url, Image targetImage)
        {

            using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
            {
                //begin request:
                var asyncOp = www.SendWebRequest();

                //await until it's done: 
                while (asyncOp.isDone == false)
                {
                    await Task.Delay(1000 / 30);//30 hertz
                }

                //read results:
                if (www.isNetworkError || www.isHttpError)
                {
                    //log error:
#if DEBUG
                    Debug.Log($"{ www.error }, URL:{ www.url }");
#endif

                    //nothing to return on error:
                    return null;
                }
                else
                {
                    //return valid results:
                    return DownloadHandlerTexture.GetContent(www);
                }
            }

        }


        #endregion

    }
}
