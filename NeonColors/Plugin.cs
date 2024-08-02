using BepInEx;
using Bepinject;
using HarmonyLib;
using System.Reflection;

namespace NeonColors
{
    [BepInPlugin(PluginInfo.Guid, PluginInfo.Name, PluginInfo.Version)]
    public class NeonColors : BaseUnityPlugin
    {
        void Awake()
        {
            new Harmony(PluginInfo.Guid).PatchAll(Assembly.GetExecutingAssembly());

            Zenjector.Install<MainInstaller>().OnProject().WithConfig(Config).WithLog(Logger);
        }
    }
}