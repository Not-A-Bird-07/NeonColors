using System.Collections.Generic;
using BepInEx;
using Bepinject;
using HarmonyLib;
using System.Reflection;
using NeonColors.Patchs;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace NeonColors
{
    [BepInDependency("tonimacaroni.computerinterface")]
    [BepInPlugin(PluginInfo.Guid, PluginInfo.Name, PluginInfo.Version)]
    public class NeonColors : BaseUnityPlugin
    {
        void Awake()
        {
            new Harmony(PluginInfo.Guid).PatchAll(Assembly.GetExecutingAssembly());
            //VRRigPatch.Hook();

            Zenjector.Install<MainInstaller>().OnProject().WithConfig(Config).WithLog(Logger);
        }
    }
}