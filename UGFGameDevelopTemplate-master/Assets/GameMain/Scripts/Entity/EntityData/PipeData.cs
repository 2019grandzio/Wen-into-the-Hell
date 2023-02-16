﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Hamood
{
    /// <summary>
    /// 管道实体数据
    /// </summary>
    public class PipeData : EntityData
    {
        /// <summary>
        /// 移动速度
        /// </summary>
        public float MoveSpeed { get; private set; }
 
        /// <summary>
        /// 上管道偏移量
        /// </summary>
        public float OffsetUp { get; private set; }
 
        /// <summary>
        /// 下管道偏移量
        /// </summary>
        public float OffsetDown { get; private set; }
 
        /// <summary>
        /// 到达此目标时隐藏自身位置
        /// </summary>
        public float HideTarget { get; private set; }
        
        public PipeData(int entityId, int typeId,float moveSpeed) : base(entityId, typeId)
        {
            MoveSpeed = moveSpeed;
            OffsetUp = Random.Range(4.5f, 7f);
            OffsetDown = Random.Range(-3.2f, -4.5f);
            HideTarget = -14f;
        }
    }
}