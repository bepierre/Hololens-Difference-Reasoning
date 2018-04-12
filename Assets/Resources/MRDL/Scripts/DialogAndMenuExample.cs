//
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
//
using HUX.Dialogs;
using HUX.Interaction;
using HUX.Receivers;
using System.Collections;
using UnityEngine;

namespace HUX
{
    public class DialogAndMenuExample : InteractionReceiver
    {
        public GameObject DialogPrefab;
        public GameObject[] LaunchDialogButtons;

        [Header ("Dialog 1 options")]
        public string Dialog1Title = "Close Dialog";
        [TextArea]
        public string Dialog1Message = "This is a message for dialog 1.";
        SimpleDialog.ButtonTypeEnum Dialog1Button = SimpleDialog.ButtonTypeEnum.Close;


        protected bool launchedDialog;

        protected override void OnTapped(GameObject obj, InteractionManager.InteractionEventArgs eventArgs)
        {
            base.OnTapped(obj, eventArgs);
            if (launchedDialog)
                return;

            SimpleDialog.ButtonTypeEnum buttons = SimpleDialog.ButtonTypeEnum.Close;
            string title = string.Empty;
            string message = string.Empty;

            switch (obj.name)
            {
                case "Dialog1":
                default:
                    title = Dialog1Title;
                    message = Dialog1Message;
                    buttons = Dialog1Button;
                    break;
            }

            launchedDialog = true;
            StartCoroutine(LaunchDialogOverTime(buttons, title, message));
        }

        protected IEnumerator LaunchDialogOverTime(SimpleDialog.ButtonTypeEnum buttons, string title, string message)
        {
            // Disable all our buttons
            foreach (GameObject buttonGo in Interactibles)
            {
                buttonGo.SetActive(false);
            }

            SimpleDialog dialog = SimpleDialog.Open(DialogPrefab, buttons, title, message);

            // Wait for dialog to close
            while (dialog.State != SimpleDialog.StateEnum.Closed)
            {
                yield return null;
            }

            // Enable all our buttons
            foreach (GameObject buttonGo in Interactibles)
            {
                buttonGo.SetActive(true);
            }
            launchedDialog = false;
            yield break;
        }


    }
}
