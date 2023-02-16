using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using GameFramework.Event;
namespace Hamood
{
	/// <summary>
    /// 管道实体脚本
    /// </summary>
    public class Pipe : Entity
    {
        /// <summary>
        /// 管道实体数据
        /// </summary>
        private PipeData m_PipeData = null;
 
        /// <summary>
        /// 上管道
        /// </summary>
        private Transform m_UpPipe = null;
 
        /// <summary>
        /// 下管道
        /// </summary>
        private Transform m_DownPipe = null;
 
        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            m_PipeData = (PipeData)userData;
            //设置初始位置
            CachedTransform.SetLocalPositionX(10f);
            if (m_UpPipe == null || m_DownPipe == null)
            {
                // 通过名称找到子节点的上下管道
                m_UpPipe = transform.Find("UpPipe");
                m_DownPipe = transform.Find("DownPipe");
            }
 
            //设置上下管道的偏移（数据来源于PipeData的随机生成）
            m_UpPipe.SetLocalPositionY(m_PipeData.OffsetUp);
            m_DownPipe.SetPositionY(m_PipeData.OffsetDown);
            //订阅事件
            GameEntry.Event.Subscribe(RestartEventArgs.EventId, OnRestart);
           
            
        }
 
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            //控制管道实体移动（移动肯定会一直移动的）
            CachedTransform.Translate(Vector3.left * m_PipeData.MoveSpeed * elapseSeconds, Space.World);
            // 隐藏设置（数据来源于PipeData）
            if (CachedTransform.position.x <= m_PipeData.HideTarget)
            {
                //隐藏自身
                GameEntry.Entity.HideEntity(this);
            }
 
 
        }
        
        // 当物体被隐藏时
        protected override void OnHide(object userData)
        {
            base.OnHide(userData);
            /*(这两个逼是多余的操作)
            m_UpPipe.gameObject.SetActive(false);
            m_DownPipe.gameObject.SetActive(false);*/
            //取消订阅事件
            GameEntry.Event.Unsubscribe(RestartEventArgs.EventId, OnRestart);
        }
        private void OnRestart(object sender, GameEventArgs e)
        {
            GameEntry.Entity.HideEntity(this);
        }
    }
}