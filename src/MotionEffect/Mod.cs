using System;
using Modding;
using UnityEngine;
using MotionEffectScript;
using System.Collections;

namespace MotionEffectScript
{
	public class Mod : ModEntryPoint
	{
		private MotionEffectModCore mod;
		private MotionEffectModGUI gui;
		public override void OnLoad()
		{
			// Called when the mod is loaded.
			if(Application.platform != RuntimePlatform.WindowsPlayer){
				ModConsole.Log("MotionEffectMod(MotionBlur) is not supported except win10/11.");
			}
			UnityEngine.Object.DontDestroyOnLoad(mod = SingleInstance<MotionEffectModCore>.Instance);
			UnityEngine.Object.DontDestroyOnLoad(gui = SingleInstance<MotionEffectModGUI>.Instance);
		}
	}
}
