using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using PhotonHashTable = ExitGames.Client.Photon.Hashtable;
using Photon.Pun;
using Photon.Pun.UtilityScripts;

namespace Jc
{
    public static class CustomProperty
    {
        // 플레이어 관련 프로퍼티
        #region Player
        #region Player Ready
        public const string READY = "Ready";
        public static void SetReady(this Player player, bool value)
        {
            PhotonHashTable customProperty = new PhotonHashTable();
            customProperty[READY] = value;
            player.SetCustomProperties(customProperty);
        }
        public static bool GetReady(this Player player)
        {
            PhotonHashTable customProperty = player.CustomProperties;
            return customProperty.TryGetValue(READY, out object value) ? (bool)value : false;
        }
        #endregion
        #region Player Load
        public const string LOAD = "Load";
        public static void SetLoad(this Player player, bool value)
        {
            PhotonHashTable customProperty = new PhotonHashTable();
            customProperty[LOAD] = value;
            player.SetCustomProperties(customProperty);
        }
        public static bool GetLoad(this Player player)
        {
            PhotonHashTable customProperty = player.CustomProperties;
            return customProperty.TryGetValue(LOAD, out object value) ? (bool)value : false;
        }
        #endregion
        #region Player Chef
        public const string CHEF = "Chef";
        public static void SetChef(this Player player, int value)
        {
            PhotonHashTable customProperty = new PhotonHashTable();
            customProperty[CHEF] = value;
            player.SetCustomProperties(customProperty);
        }
        public static int GetChef(this Player player)
        {
            PhotonHashTable customProperty = player.CustomProperties;
            return customProperty.TryGetValue(CHEF, out object value) ? (int)value : 0;
        }
        #endregion
        #endregion

        // 방 관련 프로퍼티
        #region Room
        public const string GAMESTART = "GameStart";
        public static void SetGameStart(this Room room, bool value)
        {
            PhotonHashTable customProperty = new PhotonHashTable();
            customProperty[GAMESTART] = value;
            room.SetCustomProperties(customProperty);
        }
        public static bool GetGameStart(this Room room)
        {
            PhotonHashTable customProperty = room.CustomProperties;
            return customProperty.TryGetValue(GAMESTART, out object value) ? (bool)value : false;
        }

        public const string GAMESTARTTIME = "GameStartTime";
        public static void SetGameStartTime(this Room room, double value)
        {
            PhotonHashTable customProperty = new PhotonHashTable();
            customProperty[GAMESTARTTIME] = value;
            room.SetCustomProperties(customProperty);
        }
        public static double GetGameStartTime(this Room room)
        {
            PhotonHashTable customProperty = room.CustomProperties;
            return customProperty.TryGetValue(GAMESTARTTIME, out object value) ? (double)value : 0;
        }
        #endregion
    }
}