﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker;
using LootLocker.Requests;
using LootLocker.Newtonsoft.Json;
using System;

namespace LootLocker.Requests
{
    public class LootLockerPurchaseRequests
    {

    }

    public class LootLockerNormalPurchaseRequest
    {
        public int asset_id { get; set; }
        public int variation_id { get; set; }
    }

    public class LootLockerRentalPurchaseRequest
    {
        public int asset_id { get; set; }
        public int variation_id { get; set; }
        public int rental_option_id { get; set; }
    }


    public class LootLockerPurchaseResponse : LootLockerResponse
    {
        public bool success { get; set; }
        public bool overlay { get; set; }
        public int order_id { get; set; }
    }

    public class LootLockerIosPurchaseVerificationRequest
    {
        public string receipt_data { get; set; }
    }

    public class LootLockerAndroidPurchaseVerificationRequest
    {
        public int asset_id { get; set; }
        public string purchase_token { get; set; }
    }
}

namespace LootLocker
{
 
        public partial class LootLockerAPIManager
        {
            public static void NormalPurchaseCall(LootLockerNormalPurchaseRequest[] data, Action<LootLockerPurchaseResponse> onComplete)
            {
                string json = "";
                if (data == null) return;
                else json = JsonConvert.SerializeObject(data);

                EndPointClass endPoint = LootLockerEndPoints.current.normalPurchaseCall;

                LootLockerServerRequest.CallAPI(endPoint.endPoint, endPoint.httpMethod, json, (serverResponse) =>
                {
                    LootLockerPurchaseResponse response = new LootLockerPurchaseResponse();
                    if (string.IsNullOrEmpty(serverResponse.Error))
                    {
                        response = JsonConvert.DeserializeObject<LootLockerPurchaseResponse>(serverResponse.text);
                        response.text = serverResponse.text;
                        onComplete?.Invoke(response);
                    }
                    else
                    {
                        response.message = serverResponse.message;
                        response.Error = serverResponse.Error;
                        onComplete?.Invoke(response);
                    }
                }, true);
            }

            public static void RentalPurchaseCall(LootLockerRentalPurchaseRequest data, Action<LootLockerPurchaseResponse> onComplete)
            {
                string json = "";
                if (data == null) return;
                else json = JsonConvert.SerializeObject(data);

                EndPointClass endPoint = LootLockerEndPoints.current.rentalPurchaseCall;

                LootLockerServerRequest.CallAPI(endPoint.endPoint, endPoint.httpMethod,json, (serverResponse) =>
                {
                    LootLockerPurchaseResponse response = new LootLockerPurchaseResponse();
                    if (string.IsNullOrEmpty(serverResponse.Error))
                    {
                        response = JsonConvert.DeserializeObject<LootLockerPurchaseResponse>(serverResponse.text);
                        response.text = serverResponse.text;
                        onComplete?.Invoke(response);
                    }
                    else
                    {
                        response.message = serverResponse.message;
                        response.Error = serverResponse.Error;
                        onComplete?.Invoke(response);
                    }
                }, true);
            }

            public static void IosPurchaseVerification(LootLockerIosPurchaseVerificationRequest[] data, Action<LootLockerPurchaseResponse> onComplete)
            {
                string json = "";
                if (data == null) return;
                else json = JsonConvert.SerializeObject(data);

                EndPointClass endPoint = LootLockerEndPoints.current.iosPurchaseVerification;

                LootLockerServerRequest.CallAPI(endPoint.endPoint, endPoint.httpMethod, json, (serverResponse) =>
                {
                    LootLockerPurchaseResponse response = new LootLockerPurchaseResponse();
                    if (string.IsNullOrEmpty(serverResponse.Error))
                    {
                        response = JsonConvert.DeserializeObject<LootLockerPurchaseResponse>(serverResponse.text);
                        response.text = serverResponse.text;
                        onComplete?.Invoke(response);
                    }
                    else
                    {
                        response.message = serverResponse.message;
                        response.Error = serverResponse.Error;
                        onComplete?.Invoke(response);
                    }
                }, true);
            }

            public static void AndroidPurchaseVerification(LootLockerAndroidPurchaseVerificationRequest[] data, Action<LootLockerPurchaseResponse> onComplete)
            {
                string json = "";
                if (data == null) return;
                else json = JsonConvert.SerializeObject(data);

                EndPointClass endPoint = LootLockerEndPoints.current.androidPurchaseVerification;

                LootLockerServerRequest.CallAPI(endPoint.endPoint, endPoint.httpMethod, json, (serverResponse) =>
                {
                    LootLockerPurchaseResponse response = new LootLockerPurchaseResponse();
                    if (string.IsNullOrEmpty(serverResponse.Error))
                    {
                        response = JsonConvert.DeserializeObject<LootLockerPurchaseResponse>(serverResponse.text);
                        response.text = serverResponse.text;
                        onComplete?.Invoke(response);
                    }
                    else
                    {
                        response.message = serverResponse.message;
                        response.Error = serverResponse.Error;
                        onComplete?.Invoke(response);
                    }
                }, true);
            }

            public static void PollingOrderStatus(LootLockerGetRequest lootLockerGetRequest, Action<LootLockerCharacterLoadoutResponse> onComplete)
            {
                EndPointClass endPoint = LootLockerEndPoints.current.pollingOrderStatus;

                string getVariable = string.Format(endPoint.endPoint, lootLockerGetRequest.getRequests[0]);

                LootLockerServerRequest.CallAPI(getVariable, endPoint.httpMethod, "", (serverResponse) =>
                {
                    LootLockerCharacterLoadoutResponse response = new LootLockerCharacterLoadoutResponse();
                    if (string.IsNullOrEmpty(serverResponse.Error))
                    {
                        response = JsonConvert.DeserializeObject<LootLockerCharacterLoadoutResponse>(serverResponse.text);
                        response.text = serverResponse.text;
                        onComplete?.Invoke(response);
                    }
                    else
                    {
                        response.message = serverResponse.message;
                        response.Error = serverResponse.Error;
                        onComplete?.Invoke(response);
                    }
                }, true);
            }

            public static void ActivatingARentalAsset(LootLockerGetRequest lootLockerGetRequest, Action<LootLockerCharacterLoadoutResponse> onComplete)
            {
                EndPointClass endPoint = LootLockerEndPoints.current.activatingARentalAsset;

                string getVariable = string.Format(endPoint.endPoint, lootLockerGetRequest.getRequests[0]);

                LootLockerServerRequest.CallAPI(getVariable, endPoint.httpMethod, "", (serverResponse) =>
                {
                    LootLockerCharacterLoadoutResponse response = new LootLockerCharacterLoadoutResponse();
                    if (string.IsNullOrEmpty(serverResponse.Error))
                    {
                        response = JsonConvert.DeserializeObject<LootLockerCharacterLoadoutResponse>(serverResponse.text);
                        response.text = serverResponse.text;
                        onComplete?.Invoke(response);
                    }
                    else
                    {
                        response.message = serverResponse.message;
                        response.Error = serverResponse.Error;
                        onComplete?.Invoke(response);
                    }
                }, true);
            }
        }
    
}