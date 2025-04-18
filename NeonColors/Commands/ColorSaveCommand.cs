﻿using ComputerInterface;
using System.Collections.Generic;
using Zenject;
using Newtonsoft.Json;
using System.IO;

namespace NeonColors.Commands
{
    internal class ColorSaveCommand : IInitializable
    {
        private readonly CommandHandler _commandHandler;
        private List<CommandToken> _commandTokens;

        public ColorSaveCommand(CommandHandler commandHandler)
        {
            _commandHandler = commandHandler;
        }

        public void Initialize()
        {
            _commandTokens = new List<CommandToken>();

            RegisterCommand(new Command(name: "savecolor", argumentTypes: new[] { typeof(string) }, args =>
            {
                string filePath = Path.Combine(BepInEx.Paths.PluginPath, "NeonColors", "Colors", "Colors.json");

                List<SaveToJson> existingData = ReadExistingData(filePath);

                string name = "";
                
                for (int i = 0; i < args.Length; i++)
                {
                    name += args[i] + " ";
                }

                existingData.Add(new SaveToJson()
                {
                    Name = name,
                    R = GorillaTagger.Instance.offlineVRRig.playerColor.r,
                    G = GorillaTagger.Instance.offlineVRRig.playerColor.g,
                    B = GorillaTagger.Instance.offlineVRRig.playerColor.b,
                });

                string json = JsonConvert.SerializeObject(existingData.ToArray(), Formatting.Indented);
                if(!File.Exists(filePath))
                    File.Create(filePath);

                File.WriteAllText(filePath, json);
                return "Saved Color";
            }));
        }

        public void RegisterCommand(Command cmd)
        {
            var token = _commandHandler.AddCommand(cmd);
            _commandTokens.Add(token);
        }

        public static List<SaveToJson> ReadExistingData(string filePath)
        {
            if (File.Exists(filePath))
            {
                string existingJson = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<List<SaveToJson>>(existingJson) ?? new List<SaveToJson>();
            }
            return new List<SaveToJson>();
        }
    }

    public class SaveToJson
    {
        public string Name;
        public float R;
        public float G;
        public float B;
    }
}