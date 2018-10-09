﻿using ProtoBuf;

namespace DriveHUD.Importers.PPPoker.Model
{
    [ProtoContract]
    class MultiBoard
    {
        [ProtoMember(1)]
        public int[] Board { get; set; } // Original name: boards
    }
}
