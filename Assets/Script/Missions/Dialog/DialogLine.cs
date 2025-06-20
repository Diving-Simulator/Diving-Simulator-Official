using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script.Missions.Dialog
{
    [System.Serializable]
    public class DialogLine
    {
        [TextArea]
        public string text;
        public float timePerCharacter = 0.1f;
    }
}
