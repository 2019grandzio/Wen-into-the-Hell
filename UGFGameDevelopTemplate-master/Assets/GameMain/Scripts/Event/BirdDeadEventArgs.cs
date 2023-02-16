using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;

namespace Hamood
{
	/// <summary>
	/// 死亡事件
	/// </summary>
	public class BirdDeadEventArgs : GameEventArgs
	{
		/// <summary>
        /// 加载配置成功事件编号。
        /// </summary>
		public static readonly int EventId = typeof(BirdDeadEventArgs).GetHashCode();
		/// <summary>
        /// 获取加载配置成功事件编号。
        /// </summary>
		public override int Id
		{
			get
			{
				return EventId;
			}
		}
		/// <summary>
        /// 清理加载配置成功事件。
        /// </summary>
		public override void Clear()
		{
		}
	}
}
