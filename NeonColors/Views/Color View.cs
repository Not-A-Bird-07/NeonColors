using ComputerInterface;
using ComputerInterface.Extensions;
using ComputerInterface.ViewLib;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using Newtonsoft.Json;
using NeonColors.Commands;

namespace NeonColors.Views
{
    internal class Color_View : ComputerView
    {
        public Dictionary<string, string> Colors = new Dictionary<string, string>();

        private const int ColorsPerPage = 12;
        private int currentIndex = 0;
        private int currentPage = 0;
        private List<string> colorKeys;

        public override void OnShow(object[] args)
        {
            base.OnShow(args);

            Colors.Clear();

            string directoryPath = Path.Combine(BepInEx.Paths.PluginPath, "NeonColors", "Colors");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            string filePath = Path.Combine(directoryPath, "Colors.json");

            // Read existing data from JSON file
            List<SaveToJson> existingData = ColorSaveCommand.ReadExistingData(filePath);

            // Populate the dictionary with the data from JSON file
            PopulateColorsDictionary(existingData);

            colorKeys = new List<string>(Colors.Keys);

            Redraw();
        }

        void Redraw()
        {
            StringBuilder builder = new StringBuilder();

            RedrawHeader(builder);
            RedrawColors(builder);

            Text = builder.ToString();
        }

        public void PopulateColorsDictionary(List<SaveToJson> colorDataList)
        {
            foreach (var colorData in colorDataList)
            {
                string colorValue = $"{colorData.R}, {colorData.G}, {colorData.B}";
                Colors[colorData.Name.ToUpper()] = colorValue;
            }
        }

        private void RedrawHeader(StringBuilder str)
        {
            str.BeginColor("ffffff50").Append("=== ").EndColor();
            str.Append($"Colors (Page {currentPage + 1}/{GetTotalPages()})").BeginColor("ffffff50").Append(" ===").EndColor().AppendLine();
        }

        private void RedrawColors(StringBuilder str)
        {
            int start = currentPage * ColorsPerPage;
            int end = Mathf.Min(start + ColorsPerPage, colorKeys.Count);

            for (int i = start; i < end; i++)
            {
                if (i == currentIndex)
                {
                    str.Append("> "); // Add indicator for the selected color
                    str.BeginColor("ffff00"); // Highlight color
                }
                else
                {
                    str.Append("  "); // Add spacing for unselected colors
                }

                str.AppendLine($"{colorKeys[i]}");

                if (i == currentIndex)
                {
                    str.EndColor();
                }
            }
        }

        private int GetTotalPages()
        {
            return Mathf.CeilToInt((float)colorKeys.Count / ColorsPerPage);
        }

        public override void OnKeyPressed(EKeyboardKey key)
        {
            switch (key)
            {
                case EKeyboardKey.Back:
                    ReturnToMainMenu();
                    break;

                case EKeyboardKey.Enter:
                    SetColor();
                    break;

                case EKeyboardKey.Down:
                    MoveDown();
                    break;

                case EKeyboardKey.Up:
                    MoveUp();
                    break;

                case EKeyboardKey.Left:
                    PreviousPage();
                    break;

                case EKeyboardKey.Right:
                    NextPage();
                    break;
            }
        }

        void SetColor()
        {
            if (currentIndex >= 0 && currentIndex < colorKeys.Count)
            {
                string selectedKey = colorKeys[currentIndex];
                string selectedColor = Colors[selectedKey];

                string[] rgb = selectedColor.Split(',');

                float r = float.Parse(rgb[0]);
                float g = float.Parse(rgb[1]);
                float b = float.Parse(rgb[2]);

                BaseGameInterface.SetColor(r, g, b);

                Debug.Log($"Setting color: {selectedKey} with value {selectedColor}");
            }
        }

        void MoveDown()
        {
            if (currentIndex < colorKeys.Count - 1)
            {
                currentIndex++;
                Redraw();
            }
        }

        void MoveUp()
        {
            if (currentIndex > 0)
            {
                currentIndex--;
                Redraw();
            }
        }

        void NextPage()
        {
            if (currentPage < GetTotalPages() - 1)
            {
                currentPage++;
                currentIndex = currentPage * ColorsPerPage;
                Redraw();
            }
        }

        void PreviousPage()
        {
            if (currentPage > 0)
            {
                currentPage--;
                currentIndex = currentPage * ColorsPerPage;
                Redraw();
            }
        }
    }
}
