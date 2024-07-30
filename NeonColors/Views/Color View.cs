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
        public static string DirectoryPath = Path.Combine(BepInEx.Paths.PluginPath, "NeonColors", "Colors");
        public static string FilePath = Path.Combine(DirectoryPath, "Colors.json");

        public override void OnShow(object[] args)
        {
            base.OnShow(args);

            Colors.Clear();

            if (!Directory.Exists(DirectoryPath))
            {
                Directory.CreateDirectory(DirectoryPath);
            }

            List<SaveToJson> existingData = ColorSaveCommand.ReadExistingData(FilePath);

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
            str.BeginCenter();
            str.BeginColor("ffffff50").Append("=== ").EndColor();
            str.Append($"Colors (Page {currentPage + 1}/{GetTotalPages()})").BeginColor("ffffff50").Append(" ===").EndColor().AppendLine();
            str.EndAlign();
        }

        private void RedrawColors(StringBuilder str)
        {
            int start = currentPage * ColorsPerPage;
            int end = Mathf.Min(start + ColorsPerPage, colorKeys.Count);

            for (int i = start; i < end; i++)
            {
                str.BeginCenter();
                if (i == currentIndex)
                {
                    str.Append(">");
                    string hexColor = ConvertToHexColor(Colors[colorKeys[i]]);
                    str.BeginColor(hexColor);
                }

                str.AppendLine($"{colorKeys[i]}");

                if (i == currentIndex)
                {
                    str.EndColor();
                }
                str.EndAlign();
            }
        }

        public string ConvertToHexColor(string colors)
        {
            string[] rgb = colors.Split(',');
            if (rgb.Length != 3) return "FFFFFF";

            if (float.TryParse(rgb[0], out float r) && float.TryParse(rgb[1], out float g) && float.TryParse(rgb[2], out float b))
            {
                return ColorUtility.ToHtmlStringRGB(new Color(r, g, b));
            }
            return "FFFFFF";
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

                case EKeyboardKey.Option1:
                    RemoveColor();
                    break;
            }
        }

        void SetColor()
        {
            if (currentIndex >= 0 && currentIndex < colorKeys.Count)
            {
                string selectedKey = colorKeys[currentIndex];
                string selectedColor = Colors[selectedKey];

                if (TryParseColor(selectedColor, out Color color))
                {
                    BaseGameInterface.SetColor(color.r, color.g, color.b);
                    Debug.Log($"Setting color: {selectedKey} with value {selectedColor}");
                }
            }
        }

        bool TryParseColor(string colorString, out Color color)
        {
            color = Color.white;
            string[] rgb = colorString.Split(',');

            if (rgb.Length != 3) return false;
            if (float.TryParse(rgb[0], out float r) && float.TryParse(rgb[1], out float g) && float.TryParse(rgb[2], out float b))
            {
                color = new Color(r, g, b);
                return true;
            }
            return false;
        }

        void MoveDown()
        {
            if (currentIndex < colorKeys.Count - 1)
            {
                currentIndex++;
                if (currentIndex >= (currentPage + 1) * ColorsPerPage)
                {
                    currentPage++;
                    Redraw();
                }
                else
                {
                    Redraw();
                }
            }
        }

        void MoveUp()
        {
            if (currentIndex > 0)
            {
                currentIndex--;
                if (currentIndex < currentPage * ColorsPerPage)
                {
                    currentPage--;
                    currentIndex = Mathf.Min(currentPage * ColorsPerPage + (ColorsPerPage - 1), colorKeys.Count - 1);
                    Redraw();
                }
                else
                {
                    Redraw();
                }
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

        void RemoveColor()
        {
            if (currentIndex >= 0 && currentIndex < colorKeys.Count)
            {
                string keyToRemove = colorKeys[currentIndex];
                Colors.Remove(keyToRemove);
                colorKeys.RemoveAt(currentIndex);

                List<SaveToJson> existingData = ColorSaveCommand.ReadExistingData(FilePath);
                SaveToJson itemToRemove = existingData.Find(item => item.Name.ToUpper() == keyToRemove);
                if (itemToRemove != null)
                {
                    existingData.Remove(itemToRemove);
                    SaveDataToFile(existingData, FilePath);
                }

                if (currentIndex >= colorKeys.Count)
                {
                    currentIndex = colorKeys.Count - 1;
                }

                if (currentPage * ColorsPerPage >= colorKeys.Count && currentPage > 0)
                {
                    currentPage--;
                }

                Redraw();
            }
        }

        void SaveDataToFile(List<SaveToJson> data, string filePath)
        {
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }
    }
}
