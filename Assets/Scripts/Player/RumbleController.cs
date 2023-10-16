using OperationPlayground.Managers;
using RicTools;
using RicTools.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OperationPlayground.Player
{
    public class RumbleController
    {
        private PlayerManager player;

        private GameTimer rumbleTimer;

        private float lowFrequency, highFrequency;

        public RumbleController(PlayerManager player)
        {
            this.player = player;
        }


        // https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/api/UnityEngine.InputSystem.Gamepad.html
        public void DoRumble(float lowFrequency, float highFrequency, float duration)
        {
            this.lowFrequency = lowFrequency;
            this.highFrequency = highFrequency;
            player.gamepad.SetMotorSpeeds(lowFrequency, highFrequency);


            if(rumbleTimer != null)
            {
                rumbleTimer.Remove();
                rumbleTimer = null;
            }

            if (duration >= 0)
            {
                rumbleTimer = TimerManager.Instance.CreateTimer(duration, false);
                rumbleTimer.Start();
                rumbleTimer.onComplete += () =>
                {
                    StopRumble();
                };
            }
        }

        public void StopRumble()
        {
            if (rumbleTimer != null)
                rumbleTimer.Remove();
            player.gamepad.SetMotorSpeeds(0, 0);
            rumbleTimer = null;
        }

        public void PauseRumble()
        {
            if(rumbleTimer != null)
            {
                rumbleTimer.Pause();
                player.gamepad.SetMotorSpeeds(0, 0);
            }
        }

        public void ResumeRumble()
        {
            if(rumbleTimer != null)
            {
                rumbleTimer.Unpause();
                player.gamepad.SetMotorSpeeds(lowFrequency, highFrequency);
            }
        }
    }
}
