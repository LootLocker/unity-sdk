﻿using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker;
using LootLocker.Requests;
using LootLocker.LootLockerEnums;


namespace LootLocker.Requests
{
    public class LootLockerGetMemberRankResponse : LootLockerResponse
    {
        // we are doing thisfor legacy reasons, since it is no longer being set on the backend
        public bool success => status;
        public string member_id { get; set; }
        public int rank { get; set; }
        public int score { get; set; }
        public LootLockerPlayer player { get; set; }
    }

    public class LootLockerPlayer
    {
        public int id { get; set; }
        public string public_uid { get; set; }
        public string name { get; set; }
    }


    public class LootLockerGetByListOfMembersResponse : LootLockerResponse
    {
        public bool success => status;
        public LootLockerMember[] members { get; set; }
    }

    public class LootLockerMember
    {
        public string member_id { get; set; }
        public int rank { get; set; }
        public int score { get; set; }
        public LootLockerPlayer player { get; set; }
    }

    public class LootLockerGetScoreListResponse : LootLockerResponse
    {
        public bool success => status;
        public LootLockerPagination pagination { get; set; }
        public LootLockerMember[] items { get; set; }
    }

    public class LootLockerPagination
    {
        public int total { get; set; }
        public int? next_cursor { get; set; }
        public int? previous_cursor { get; set; }
        public bool allowNext { get; set; }
        public bool allowPrev { get; set; }
    }

    public class LootLockerSubmitScoreResponse : LootLockerResponse
    {
        public bool success => status;
        public string member_id { get; set; }
        public int rank { get; set; }
        public int score { get; set; }
    }


    public class LootLockerSubmitScoreRequest
    {
        public string member_id { get; set; }
        public int score { get; set; }
    }

    public class LootLockerGetMemberRankRequest
    {
        public string leaderboardId { get; set; }
        public int member_id { get; set; }
    }

    public class LootLockerGetScoreListRequest
    {
        public int leaderboardId { get; set; }
        public string count { get; set; }
        public string after { get; set; }

        public static int? nextCursor;
        public static int? prevCursor;
        public static void Reset()
        {
            nextCursor = 0;
            prevCursor = 0;
        }
    }

    public class LootLockerGetByListMembersRequest
    {
        public string[] members { get; set; }
    }
}

namespace LootLocker
{

    public partial class LootLockerAPIManager
    {
        public static void GetMemberRank(LootLockerGetMemberRankRequest data, Action<LootLockerGetMemberRankResponse> onComplete)
        {
            EndPointClass endPoint = LootLockerEndPoints.getMemberRank;
            string tempEndpoint = string.Format(endPoint.endPoint, data.leaderboardId, data.member_id);
            LootLockerServerRequest.CallAPI(tempEndpoint, endPoint.httpMethod, null, (serverResponse) =>
            {
                LootLockerGetMemberRankResponse response = new LootLockerGetMemberRankResponse();
                if (string.IsNullOrEmpty(serverResponse.Error))
                    response = JsonConvert.DeserializeObject<LootLockerGetMemberRankResponse>(serverResponse.text);

                //   LootLockerSDKManager.DebugMessage(serverResponse.text, !string.IsNullOrEmpty(serverResponse.Error));
                response.text = serverResponse.text;
                response.status = serverResponse.status;
                response.Error = serverResponse.Error; response.statusCode = serverResponse.statusCode;
                onComplete?.Invoke(response);
            }, true, LootLocker.LootLockerEnums.LootLockerCallerRole.User);
        }

        public static void GetByListOfMembers(LootLockerGetByListMembersRequest data, string id, Action<LootLockerGetByListOfMembersResponse> onComplete)
        {
            EndPointClass requestEndPoint = LootLockerEndPoints.getByListOfMembers;
            string json = "";
            if (data == null) return;
            else json = JsonConvert.SerializeObject(data);

            string endPoint = string.Format(requestEndPoint.endPoint, id);

            LootLockerServerRequest.CallAPI(endPoint, requestEndPoint.httpMethod, json, (serverResponse) =>
            {
                LootLockerGetByListOfMembersResponse response = new LootLockerGetByListOfMembersResponse();
                if (string.IsNullOrEmpty(serverResponse.Error))
                    response = JsonConvert.DeserializeObject<LootLockerGetByListOfMembersResponse>(serverResponse.text);

                //    LootLockerSDKManager.DebugMessage(serverResponse.text, !string.IsNullOrEmpty(serverResponse.Error));
                response.text = serverResponse.text;
                response.status = serverResponse.status;
                response.Error = serverResponse.Error; response.statusCode = serverResponse.statusCode;

                onComplete?.Invoke(response);
            }, true, LootLocker.LootLockerEnums.LootLockerCallerRole.User);
        }

        public static void GetScoreList(LootLockerGetScoreListRequest getRequests, Action<LootLockerGetScoreListResponse> onComplete)
        {
            EndPointClass requestEndPoint = LootLockerEndPoints.getScoreList;

            string tempEndpoint = requestEndPoint.endPoint;
            string endPoint = string.Format(requestEndPoint.endPoint, getRequests.leaderboardId, int.Parse(getRequests.count));

            if (!string.IsNullOrEmpty(getRequests.after))
            {
                tempEndpoint = requestEndPoint.endPoint + "&after={2}";
                endPoint = string.Format(tempEndpoint, getRequests.leaderboardId, int.Parse(getRequests.count), int.Parse(getRequests.after));
            }

            LootLockerServerRequest.CallAPI(endPoint, requestEndPoint.httpMethod, null, (serverResponse) =>
            {
                LootLockerGetScoreListResponse response = new LootLockerGetScoreListResponse();
                if (string.IsNullOrEmpty(serverResponse.Error))
                    response = JsonConvert.DeserializeObject<LootLockerGetScoreListResponse>(serverResponse.text);

                //   LootLockerSDKManager.DebugMessage(serverResponse.text, !string.IsNullOrEmpty(serverResponse.Error));
                response.text = serverResponse.text;
                response.status = serverResponse.status;
                response.Error = serverResponse.Error; response.statusCode = serverResponse.statusCode;
                onComplete?.Invoke(response);
            }, true, LootLocker.LootLockerEnums.LootLockerCallerRole.User);
        }

        public static void SubmitScore(LootLockerSubmitScoreRequest data, string id, Action<LootLockerSubmitScoreResponse> onComplete)
        {
            EndPointClass requestEndPoint = LootLockerEndPoints.submitScore;
            string json = "";
            if (data == null) return;
            else json = JsonConvert.SerializeObject(data);

            string endPoint = string.Format(requestEndPoint.endPoint, id);

            LootLockerServerRequest.CallAPI(endPoint, requestEndPoint.httpMethod, json, onComplete: (serverResponse) =>
            {
                LootLockerSubmitScoreResponse response = new LootLockerSubmitScoreResponse();
                if (string.IsNullOrEmpty(serverResponse.Error))
                    response = JsonConvert.DeserializeObject<LootLockerSubmitScoreResponse>(serverResponse.text);

                // LootLockerSDKManager.DebugMessage(serverResponse.text, !string.IsNullOrEmpty(serverResponse.Error));
                response.text = serverResponse.text;
                response.status = serverResponse.status;
                response.Error = serverResponse.Error; response.statusCode = serverResponse.statusCode;
                onComplete?.Invoke(response);
            }, useAuthToken: true, callerRole: LootLocker.LootLockerEnums.LootLockerCallerRole.User);
        }

    }
}
