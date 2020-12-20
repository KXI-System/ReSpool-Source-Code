using System;
using System.Collections;
using System.Collections.Generic;

namespace KXI 
{
    public class DialogueParser
    {
        public static ParserOptions DefaultOptions = new ParserOptions 
        {
            Mod_DirectionName = "CMD"
        };

        public static void ParseMods(ref DialogueInfo inputInfo)
        {
            ParseMods(ref inputInfo, DefaultOptions);
        }

        public static void ParseMods(ref DialogueInfo inputInfo, ParserOptions options) 
        {
            if (inputInfo.RawMods.Length == 0 || inputInfo.RawMods == null) return;

            if (Array.Exists(inputInfo.RawMods, element => element.ToUpper() == options.Mod_DirectionName.ToUpper()))
            {
                inputInfo.isDirection = true;
            }
            else
            {
                inputInfo.isDirection = false;
            }
        }
    }

    public struct DialogueInfo 
    {
        public string Speaker;
        public string Dialogue;
        public string[] RawMods;
        public string RawText;

        // Direction or Command
        public bool isDirection;
    }

    public struct OptionInfo 
    {
        public int id;
        public string text;
        public string[] RawMods;
    }

    public struct ParserOptions 
    {
        public string Mod_DirectionName;
    }
}
