﻿using ProtoBuf;

namespace PPPokerCardCatcher.Importers.PPPoker.Model
{
    [ProtoContract]
    class OtherLeaveRoomBRC
    {
        [ProtoMember(1)]
        public long Uid { get; set; }
    }
}
