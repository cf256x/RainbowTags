using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandSystem;
using Exiled.API.Features;

namespace RainbowTags
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class ChangeRainbowTagColors : ICommand
    {
        public string Command { get; set; } = "changerainbowtagcolor";

        public string[] Aliases { get; set; } = { "crtc" };

        public string Description { get; set; } = "Поменять цвета переливания тега";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);
            if (Plugin.Instance.Config.GroupWithRainbowTag != player.GroupName)
            {
                response = "Ваша группа не является разрешенной";
                return false;
            }

            if (!Plugin.Instance.CustomColors.ContainsKey(player.UserId))
            {
                Plugin.Instance.CustomColors.Add(player.UserId, Plugin.Instance.Config.Colors);
            }

            if (arguments.Count < 1)
            {
                response = "Один из аргументов введен неверно:\n.crtc replace color_number color\n.crtc default color_number";
                return false;
            }

            if (arguments.At(0) == "default")
            {
                if (Int32.Parse(arguments.At(1)) > 8 || Int32.Parse(arguments.At(1)) < 1)
                {
                    response = "Номер цвета не может быть больше 8 или меньше 1";
                    return false;
                }

                Plugin.Instance.CustomColors[player.UserId][Int32.Parse(arguments.At(1)) - 1] = Plugin.Instance.Config.Colors[Int32.Parse(arguments.At(1)) - 1];

                response = $"Вы успешно установили обычный цвет";
                return true;
            }
            else if (arguments.At(0) == "replace")
            {
                if (Int32.Parse(arguments.At(1)) > 8 || Int32.Parse(arguments.At(1)) < 1)
                {
                    response = "Номер цвета не может быть больше 8 или меньше 1";
                    return false;
                }

                Plugin.Instance.CustomColors[player.UserId][Int32.Parse(arguments.At(1)) - 1] = arguments.At(2);

                response = $"Вы успешно свой цвет";
                return true;
            }

            response = "Один из аргументов введен неверно:\n.crtc replace color_number color\n.crtc default color_number";
            return false;
        }
    }
}
