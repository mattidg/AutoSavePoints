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
        public bool showSaveWarning = false;
        public int autoSaveTime = 300;        // Seconds
        public int autoSaveMissions = 2;      // Missions before autosave
    }
}