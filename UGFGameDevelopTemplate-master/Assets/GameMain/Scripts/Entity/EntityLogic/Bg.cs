using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Hamood
{
    /// <summary>
    /// 背景实体脚本（在游戏运行后会被挂载到相应物体上）
    /// </summary>
    public class Bg : Entity
    {
        /// <summary>
        /// 背景实体数据
        /// </summary>
        private BgData m_BgData = null;
 
        private bool m_IsSpawn = false;
 
        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            // 数据先读一下
            m_BgData = (BgData)userData;
            //修改开始位置（修改缓存位置）
            CachedTransform.SetLocalPositionX(m_BgData.StartPostion);
        }
 
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
 
            //控制背景实体移动（移动肯定会一直移动的）
            CachedTransform.Translate(Vector3.left * m_BgData.MoveSpeed * elapseSeconds, Space.World);
            // 同时满足生成位置小于生成位置与生成参数（维持最大场景数不变）即可
            if (CachedTransform.position.x <= m_BgData.SpawnTarget && m_IsSpawn == false)
            {
                //显示背景实体
                GameEntry.Entity.ShowBg(new BgData(GameEntry.Entity.GenerateSerialId(), m_BgData.TypeId, m_BgData.MoveSpeed, 17.92f));
                // 参数判断是否应该生成新的背景
                m_IsSpawn = true;
            }
 
            if (CachedTransform.position.x <= m_BgData.HideTarget)
            {
                //隐藏实体（判断依据是类BgData中的HideTarget，此前在BgData脚本中定义过）
                GameEntry.Entity.HideEntity(this);
            }
        }
        // 当物体被隐藏时
        // 生成新背景的参数修改为false
        protected override void OnHide(object userData)
        {
            base.OnHide(userData);
            m_IsSpawn = false;
        }
    }
}