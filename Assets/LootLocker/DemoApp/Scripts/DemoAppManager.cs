﻿using LootLockerAdmin;
using LootLockerAdminRequests;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LootLockerRequests
{
    public partial class LootLockerSDKManager
    {
        #region Authentication
        /// <summary>
        /// This is an admin call, please do not use this as a normal call. This is not intended to be used to connect to players. Lootlocker does not currently support
        /// username and password login for normal users
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="onComplete"></param>

        public static void InitialAuthRequest(string email, string password, Action<AuthResponse> onComplete)
        {
            if (!CheckInitialized()) return;
            var data = new InitialAuthRequest();
            data.email = email;
            data.password = password;
            DemoAppAdminRequests.InitialAuthenticationRequest(data, onComplete);
        }
        /// <summary>
        /// This is also an admin call, please do not use this for the normal game
        /// </summary>
        /// <param name="mfa_key"></param>
        /// <param name="secret"></param>
        /// <param name="onComplete"></param>
        public static void TwoFactorAuthVerification(string mfa_key, string secret, Action<AuthResponse> onComplete)
        {

            if (!CheckInitialized()) return;
            var data = new TwoFactorAuthVerficationRequest();
            data.mfa_key = mfa_key;
            data.secret = secret;
            DemoAppAdminRequests.TwoFactorAuthVerification(data, onComplete);

        }

        public static void SubsequentRequestsRequest(Action<SubsequentRequestsResponse> onComplete)
        {

            if (!CheckInitialized()) return;
            DemoAppAdminRequests.SubsequentRequests(onComplete);

        }

        #endregion
        /// <summary>
        /// This is an admin call, for creating games
        /// </summary>
        /// <param name="name"></param>
        /// <param name="steam_app_id"></param>
        /// <param name="sandbox_mode"></param>
        /// <param name="organisation_id"></param>
        /// <param name="demo"></param>
        /// <param name="onComplete"></param>
        public static void CreatingAGame(string name, string steam_app_id, bool sandbox_mode, int organisation_id, bool demo, Action<CreatingAGameResponse> onComplete)
        {

            if (!CheckInitialized()) return;

            CreatingAGameRequest data = new CreatingAGameRequest
            {

                name = name,
                steam_app_id = steam_app_id,
                sandbox_mode = sandbox_mode,
                organisation_id = organisation_id,
                demo = demo

            };

            DemoAppAdminRequests.CreatingAGame(data, onComplete);

        }

        public static void GetDetailedInformationAboutAGame(string id, Action<CreatingAGameResponse> onComplete)
        {
            if (!CheckInitialized()) return;
            LootLockerGetRequest lootLockerGetRequest = new LootLockerGetRequest();
            lootLockerGetRequest.getRequests.Add(id.ToString());
            DemoAppAdminRequests.GetDetailedInformationAboutAGame(lootLockerGetRequest, onComplete);
        }

        public static void ListTriggers(int game_id, Action<ListTriggersResponse> onComplete)
        {
            if (!CheckInitialized()) return;
            LootLockerGetRequest data = new LootLockerGetRequest();
            data.getRequests.Add(game_id.ToString());
            DemoAppAdminRequests.ListTriggers(data, onComplete);
        }

        public static void CreateTriggers(CreateTriggersRequest requestData, int game_id, Action<ListTriggersResponse> onComplete)
        {
            if (!CheckInitialized()) return;
            LootLockerGetRequest data = new LootLockerGetRequest();
            data.getRequests.Add(game_id.ToString());
            DemoAppAdminRequests.CreateTriggers(requestData, data, onComplete);
        }
    }

}