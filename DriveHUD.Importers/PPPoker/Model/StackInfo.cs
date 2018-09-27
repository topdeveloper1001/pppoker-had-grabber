﻿using ProtoBuf;

namespace DriveHUD.Importers.PPPoker.Model
{
    [ProtoContract]
    class StackInfo // Original: StartInfo
    {
        [ProtoMember(1)]
        public int SeatID { get; set; }

        [ProtoMember(2)]
        public long Chips { get; set; }
    }
}
