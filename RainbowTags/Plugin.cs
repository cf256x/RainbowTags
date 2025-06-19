using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;
using static PlayerList;

namespace RainbowTags
{
    public class Plugin : Plugin<Config>
    {
        public List<string> PlayersNotRainbowTags { get; set; } = new List<string>();
        public List<string> PlayersRainbowTags { get; set; } = new List<string>();
        public CoroutineHandle _corotine;
        public static Plugin Instance;

        public override void OnEnabled()
        {
            Instance = this;

            Exiled.Events.Handlers.Player.Verified += OnVerified;
            Exiled.Events.Handlers.Player.Destroying += OnDestroying;
            
            base.OnEnabled();
        }
        public override void OnDisabled()
        {
            Instance = null;

            Exiled.Events.Handlers.Player.Verified -= OnVerified;
            Exiled.Events.Handlers.Player.Destroying -= OnDestroying;

            Timing.KillCoroutines(_corotine);
            base.OnDisabled();
        }

        private void OnVerified(VerifiedEventArgs ev)
        {
            if (ev.Player.GroupName == Config.GroupWithRainbowTag) 
            {
                Log.Debug($"{ev.Player.Nickname} has been added to PlayersRainbowTags");
                PlayersRainbowTags.Add(ev.Player.UserId);

                if (_corotine.IsRunning) return;

                _corotine = Timing.RunCoroutine(ChangeColors());
            }
        }
        private void OnDestroying(DestroyingEventArgs ev)
        {
            if (ev.Player.GroupName == Config.GroupWithRainbowTag)
            {
                Log.Debug($"{ev.Player.Nickname} has been deleted from PlayersRainbowTags ?and PlayersNotRainbowTags");

                PlayersRainbowTags.Remove(ev.Player.UserId);
                PlayersNotRainbowTags?.Remove(ev.Player.UserId);

                if (!PlayersRainbowTags.IsEmpty()) return;

                Timing.KillCoroutines(_corotine);
            }
        }

        public IEnumerator<float> ChangeColors()
        {
            int posCurrentColor = 0;
            for (; ; )
            {
                yield return Timing.WaitForSeconds(Config.Delay);

                Log.Debug($"Start: Pos curr color: {posCurrentColor}");

                foreach (string uplayer in PlayersRainbowTags)
                {
                    if (!PlayersNotRainbowTags.Contains(uplayer)) 
                    {
                        Player.Get(uplayer).RankColor = Config.Colors[posCurrentColor];
                        Log.Debug($"{Player.Get(uplayer).Nickname} get color {Config.Colors[posCurrentColor]}");
                    }
                }

                if (posCurrentColor == 7) posCurrentColor = 0;
                else { posCurrentColor++; }

                Log.Debug($"End: Pos curr color: {posCurrentColor}");
            }
        }
    }
}
