using GameFramework;
using GameFramework.DataTable;
using GameFramework.Event;
using GameFramework.Procedure;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace Hamood
{

    /// <summary>
    /// 切换场景流程
    /// </summary>
    public partial class ProcedureChangeScene : ProcedureBase
    {
        // 加载场景完成参数
        private bool m_IsChangeSceneComplete = false;
        // 背景音乐ID
        private int m_BackgroundMusicId = 0;
        // 切换的场景ID
        private int gotoSceneId = 0;

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            // 加载场景参数完成参数（意为加载场景未完成）
            m_IsChangeSceneComplete = false;
            // 订阅各种加载场景事件(事件为内置事件无需派发)
            GameEntry.Event.Subscribe(LoadSceneSuccessEventArgs.EventId, OnLoadSceneSuccess);
            GameEntry.Event.Subscribe(LoadSceneFailureEventArgs.EventId, OnLoadSceneFailure);
            GameEntry.Event.Subscribe(LoadSceneUpdateEventArgs.EventId, OnLoadSceneUpdate);
            GameEntry.Event.Subscribe(LoadSceneDependencyAssetEventArgs.EventId, OnLoadSceneDependencyAsset);

            // 停止所有声音（包括已经加载的和正在加载的）
            GameEntry.Sound.StopAllLoadingSounds();
            GameEntry.Sound.StopAllLoadedSounds();

            // 隐藏所有实体（包括已经加载的和正在加载的）
            GameEntry.Entity.HideAllLoadingEntities();
            GameEntry.Entity.HideAllLoadedEntities();

            // 卸载所有场景
            string[] loadedSceneAssetNames = GameEntry.Scene.GetLoadedSceneAssetNames();
            for (int i = 0; i < loadedSceneAssetNames.Length; i++)
            {
                GameEntry.Scene.UnloadScene(loadedSceneAssetNames[i]);
            }

            // 还原游戏速度
            GameEntry.Base.ResetNormalGameSpeed();

            //读取流程状态机中的数据
            gotoSceneId = procedureOwner.GetData<VarInt>(Constant.ProcedureData.NextSceneId).Value;
            // 读取场景数据表
            IDataTable<DRScene> dtScene = GameEntry.DataTable.GetDataTable<DRScene>();
            // 获取场景ID
            DRScene drScene = dtScene.GetDataRow(gotoSceneId);
            if (drScene == null)
            {
                Log.Warning("Can not load scene '{0}' from data table.", gotoSceneId.ToString());
                return;
            }
            // 根据场景名称加载场景（场景会与GF场景叠加）
            GameEntry.Scene.LoadScene(AssetUtility.GetSceneAsset(drScene.AssetName), this);
            // 将场景ID赋给背景音乐ID（二者的数值已经在数据表中设置相同）
            m_BackgroundMusicId = drScene.BackgroundMusicId;
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            // 取消订阅事件
            GameEntry.Event.Unsubscribe(LoadSceneSuccessEventArgs.EventId, OnLoadSceneSuccess);
            GameEntry.Event.Unsubscribe(LoadSceneFailureEventArgs.EventId, OnLoadSceneFailure);
            GameEntry.Event.Unsubscribe(LoadSceneUpdateEventArgs.EventId, OnLoadSceneUpdate);
            GameEntry.Event.Unsubscribe(LoadSceneDependencyAssetEventArgs.EventId, OnLoadSceneDependencyAsset);

            base.OnLeave(procedureOwner, isShutdown);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            // 场景没有加载成功就return
            if (!m_IsChangeSceneComplete)
            {
                return;
            }
            //TODO：在这里根据切换到的场景编号进行对应的流程切换
            switch (gotoSceneId)
            {
                // 切换到菜单流程
                case 1:
                    ChangeState<ProcedureMenu>(procedureOwner);
                    break;
                // 切换到主流程
                case 2:
                    ChangeState<ProcedureMain>(procedureOwner);
                    break;

                default:
                    break;
            }
        }
        // 加载成功事件订阅的函数
        private void OnLoadSceneSuccess(object sender, GameEventArgs e)
        {
            // 获取场景名称
            LoadSceneSuccessEventArgs ne = (LoadSceneSuccessEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            Log.Info("Load scene '{0}' OK.", ne.SceneAssetName);
            // 播放背景音乐
            if (m_BackgroundMusicId > 0)
            {
                GameEntry.Sound.PlayMusic(m_BackgroundMusicId);
                Log.Info("Load music '{0}' OK.", m_BackgroundMusicId);
            }
            // 加载场景参数完成参数（意为加载场景完成）
            m_IsChangeSceneComplete = true;
        }
        // 加载失败事件订阅的函数
        private void OnLoadSceneFailure(object sender, GameEventArgs e)
        {
            LoadSceneFailureEventArgs ne = (LoadSceneFailureEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            Log.Error("Load scene '{0}' failure, error message '{1}'.", ne.SceneAssetName, ne.ErrorMessage);
        }
        // 正在加载事件订阅的函数
        private void OnLoadSceneUpdate(object sender, GameEventArgs e)
        {
            LoadSceneUpdateEventArgs ne = (LoadSceneUpdateEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            Log.Info("Load scene '{0}' update, progress '{1}'.", ne.SceneAssetName, ne.Progress.ToString("P2"));
        }

        private void OnLoadSceneDependencyAsset(object sender, GameEventArgs e)
        {
            LoadSceneDependencyAssetEventArgs ne = (LoadSceneDependencyAssetEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            Log.Info("Load scene '{0}' dependency asset '{1}', count '{2}/{3}'.", ne.SceneAssetName, ne.DependencyAssetName, ne.LoadedCount.ToString(), ne.TotalCount.ToString());
        }
    }
}
