namespace DriveHUD.Importers.PPPoker.Model
{
    interface IRoomInfo
    {
        int RoomID { get; set; }

        string RoomName { get; set; }

        long Blind { get; set; }

        long Ante { get; set; }

        int SeatNum { get; set; }

        string TempID { get; set; }
    }
}
