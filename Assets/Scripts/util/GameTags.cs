using System;
using UnityEngine;

[Serializable]
public static class GameTags
{
    public static readonly string PLAYER = "Player";
    public static readonly string PUSHABLE = "Pushable";

    public static string of(GameTagsEnum tag)
    {
        switch (tag)
        {
            case GameTagsEnum.PLAYER:
                return PLAYER;
            case GameTagsEnum.PUSHABLE:
                return PUSHABLE;
            default:
                throw new ArgumentOutOfRangeException(nameof(tag), tag, null);
        }
    }
}

[Serializable]
public enum GameTagsEnum
{
    PLAYER, PUSHABLE
}