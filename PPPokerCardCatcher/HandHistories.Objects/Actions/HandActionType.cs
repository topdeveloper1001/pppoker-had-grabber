﻿namespace HandHistories.Objects.Actions
{
    public enum HandActionType
    {
        FOLD,
        CALL,
        CHECK,
        ANTE,
        SHOW,
        SHOWS_FOR_LOW,
        WINS,
        WINS_THE_LOW,
        WINS_SIDE_POT,
        WINS_TOURNAMENT,
        END_TOURNAMENT,
        DEALT_HERO_CARDS,
        TIES,
        TIES_SIDE_POT,
        RAISE,
        BET,
        SMALL_BLIND,
        BIG_BLIND,
        UNCALLED_BET,
        REQUEST_TIME,
        FIFTEEN_SECONDS_LEFT,
        FIVE_SECONDS_LEFT,
        MUCKS,
        POSTS,
        DISCONNECTED,
        RECONNECTED,
        STANDS_UP,
        SITS_DOWN,
        SITTING_OUT,
        ERROR,
        ADDS,
        TIMED_OUT,
        RETURNED,
        SECONDS_TO_RECONNECT,
        CHAT,
        FEELING_CHANGE,
        ALL_IN,
        GAME_CANCELLED,
        RABBIT,
        JACKPOTCONTRIBUTION,
        STREET,
        UNKNOWN,
        STRADDLE
    }
}