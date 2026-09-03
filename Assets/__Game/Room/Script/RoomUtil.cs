using Library;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    /// <summary>방 진행 계산 유틸 — 성장 배율·웨이브 조회·적 미리보기·선택지 세트 파싱·보스 추첨·유닛 아이콘 로드</summary>
    public static class RoomUtil
    {
        #region Function
        /// <summary>_roomIndex 방의 적 HP 배율을 반환한다 (1 + Room_GrowthHp × (순번 - 1))</summary>
        public static float GetHpScale(int _roomIndex)
        {
            return 1f + TableManager.instance.Const.Room_GrowthHp * (_roomIndex - 1);
        }
        /// <summary>_roomIndex 방의 적 공격력 배율을 반환한다 (1 + Room_GrowthAtk × (순번 - 1))</summary>
        public static float GetAtkScale(int _roomIndex)
        {
            return 1f + TableManager.instance.Const.Room_GrowthAtk * (_roomIndex - 1);
        }
        /// <summary>_roomIndex 가 RoomMin~RoomMax 에 드는 웨이브를 WaveIndex 순으로 반환한다. 없으면 예외</summary>
        public static List<WaveTable> GetWaves(int _roomIndex)
        {
            var waves = new List<WaveTable>();
            foreach (var wave in TableManager.instance.Wave.Data.Values)
                if (wave.RoomMin <= _roomIndex && _roomIndex <= wave.RoomMax)
                    waves.Add(wave);
            if (waves.Count == 0)
                throw new InvalidOperationException($"Wave 테이블에 방 순번 {_roomIndex} 구간이 없다");
            waves.Sort((a, b) => a.WaveIndex.CompareTo(b.WaveIndex));
            return waves;
        }
        /// <summary>_roomIndex 방 전체 웨이브의 적 종류별 합계를 등장 순으로 반환한다</summary>
        public static SEnemyPreview[] GetPreview(int _roomIndex)
        {
            var order = new List<string>();
            var counts = new Dictionary<string, int>();
            foreach (var wave in GetWaves(_roomIndex))
            {
                var slots = new (string id, int count)[] { (wave.Enemy1Id, wave.Enemy1Count), (wave.Enemy2Id, wave.Enemy2Count), (wave.Enemy3Id, wave.Enemy3Count) };
                foreach (var (id, count) in slots)
                {
                    if (string.IsNullOrEmpty(id) || count <= 0)
                        continue;
                    if (!counts.ContainsKey(id))
                    {
                        order.Add(id);
                        counts.Add(id, 0);
                    }
                    counts[id] += count;
                }
            }
            var result = new SEnemyPreview[order.Count];
            for (int i = 0; i < order.Count; i++)
                result[i] = new SEnemyPreview(order[i], counts[order[i]]);
            return result;
        }
        /// <summary>"좌/우" 형식의 선택지 세트 _set 을 두 방 종류로 나눠 반환한다. 형식이 다르면 예외</summary>
        public static (string left, string right) ParseChoiceSet(string _set)
        {
            var parts = string.IsNullOrEmpty(_set) ? null : _set.Split('/');
            if (parts == null || parts.Length != 2)
                throw new FormatException($"선택지 세트 형식이 \"좌/우\" 가 아니다 : {_set}");
            return (parts[0], parts[1]);
        }
        /// <summary>Boss 테이블에서 보스 ID 하나를 무작위로 반환한다. 행이 없으면 예외</summary>
        public static string RollBoss()
        {
            var ids = TableManager.instance.Boss.ID;
            if (ids.Count == 0)
                throw new InvalidOperationException("Boss 테이블이 비어 있다");
            return ids[UnityEngine.Random.Range(0, ids.Count)];
        }
        /// <summary>Enemy·Boss 테이블 _unitId 의 Icon 프레임을 Resources/SpriteAnim 에서 읽어 반환한다. 행·파일이 없으면 예외</summary>
        public static Sprite LoadUnitIcon(string _unitId)
        {
            var table = TableManager.instance;
            string icon;
            if (table.Enemy.Data.TryGetValue(_unitId, out var enemy))
                icon = enemy.Icon;
            else if (table.Boss.Data.TryGetValue(_unitId, out var boss))
                icon = boss.Icon;
            else
                throw new ArgumentException($"Enemy·Boss 테이블에 없는 ID : {_unitId}", nameof(_unitId));
            var sprite = Resources.Load<Sprite>($"SpriteAnim/{icon}");
            if (sprite == null)
                throw new InvalidOperationException($"Resources/SpriteAnim/{icon} 스프라이트가 없다 ({_unitId})");
            return sprite;
        }
        #endregion
    }
}
