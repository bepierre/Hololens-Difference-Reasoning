
using HUX.Focus;
using HUX.Interaction;
using HUX.Receivers;
using UnityEngine;

namespace HoloToolkit.Unity
{
    public class MenuReceiver : InteractionReceiver
    {
        private GameObject MiniMap_;
        private GameObject Map_;
        private Camera MainCamera_;
        private RenderDepthDifference RDDscript_;
        private GameObject SpatialMapping_;
        private GameObject ToolBar_;
        private Tagalong TAscript_;
        private Billboard Bscript_;
        private GameObject LockButton_;
        private GameObject UnlockButton_;

        void Start()
        {
            //Get everything that we need
            MiniMap_ = GameObject.Find("MiniMap");
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
                    SpatialMapping_.SetActive(true);
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

                case "LoadButton":
                    
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

    }
}
