﻿using Sprint.Characters;
using Sprint.Interfaces;

namespace Sprint.Functions
{
    internal class StopMoving : ICommand
    {
        private Player player;

        public StopMoving(Player player)
        {
            this.player = player;
        }

        public void Execute()
        {
            this.player.StopMoving();
        }
    }

}
