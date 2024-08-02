using ComputerInterface.Interfaces;
using NeonColors.Views;
using System;

namespace NeonColors.View_Entry
{
    internal class Color_Entry : IComputerModEntry
    {
        public string EntryName => "<color=#FF0000>C</color><color=#FF7F00>o</color><color=#FFFF00>l</color><color=#7FFF00>o</color><color=#00FF00>r</color> <color=#00FEFF>P</color><color=#007FFF>i</color><color=#0000FF>c</color><color=#7F00FF>k</color><color=#FF00FE>e</color><color=#FF007F>r</color>";
        public Type EntryViewType => typeof(Color_View);
    }
}