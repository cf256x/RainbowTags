using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandSystem;
using Exiled.API.Features;
using MEC;
using UnityEngine;

namespace RainbowTags
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class ToggleRainbowColor : ICommand
    {
        public string Command { get; set; } = "togglerainbowtag";

        public string[] Aliases { get; set; } = { "trt" };

        public string Description { get; set; } = "Переключает переливание тега различными цветами";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);
            if (Plugin.Instance.Config.GroupWithRainbowTag != player.GroupName)
            {
                response = "Ваша группа не является разрешенной";
                return false;
            }

            if (Plugin.Instance.PlayersNotRainbowTags.Contains(player.UserId))
            {
                Plugin.Instance.PlayersNotRainbowTags.Remove(player.UserId);

                if (!Plugin.Instance._corotine.IsRunning) Plugin.Instance._corotine = Timing.RunCoroutine(Plugin.Instance.ChangeColors());

                response = "Вы включили переливающийся тег";
                return true;
            }

            Plugin.Instance.PlayersNotRainbowTags.Add(player.UserId);
            player.RankColor = player.Group.BadgeColor;

            if (Plugin.Instance.PlayersNotRainbowTags.Count() == Plugin.Instance.PlayersRainbowTags.Count()) Timing.KillCoroutines(Plugin.Instance._corotine);

            response = "Вы выключили переливающийся тег";
            return true;
        }
    }
}
