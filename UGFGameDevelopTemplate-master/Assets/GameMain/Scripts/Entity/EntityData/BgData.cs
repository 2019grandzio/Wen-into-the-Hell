using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Hamood
{
 
	/// <summary>
    /// 背景实体数据
    /// </summary>
    public class BgData : EntityData
    {
        /// <summary>
        /// 移动速度
        /// </summary>
        public float MoveSpeed { get; private set; }
 
        /// <summary>
        /// 到达此目标时产生新的背景实体位置
        /// </summary>
        public float SpawnTarget { get; private set; }
 
        /// <summary>
        /// 到达此目标时隐藏自身位置
        /// </summary>
        public float HideTarget { get; private set; }
        /// <summary>
        /// 移动起始点位置
        /// </summary>
        public float StartPostion { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId">实体ID（下面这个逼是继承过来的）</param>
        /// <param name="typeId">实体类型ID（上面那个逼是继承过来的）</param>
        /// <param name="moveSpeed">移动速度</param>
        /// <param name="startPostion">生成位置（默认都是0）</param>
        /// <returns></returns>
        public BgData(int entityId, int typeId, float moveSpeed, float startPostion) : base(entityId, typeId)
        {
            MoveSpeed = moveSpeed;
            SpawnTarget = -8.66f;
            HideTarget = -26.4f;
            StartPostion = startPostion;
        }
    }
}
