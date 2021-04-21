﻿using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker;
using LootLocker.Requests;

namespace LootLocker.Requests
{

    public class LootLockerGetPlayerInfoResponse : LootLockerResponse
    {
        public bool success { get; set; }
        public int? account_balance { get; set; }
        public int? xp { get; set; }
        public int? level { get; set; }
        public LootLockerLevel_Thresholds level_thresholds { get; set; }
    }

    [System.Serializable]
    public class LootLockerStandardResponse : LootLockerResponse
    {
        public bool success { get; set; }
    }

    [System.Serializable]
    public class LootLockerDlcResponse : LootLockerResponse
    {
        public bool success { get; set; }
        public string[] dlcs { get; set; }
    }

    [System.Serializable]
    public class LootLockerDeactivatedAssetsResponse : LootLockerResponse
    {
        public bool success { get; set; }
        public LootLockerDeactivatedObjects[] objects { get; set; }
    }
    [System.Serializable]
    public class LootLockerDeactivatedObjects
    {
        public int deactivated_asset_id { get; set; }
        public int replacement_asset_id { get; set; }
        public string reason { get; set; }
    }


    [System.Serializable]
    public class LootLockerBalanceResponse : LootLockerResponse
    {
        public bool success { get; set; }
        public int? balance { get; set; }
    }

    [System.Serializable]
    public class LootLockerXpSubmitResponse : LootLockerResponse
    {
        public bool success { get; set; }
        public LootLockerXp xp { get; set; }
        public LootLockerLevel[] levels { get; set; }
        public bool check_grant_notifications { get; set; }
    }

    [System.Serializable]
    public class LootLockerXpResponse : LootLockerResponse
    {
        public bool success { get; set; }
        public int? xp { get; set; }
        public int? level { get; set; }
    }
    [System.Serializable]
    public class LootLockerXp
    {
        public int? previous { get; set; }
        public int? current { get; set; }
    }
    [System.Serializable]
    public class LootLockerLevel
    {
        public int? level { get; set; }
        public int? xp_threshold { get; set; }
    }

    [System.Serializable]

    public class LootLockerInventoryResponse : LootLockerResponse
    {

        public bool success;

        public LootLockerInventory[] inventory;

    }
    public class LootLockerInventory
    {
        public int instance_id { get; set; }
        public int? variation_id { get; set; }
        public string rental_option_id { get; set; }
        public string acquisition_source { get; set; }
        public LootLockerCommonAsset asset { get; set; }
        public LootLockerRental rental { get; set; }


        public float balance;
    }
    [System.Serializable]
    public class LootLockerAssetClass
    {
        public string Asset { get; set; }
    }
    [System.Serializable]
    public class LootLockerRental
    {
        public bool is_rental { get; set; }
        public string time_left { get; set; }
        public string duration { get; set; }
        public string is_active { get; set; }
    }
    [System.Serializable]
    public class LootLockerXpSubmitRequest
    {
        public int? points;

        public LootLockerXpSubmitRequest(int points)
        {
            this.points = points;
        }
    }
    [System.Serializable]
    public class LootLockerXpRequest : LootLockerGetRequest
    {
        public LootLockerXpRequest()
        {
            getRequests.Clear();
            getRequests.Add(LootLockerConfig.current.deviceID);
            getRequests.Add(LootLockerConfig.current.platform.ToString());
        }
    }

    public class LootLockerPlayerAssetNotificationsResponse : LootLockerResponse
    {
        public bool success { get; set; }
        public LootLockerRewardObject[] objects { get; set; }
    }

    public class LootLockerRewardObject
    {
        public int instance_id { get; set; }
        public int variation_id { get; set; }
        public string acquisition_source { get; set; }
        public LootLockerCommonAsset asset { get; set; }
    }
}

namespace LootLocker
{
    public partial class LootLockerAPIManager
    {
        public static void GetPlayerInfo(Action<LootLockerGetPlayerInfoResponse> onComplete)
        {
            EndPointClass endPoint = LootLockerEndPoints.current.getPlayerInfo;

            LootLockerServerRequest.CallAPI(endPoint.endPoint, endPoint.httpMethod, null, onComplete: (serverResponse) =>
            {
                LootLockerGetPlayerInfoResponse response = new LootLockerGetPlayerInfoResponse();
                if (string.IsNullOrEmpty(serverResponse.Error))
                    response = JsonConvert.DeserializeObject<LootLockerGetPlayerInfoResponse>(serverResponse.text);

                //LootLockerSDKManager.DebugMessage(serverResponse.text, !string.IsNullOrEmpty(serverResponse.Error));
                response.text = serverResponse.text;
                     response.status = serverResponse.status;
            response.Error = serverResponse.Error; response.statusCode = serverResponse.statusCode;
                onComplete?.Invoke(response);
            }, true);
        }

