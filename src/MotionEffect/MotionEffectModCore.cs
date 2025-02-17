using System;
using System.Collections;
using Modding;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MotionEffectScript
{
    class MotionEffectModCore : SingleInstance<MotionEffectModCore>
    {
        public override string Name => "MotionEffectMod Core";
        private static AssetBundle assetBundle;//AmplifyMotionのshaderのアセットバンドル
        private GameObject mainCamera;
        private GameObject machine;
        private bool isFirstFrame = true;
        private void LoadAssetBundle()
        {
            if(assetBundle != null)return;
            if(Application.platform == RuntimePlatform.WindowsPlayer){
                assetBundle = ModResource.GetAssetBundle("AmplifyMotionResourcesWin").AssetBundle;//Windows用
            }else{
                assetBundle = ModResource.GetAssetBundle("AmplifyMotionResourcesMac").AssetBundle;//その他UNIX系
            }
        }
        public AssetBundle GetAssetBundle(){
            return assetBundle;//アセットバンドルデータを返す
        }
        private void OnSimulateStart()
        {
            machine = GameObject.Find("Simulation Machine");//シミュ中のマシンを取得
            if(machine != null)
            {
                if(machine.GetComponent<AmplifyMotionObject>() == null)
                {
                    machine.AddComponent<AmplifyMotionObject>();//マシンにモーションブラー適用
                }
            }
            GameObject level = GameObject.Find("MULTIPLAYER LEVEL");//レベルマップ取得
            if(level != null){
                if(level.GetComponent<AmplifyMotionObjectBase>() == null)
                {
                    level.AddComponent<AmplifyMotionObjectBase>();//レベルマップにモーションブラー適用
                }
            }
        }
        void Awake()
        {
            StartCoroutine(CheckVersion());
            LoadAssetBundle();
            SceneManager.activeSceneChanged += OnSceneChaneged;//シーンチェンジイベントに追加
            if(StatMaster.isMP)//マルチプレイヤーシーン対策(シーンチェンジ後にmod起動される為)
            {
                OnSceneChaneged(SceneManager.GetActiveScene(), SceneManager.GetActiveScene());
            }
        }
        void Update()
        {
            if(StatMaster.levelSimulating)
            {
                if(isFirstFrame)
                {
                    isFirstFrame = false;
                    OnSimulateStart();
                }
            }
            else
            {
                isFirstFrame = true;
            }
        }
        private void OnSceneChaneged(Scene arg0, Scene arg1)
        {
            string sceneName = SceneManager.GetActiveScene().name;
            if(!(sceneName is
                "MasterSceneMultiplayer" // mv or level editor
                or "BARREN EXPANSE" or "SANDBOX" or "MISTY MOUNTAIN" or "WATER SANDBOX" or "LEGACY SANDBOX" // sandboxes
                or "5vpjvHZZpwjxpxZp5" //the hidden level
            ) && !int.TryParse(sceneName, out _) //campaign Levels
            ){
                return;
            }
            mainCamera = Camera.main.gameObject;//メインカメラ取得
            if(mainCamera.GetComponent<AmplifyMotionEffectBase>() == null)
            {
                mainCamera.AddComponent<AmplifyMotionEffectBase>();//メインカメラにモーションエフェクトを適用
            }
        }
		private IEnumerator CheckVersion()//コンソールウィンドウへのmodバージョン表示用
        {
            yield return new WaitForSeconds(1f);
            Debug.Log("MotionEffectMod(MotionBlur) Version :" + Mods.GetVersion(new Guid("8bbf03ed-9050-4466-8974-18a6b62a26f4")));
        }
    }
}
