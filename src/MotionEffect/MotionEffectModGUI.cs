using System;
using System.Collections;
using Modding;
using UnityEngine;
using MotionEffectScript;
using UnityEngine.UI;

namespace MotionEffectScript
{
    class MotionEffectModGUI:SingleInstance<MotionEffectModGUI>
    {
        public override string Name => "MotionEffectMod GUI";
        private bool tabHide = false;//tabが押されたかどうか(trueでGUIを隠す)
        private bool keyHide = true;//Alt+Mが押されたかどうか(trueでGUIを隠す)
        private int windowId;
        private Rect windowRect = new Rect(20, 100, 250, 310);
        private float sliderMotionScale = 3.0f;
        private float sliderCameraMotionMult = 0.5f;
        private float sliderObjectMotionMult = 1.0f;
        private float sliderMinVelocity = 1.0f;
        private float sliderMaxVelocity = 10.0f;
        private bool toggleNoise = false;
        private AmplifyMotionEffectBase amplifyMotionEffectBase;
        private void ApplyBtnClicked()
        {
            if(amplifyMotionEffectBase == null)
            {
                amplifyMotionEffectBase = Camera.main.gameObject.GetComponent<AmplifyMotionEffectBase>();
            }
            amplifyMotionEffectBase.MotionScale = sliderMotionScale;
            amplifyMotionEffectBase.CameraMotionMult = sliderCameraMotionMult;
            amplifyMotionEffectBase.ObjectMotionMult = sliderObjectMotionMult;
            amplifyMotionEffectBase.MinVelocity = sliderMinVelocity;
            amplifyMotionEffectBase.MaxVelocity = sliderMaxVelocity;
            amplifyMotionEffectBase.Noise = toggleNoise;
        }
        private void ResetBtnClicked()
        {
            sliderMotionScale = 3.0f;
            sliderCameraMotionMult = 0.5f;
            sliderObjectMotionMult = 1.0f;
            sliderMinVelocity = 1.0f;
            sliderMaxVelocity = 10.0f;
            toggleNoise = false;

            ApplyBtnClicked();
        }
        void Awake(){
            windowId = ModUtility.GetWindowId();
        }
        void Update(){
            if(Input.GetKeyDown(KeyCode.Tab))
            {
                tabHide = !tabHide;
            }
            if(Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.M))
            {
                keyHide = !keyHide;
            }
        }
        void OnGUI()
        {
            if(!StatMaster.isMainMenu && !tabHide && !keyHide && !StatMaster.inMenu)
            {
                windowRect = GUI.Window(windowId, windowRect, delegate(int windowId){
                    GUI.Label(new Rect(25, 25, 200, 20), "MotionScale:" + sliderMotionScale);
                    sliderMotionScale = GUI.HorizontalSlider(new Rect(25, 45, 200, 30), sliderMotionScale, 0f, 10f);
                    GUI.Label(new Rect(25, 65, 200, 20), "CameraMotionMult:" + sliderCameraMotionMult);
                    sliderCameraMotionMult = GUI.HorizontalSlider(new Rect(25, 85, 200, 30), sliderCameraMotionMult, 0f, 10f);
                    GUI.Label(new Rect(25, 105, 200, 20), "ObjectMotionMult:" + sliderObjectMotionMult);
                    sliderObjectMotionMult = GUI.HorizontalSlider(new Rect(25, 125, 200, 30), sliderObjectMotionMult, 0f, 10f);
                    GUI.Label(new Rect(25, 145, 200, 20), "MinVelocity:" + sliderMinVelocity);
                    sliderMinVelocity = GUI.HorizontalSlider(new Rect(25, 165, 200, 30), sliderMinVelocity, 0f, 100f);
                    GUI.Label(new Rect(25, 185, 200, 20), "MaxVelocity:" + sliderMaxVelocity);
                    sliderMaxVelocity = GUI.HorizontalSlider(new Rect(25, 205, 200, 30), sliderMaxVelocity, 0f, 100f);
                    toggleNoise = GUI.Toggle(new Rect(25, 225, 200, 30), toggleNoise, "Noise");
                    if(GUI.Button(new Rect(25, 255, 200, 20), "Apply"))
                    {
                        ApplyBtnClicked();
                    }
                    if(GUI.Button(new Rect(25, 280, 200, 20), "Reset"))
                    {
                        ResetBtnClicked();
                    }
                    GUI.DragWindow();
                }, "MotionBlur Setting[Alt+M]");
            }
        }
        public int GetWindowId()
        {
            return windowId;
        }
    }
}
