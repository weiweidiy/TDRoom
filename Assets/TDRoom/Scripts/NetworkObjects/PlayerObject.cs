using System;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class PlayerObject : NetworkBehaviour, IUnit
    {
        public event Action<IUnit, BaseAction, Vector3> onActionCast;

        [SerializeField] TextMeshProUGUI txtName;
        [SerializeField] Image imgHp;
        [SerializeField] TextMeshProUGUI txtHp;
        [SerializeField] NetworkAnimator animator;

        public NetworkVariable<PlayerData> data = new NetworkVariable<PlayerData>();

        ActionManager actionManager = new ActionManager();

        public UnitType UnitType => UnitType.Player;

        public int LineIndex => data.Value.lineIndex;

        /// <summary>
        /// 在spawn之前调用初始化数据
        /// </summary>
        /// <param name="playerData"></param>
        public void Init(PlayerData playerData, IFinder finder)
        {
            data.Value = playerData;
            actionManager.Init(playerData.skillDatas, finder, this);
            actionManager.onActionCast += OnActionCast;
        }

        public void PlayCastSkill()
        {
            animator.SetTrigger("Attack");
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            txtName.text = data.Value.playerName;
            txtHp.text = data.Value.hp.ToString() + "/" + data.Value.maxHp.ToString();
            imgHp.fillAmount = (float)data.Value.hp / data.Value.maxHp;

            data.OnValueChanged += (PlayerData previousValue, PlayerData newValue) =>
            {
                if (previousValue.hp != newValue.hp)
                {
                    // 同步UI
                    txtHp.text = newValue.hp.ToString() + "/" + newValue.maxHp.ToString();
                    imgHp.fillAmount = (float)newValue.hp / newValue.maxHp;
                }
            };
        }

        public void Hurt(int damage)
        {
            var pd = data.Value; // 读取副本
            pd.hp = Mathf.Max(0, pd.hp - damage); // 修改副本
            data.Value = pd; // 写回，触发同步与 OnValueChanged
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        public void UpdateLogic(float deltaTime)
        {
            // 逻辑更新
            actionManager.Update(Time.deltaTime);
        }

        private void OnActionCast(IUnit unit, BaseAction action, Vector3 vector)
        {
            onActionCast?.Invoke(unit, action, vector);
            PlayCastSkill();
        }

        public void AddAction(ActionData data)
        {
            actionManager.AddAction(data);
        }

        public void RemoveAction(ActionData data)
        {
            actionManager.RemoveAction(data);
        }

    }
}
