﻿using LootLocker.Requests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LootLockerDemoApp
{
    public class KeyValueElements : MonoBehaviour
    {
        public InputField key;
        public InputField value;
        public Button editBtn;

        void Awake()
        {
            editBtn?.onClick.AddListener(Edit);
        }

        public void Init(LootLockerPayload payload)
        {
            this.key.text = payload.key;
            this.value.text = payload.value;
        }

        public void Edit()
        {
            GetComponentInParent<StorageScreen>()?.OpenKeyWindow(this.key.text, this.value.text, new string[] { "Edit", "Delete" });
        }
    }
}
