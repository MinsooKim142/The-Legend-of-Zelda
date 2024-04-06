﻿using Sprint.GameStates;
using Sprint.Interfaces;

namespace Sprint.Functions
{
    internal class CloseInventoryCommand : ICommand
    {

        private InventoryState receiver;

        public CloseInventoryCommand(InventoryState receiver)
        {
            this.receiver = receiver;
        }

        public void Execute()
        {
            receiver.CloseInventory();
        }
    }
}