        public static void GetInventory(Action<LootLockerInventoryResponse> onComplete)
        {
            EndPointClass endPoint = LootLockerEndPoints.current.getInventory;

            LootLockerServerRequest.CallAPI(endPoint.endPoint, endPoint.httpMethod, null, onComplete: (serverResponse) =>
               {
                   LootLockerInventoryResponse response = new LootLockerInventoryResponse();
                   if (string.IsNullOrEmpty(serverResponse.Error))
                       response = JsonConvert.DeserializeObject<LootLockerInventoryResponse>(serverResponse.text);

                   //LootLockerSDKManager.DebugMessage(serverResponse.text, !string.IsNullOrEmpty(serverResponse.Error));
                   response.text = serverResponse.text;
                        response.status = serverResponse.status;
               response.Error = serverResponse.Error; response.statusCode = serverResponse.statusCode;
                   onComplete?.Invoke(response);
               }, true);
        }

        public static void GetBalance(Action<LootLockerBalanceResponse> onComplete)
        {
            EndPointClass endPoint = LootLockerEndPoints.current.getCurrencyBalance;

            LootLockerServerRequest.CallAPI(endPoint.endPoint, endPoint.httpMethod, null, onComplete: (serverResponse) =>
            {
                LootLockerBalanceResponse response = new LootLockerBalanceResponse();
                if (string.IsNullOrEmpty(serverResponse.Error))
                    response = JsonConvert.DeserializeObject<LootLockerBalanceResponse>(serverResponse.text);

                //LootLockerSDKManager.DebugMessage(serverResponse.text, !string.IsNullOrEmpty(serverResponse.Error));
                response.text = serverResponse.text;
                     response.status = serverResponse.status;
            response.Error = serverResponse.Error; response.statusCode = serverResponse.statusCode;
                onComplete?.Invoke(response);
            }, true);
        }

        public static void SubmitXp(LootLockerXpSubmitRequest data, Action<LootLockerXpSubmitResponse> onComplete)
        {
            string json = "";
            if (data == null) return;
            else json = JsonConvert.SerializeObject(data);

            EndPointClass endPoint = LootLockerEndPoints.current.submitXp;

            LootLockerServerRequest.CallAPI(endPoint.endPoint, endPoint.httpMethod, json, (serverResponse) =>
            {
                LootLockerXpSubmitResponse response = new LootLockerXpSubmitResponse();
                if (string.IsNullOrEmpty(serverResponse.Error))
                    response = JsonConvert.DeserializeObject<LootLockerXpSubmitResponse>(serverResponse.text);

                //LootLockerSDKManager.DebugMessage(serverResponse.text, !string.IsNullOrEmpty(serverResponse.Error));
                response.text = serverResponse.text;
                     response.status = serverResponse.status;
            response.Error = serverResponse.Error; response.statusCode = serverResponse.statusCode;
                onComplete?.Invoke(response);
            }, true);
        }

        public static void GetXpAndLevel(LootLockerGetRequest data, Action<LootLockerXpResponse> onComplete)
        {
            EndPointClass endPoint = LootLockerEndPoints.current.getXpAndLevel;

            string getVariable = string.Format(endPoint.endPoint, data.getRequests[0], data.getRequests[1]);

            LootLockerServerRequest.CallAPI(getVariable, endPoint.httpMethod, null, (serverResponse) =>
            {
                LootLockerXpResponse response = new LootLockerXpResponse();
                if (string.IsNullOrEmpty(serverResponse.Error))
                    response = JsonConvert.DeserializeObject<LootLockerXpResponse>(serverResponse.text);

                //LootLockerSDKManager.DebugMessage(serverResponse.text, !string.IsNullOrEmpty(serverResponse.Error));
                response.text = serverResponse.text;
                     response.status = serverResponse.status;
            response.Error = serverResponse.Error; response.statusCode = serverResponse.statusCode;
                onComplete?.Invoke(response);
            }, true);
        }

        public static void GetPlayerAssetNotification(Action<LootLockerPlayerAssetNotificationsResponse> onComplete)
        {
            EndPointClass endPoint = LootLockerEndPoints.current.playerAssetNotifications;

            string getVariable = endPoint.endPoint;

            LootLockerServerRequest.CallAPI(getVariable, endPoint.httpMethod, null, (serverResponse) =>
            {
                LootLockerPlayerAssetNotificationsResponse response = new LootLockerPlayerAssetNotificationsResponse();
                if (string.IsNullOrEmpty(serverResponse.Error))
                    response = JsonConvert.DeserializeObject<LootLockerPlayerAssetNotificationsResponse>(serverResponse.text);

                //LootLockerSDKManager.DebugMessage(serverResponse.text, !string.IsNullOrEmpty(serverResponse.Error));
                response.text = serverResponse.text;
                     response.status = serverResponse.status;
            response.Error = serverResponse.Error; response.statusCode = serverResponse.statusCode;
                onComplete?.Invoke(response);
            }, true);
        }

