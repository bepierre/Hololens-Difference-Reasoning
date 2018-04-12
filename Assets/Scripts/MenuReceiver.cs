
using HUX.Focus;
using HUX.Interaction;
using HUX.Receivers;
using UnityEngine;
using System.Collections;
using HUX.Dialogs;


namespace HoloToolkit.Unity
{
    public class MenuReceiver : InteractionReceiver
    {
        private GameObject MiniMap_;
        //private BoxCollider BoxCol_;
        private GameObject Map_;
        private Camera MainCamera_;
        private RenderDepthDifference RDDscript_;
        private GameObject SpatialMapping_;
        private GameObject ToolBar_;
        private Tagalong TAscript_;
        private Billboard Bscript_;
        private GameObject LockButton_;
        private GameObject UnlockButton_;
        private GameObject BBshell_;

        //About dialog
        public GameObject DialogPrefab;
        public GameObject[] LaunchDialogButtons;
        [Header("About Button options")]
        public string Dialog1Title = "About.";
        [TextArea]
        public string Dialog1Message = "Description.";
        SimpleDialog.ButtonTypeEnum Dialog1Button = SimpleDialog.ButtonTypeEnum.Close;

        void Start()
        {
            //Get everything that we need
            MiniMap_ = GameObject.Find("MiniMap");
            //BoxCol_ = MiniMap_.GetComponent<BoxCollider>();
            Map_ = GameObject.Find("Map");
            MainCamera_ = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
            RDDscript_ = MainCamera_.GetComponent<RenderDepthDifference>();
            SpatialMapping_ = GameObject.Find("SpatialMapping");
            ToolBar_ = GameObject.Find("ToolBar");
            TAscript_ = ToolBar_.GetComponent<Tagalong>();
            Bscript_ = ToolBar_.GetComponent<Billboard>();
            LockButton_ = GameObject.Find("LockButton");
            UnlockButton_ = GameObject.Find("UnlockButton");

            // Everything off at start
            RDDscript_.enabled = false;
            Map_.layer = LayerMask.NameToLayer("Hidden");
            MiniMap_.layer = LayerMask.NameToLayer("Hidden");
            SpatialMapping_.SetActive(false);
            UnlockButton_.SetActive(false);
        }

        protected override void OnTapped(GameObject obj, InteractionManager.InteractionEventArgs eventArgs)
        {

            switch (obj.name)
            {
                // Manual Alignement
                case "MaButton":
                    RDDscript_.enabled = false;
                    Map_.layer = LayerMask.NameToLayer("Default");
                    MiniMap_.layer = LayerMask.NameToLayer("UI");
                    SpatialMapping_.SetActive(false);
                    break;

                // Difference Reasoning
                case "DrButton":
                    RDDscript_.enabled = true;
                    Map_.layer = LayerMask.NameToLayer("ReferenceLayer");
                    MiniMap_.layer = LayerMask.NameToLayer("Hidden");
                    //BoxCol_.enabled = false;
                    SpatialMapping_.SetActive(true);

                    //remove the bounding box shell that is created when tapping on minimap during manual alignement
                    //clicking on the minimap in manual align will bring it back
                    BBshell_ = GameObject.Find("BoundingBoxShell(Clone)");
                    Destroy(BBshell_);
                    break;

                // Lock Button
                case "LockButton":
                    TAscript_.enabled = false;
                    Bscript_.enabled = false;
                    LockButton_.SetActive(false);
                    UnlockButton_.SetActive(true);
                    break;

                // Unlock Button
                case "UnlockButton":
                    TAscript_.enabled = true;
                    Bscript_.enabled = true;
                    UnlockButton_.SetActive(false);
                    LockButton_.SetActive(true);
                    break;

                // Load Button
                // loads minimap and map mesh
                case "LoadButton":
                    break;

                // About Button
                case "AboutButton":
                    SimpleDialog.ButtonTypeEnum buttons = SimpleDialog.ButtonTypeEnum.Close;
                    string title = string.Empty;
                    string message = string.Empty;
                    title = Dialog1Title;
                    message = Dialog1Message;
                    buttons = Dialog1Button;
                    StartCoroutine(LaunchDialogOverTime(buttons, title, message));
                    break;


                /*
                case "RmButton":
                    // Remove Button Hides menu
                    foreach (Transfor
                    m trans in ToolBar_.GetComponentsInChildren<Transform>(true))
                    {
                        trans.gameObject.layer = LayerMask.NameToLayer("Hidden");
                    }
                    break;
                */
            }
            base.OnTapped(obj, eventArgs);
        }

        protected override void OnHoldStarted(GameObject obj, InteractionManager.InteractionEventArgs eventArgs)
        {
            base.OnHoldStarted(obj, eventArgs);
        }

        protected override void OnFocusEnter(GameObject obj, FocusArgs args)
        {
            base.OnFocusEnter(obj, args);
        }

        protected override void OnFocusExit(GameObject obj, FocusArgs args)
        {
            base.OnFocusExit(obj, args);
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
            yield break;
        }
    }
}
