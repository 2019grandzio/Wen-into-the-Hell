using GameFramework;
using GameFramework.Procedure;
using System;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
using Random = UnityEngine.Random;
using GameFramework.Event;

namespace Hamood
{
    /// <summary>
    /// 主流程
    /// </summary>
    public class ProcedureMain : ProcedureBase
    {
        /// <summary>
        /// 管道产生时间
        /// </summary>
        private float m_PipeSpawnTime = 0f;
        /// <summary>
        /// 管道产生计时器
        /// </summary>
        private float m_PipeSpawnTimer = 0f;
        /// <summary>
        /// 结束界面ID
        /// </summary>
        private int m_ScoreFormId = -1;
        /// <summary>
        /// 是否返回主菜单
        /// </summary>
        private bool m_IsReturnMenu = false;
 
        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            // 生成显示分数UI
            m_ScoreFormId = GameEntry.UI.OpenUIForm(UIFormId.ScoreForm).Value;
            // 背景生成函数
            GameEntry.Entity.ShowBg(new BgData(GameEntry.Entity.GenerateSerialId(), 1, 1f, 0));
            // 生成文老爷子
            GameEntry.Entity.ShowBird(new BirdData(GameEntry.Entity.GenerateSerialId(), 3, 5f));
            //设置初始管道产生时间
            m_PipeSpawnTime = Random.Range(3f, 5f);
            //订阅返回菜单事件
            GameEntry.Event.Subscribe(ReturnMenuEventArgs.EventId, OnReturnMenu);
        }
 
        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            // 只要游戏正在运行就会执行
            m_PipeSpawnTimer += elapseSeconds;
            if (m_PipeSpawnTimer >= m_PipeSpawnTime)
            {
                m_PipeSpawnTimer = 0;
                //随机设置管道产生时间
                m_PipeSpawnTime = Random.Range(3f, 5f);
                //产生管道
                GameEntry.Entity.ShowPipe(new PipeData(GameEntry.Entity.GenerateSerialId(), 2, 1f));
 
            }
            //切换场景
            if (m_IsReturnMenu)
            {
                m_IsReturnMenu = false;
                // 读取菜单场景ID
                procedureOwner.SetData<VarInt>(Constant.ProcedureData.NextSceneId, GameEntry.Config.GetInt("Scene.Menu"));
                // 先要切换到切换场景流程
                ChangeState<ProcedureChangeScene>(procedureOwner);
            }
        }
        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            // 关闭显示分数UI
            GameEntry.UI.CloseUIForm(m_ScoreFormId);
            //取消订阅返回菜单事件
            GameEntry.Event.Unsubscribe(ReturnMenuEventArgs.EventId, OnReturnMenu);
        }
        private void OnReturnMenu(object sender, GameEventArgs e)
        {
            // 就是说当你点击返回主菜单按钮时他就被执行了
            m_IsReturnMenu = true;
        }

    }
}