        public static void GetDeactivatedAssetNotification(Action<LootLockerDeactivatedAssetsResponse> onComplete)
        {
            EndPointClass endPoint = LootLockerEndPoints.current.playerAssetDeactivationNotification;

            string getVariable = endPoint.endPoint;

            LootLockerServerRequest.CallAPI(getVariable, endPoint.httpMethod, null, (serverResponse) =>
            {
                LootLockerDeactivatedAssetsResponse response = new LootLockerDeactivatedAssetsResponse();
                if (string.IsNullOrEmpty(serverResponse.Error))
                    response = JsonConvert.DeserializeObject<LootLockerDeactivatedAssetsResponse>(serverResponse.text);

                //LootLockerSDKManager.DebugMessage(serverResponse.text, !string.IsNullOrEmpty(serverResponse.Error));
                response.text = serverResponse.text;
                     response.status = serverResponse.status;
            response.Error = serverResponse.Error; response.statusCode = serverResponse.statusCode;
                onComplete?.Invoke(response);
            }, true);
        }

        public static void InitiateDLCMigration(Action<LootLockerDlcResponse> onComplete)
        {
            EndPointClass endPoint = LootLockerEndPoints.current.initiateDlcMigration;

            string getVariable = endPoint.endPoint;
            LootLockerServerRequest.CallAPI(getVariable, endPoint.httpMethod, null, (serverResponse) =>
            {
                LootLockerDlcResponse response = new LootLockerDlcResponse();
                if (string.IsNullOrEmpty(serverResponse.Error))
                    response = JsonConvert.DeserializeObject<LootLockerDlcResponse>(serverResponse.text);

                //LootLockerSDKManager.DebugMessage(serverResponse.text, !string.IsNullOrEmpty(serverResponse.Error));
                response.text = serverResponse.text;
                     response.status = serverResponse.status;
            response.Error = serverResponse.Error; response.statusCode = serverResponse.statusCode;
                onComplete?.Invoke(response);
            }, true);
        }

        public static void GetDLCMigrated(Action<LootLockerDlcResponse> onComplete)
        {
            EndPointClass endPoint = LootLockerEndPoints.current.getDlcMigration;

            string getVariable = endPoint.endPoint;

            LootLockerServerRequest.CallAPI(getVariable, endPoint.httpMethod, null, (serverResponse) =>
            {
                LootLockerDlcResponse response = new LootLockerDlcResponse();
                if (string.IsNullOrEmpty(serverResponse.Error))
                    response = JsonConvert.DeserializeObject<LootLockerDlcResponse>(serverResponse.text);

                //LootLockerSDKManager.DebugMessage(serverResponse.text, !string.IsNullOrEmpty(serverResponse.Error));
                response.text = serverResponse.text;
                     response.status = serverResponse.status;
            response.Error = serverResponse.Error; response.statusCode = serverResponse.statusCode;
                onComplete?.Invoke(response);
            }, true);
        }

        public static void SetProfilePrivate(Action<LootLockerStandardResponse> onComplete)
        {
            EndPointClass endPoint = LootLockerEndPoints.current.setProfilePrivate;

            string getVariable = endPoint.endPoint;

            LootLockerServerRequest.CallAPI(getVariable, endPoint.httpMethod, null, (serverResponse) =>
            {
                LootLockerStandardResponse response = new LootLockerStandardResponse();
                if (string.IsNullOrEmpty(serverResponse.Error))
                    response = JsonConvert.DeserializeObject<LootLockerStandardResponse>(serverResponse.text);

                //LootLockerSDKManager.DebugMessage(serverResponse.text, !string.IsNullOrEmpty(serverResponse.Error));
                response.text = serverResponse.text;
                     response.status = serverResponse.status;
            response.Error = serverResponse.Error; response.statusCode = serverResponse.statusCode;
                onComplete?.Invoke(response);
            }, true);
        }

        public static void SetProfilePublic(Action<LootLockerStandardResponse> onComplete)
        {
            EndPointClass endPoint = LootLockerEndPoints.current.setProfilePublic;

            string getVariable = endPoint.endPoint;

            LootLockerServerRequest.CallAPI(getVariable, endPoint.httpMethod, null, (serverResponse) =>
            {
                LootLockerStandardResponse response = new LootLockerStandardResponse();
                if (string.IsNullOrEmpty(serverResponse.Error))
                    response = JsonConvert.DeserializeObject<LootLockerStandardResponse>(serverResponse.text);

                //LootLockerSDKManager.DebugMessage(serverResponse.text, !string.IsNullOrEmpty(serverResponse.Error));
                response.text = serverResponse.text;
                     response.status = serverResponse.status;
            response.Error = serverResponse.Error; response.statusCode = serverResponse.statusCode;
                onComplete?.Invoke(response);
            }, true);
        }

    }

}