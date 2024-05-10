using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    public class LobbyPanel : MonoBehaviour
    {
        
        [SerializeField]
        private RectTransform roomContent;      // 방 엔트리 부모 트랜스폼
        [SerializeField] 
        private RoomEntry roomEntryPrefab;      // 방 엔트리 프리팹    

        // Key : 방 이름, Value : 방 엔트리
        Dictionary<string, RoomEntry> roomDictionary; 

        private void Awake()
        {
            roomDictionary = new Dictionary<string, RoomEntry>();
        }

        private void OnDisable()
        {
            // 딕셔너리에 할당된 모든 엔트리 제거 및 클리어
            foreach (string key in roomDictionary.Keys)
            {
                Destroy(roomDictionary[key].gameObject);
            }
            roomDictionary.Clear();
        }

        public void UpdateRoomList(List<RoomInfo> roomList)
        {
            // 룸 리스트 업데이트
            foreach (RoomInfo info in roomList)
            {
                // 방이 닫힌경우 처리
                if (info.RemovedFromList || !info.IsOpen || !info.IsVisible)
                {
                    // 딕셔너리에 존재할 경우 할당해제 및 제거
                    if (roomDictionary.ContainsKey(info.Name))
                    {
                        Destroy(roomDictionary[info.Name].gameObject);
                        roomDictionary.Remove(info.Name);
                    }

                    continue;
                }

                // 로비에 존재하는 모든 룸엔트리 업데이트
                if (roomDictionary.ContainsKey(info.Name))
                {
                    roomDictionary[info.Name].SetRoomInfo(info);
                }
                // 로비에 룸 엔트리 추가
                else
                {
                    RoomEntry entry = Instantiate(roomEntryPrefab, roomContent);
                    entry.SetRoomInfo(info);
                    roomDictionary.Add(info.Name, entry);
                }
            }
        }

        // 로비 퇴장
        public void LeaveLobby()
        {
            PhotonNetwork.LeaveLobby();
        }
    }
}
