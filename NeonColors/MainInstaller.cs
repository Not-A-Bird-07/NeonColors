using ComputerInterface.Interfaces;
using NeonColors.Commands;
using NeonColors.View_Entry;
using Zenject;

namespace NeonColors
{
    internal class MainInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.Bind<IComputerModEntry>().To<Color_Entry>().AsSingle();
            Container.BindInterfacesAndSelfTo<ColorSaveCommand>().AsSingle();
        }
    }
}