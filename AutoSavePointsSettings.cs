using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
Class for the JSON Settings file
 */

namespace AutoSavePoints
{
    public class AutoSavePointsSettings
    {
        public bool showSaveWarning { get; set; } = false;
        public int autoSaveTime { get; set; } = 300;        // Seconds
        public int autoSaveMissions { get; set; } = 2;      // Missions before autosave
    }
}