﻿namespace MCUNetwork.Models
{
    internal class Message
    {
        public int Size { get; private set; }

        public Message(int size) => Size = size;
    }
}
