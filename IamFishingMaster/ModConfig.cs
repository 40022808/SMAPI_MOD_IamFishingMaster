using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IamFishingMaster
{
    public sealed class ModConfig
    {
        [Range(1, 100)] // 限制 Increase 在 1 到 100 之间
        public int staminaMultiplier { get; set; } = 1;


        [Range(1, 100)] // 限制 AddingStandards 在 5 到 50 之间
        public int fishMultiplier { get; set; } = 1;


        public bool experienceMultiplier { get; set; } = true;

    }
}
