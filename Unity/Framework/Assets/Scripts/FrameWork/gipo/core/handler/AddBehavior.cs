using System;
using System.Collections.Generic;

namespace gipo.core.handler
{
	/// Gear用Handlerをリストにどういう風に追加するかのクラス
	/// 元はGearへのメソッド登録だったが、Genericが複雑になりすぎるのと、パターン数が多くないということでstaticに変更
	public static class AddBehavior
	{
		/// メソッドタイプ
		public enum MethodType
		{
			lastOnly,
			addTail,
			addHead,
			addError,
		};

		/// 追加メソッド（メソッドタイプを指定する）
		public static void Execute<T>(MethodType methodType, List<T> list, T newProcess)
		{
			switch(methodType)
			{
			case MethodType.lastOnly:
				LastOnly(list, newProcess);
				break;
			case MethodType.addTail:
				AddTail(list, newProcess);
				break;
			case MethodType.addHead:
				AddHead(list, newProcess);
				break;
			case MethodType.addError:
				AddError(list, newProcess);
				break;
			default:
				throw new Exception("存在しないメソッドタイプです");
			}
		}

		/// １つだけ
		private static void LastOnly<T>(List<T> list, T newProcess)
		{
			int last = list.Count - 1;
			list.RemoveAt(last);
			list.Add(newProcess);
		}

		/// 末尾追加
		private static void AddTail<T>(List<T> list, T newProcess)
		{
			list.Add(newProcess);
		}

		/// 先頭追加
		private static void AddHead<T>(List<T> list, T newProcess)
		{
			list.Insert(0, newProcess);
		}

		/// 空なら追加？？？
		private static void AddError<T>(List<T> list, T newProcess)
		{
			if (list.Count == 0)
			{
				list.Add(newProcess);
			}
			else
			{
				throw new Exception("このタスクは複数登録出来ません");
			}
		}

	}
}
