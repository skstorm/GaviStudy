using System.Text;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

namespace Ark.Util
{
	public class CommonFunc
	{
		/// <summary>
		/// 座標の存在方向の定数
		/// </summary>
		private enum PositionSide
		{
			NONE,
			LEFT,           // 左側にある
			ON_THE_LINE,    // ギリギリにある
			RIGHT           // 右側にある
		}

		/*
		* FromからToまでのパスを取得する
		@param isIncludeParent : 親のパスを含めるかどうか
		*/
		public static string GetPath(GameObject fromObj, GameObject toObj, bool isIncludeParent)
		{
			// toObjから逆上りながらパスを作る
			StringBuilder sb = new StringBuilder(SystemText.EMPTY.Text());
			GameObject toParentObj = toObj.transform.parent.gameObject;
			if (toParentObj == null)
			{
				sb.Append(toObj.name);
			}
			else if (toParentObj == fromObj)
			{
				if (isIncludeParent)
				{
					sb.Append(toParentObj.name);
					sb.Append(SystemText.SLASH.Text());
				}
				sb.Append(toObj.name);
			}
			else
			{
				string path = GetPath(fromObj, toParentObj, isIncludeParent); ;
				sb.Append(path);
				sb.Append(SystemText.SLASH.Text());
				sb.Append(toObj.name);
			}

			return sb.ToString();
		}

		/// <summary>
		/// (Clone)を削除する
		/// </summary>
		public static string RemoveClone(string str)
		{
			return str.Replace("(Clone)", "");
		}


		/*
		* 当たり判定
		*/
		public static bool CollisionCircle(float x1, float y1, float r1, float x2, float y2, float r2)
		{
			float dx = x2 - x1;
			float dy = y2 - y1;
			float radiusSqr = (r1 + r2) * (r1 + r2);
			float distanceSqr = dx * dx + dy * dy;
			if (distanceSqr < radiusSqr)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		/*
		 * 点が四角形の中にあるかどうかを判定する関数
		 */
		public static bool IsPointInRect(float pointX, float pointY, float rectX, float rectY, float rectWidth, float rectHeight)
		{
			float halfRectWidth = rectWidth * 0.5f;
			float halfRectHeight = rectHeight * 0.5f;
			float left = rectX - halfRectWidth;
			float right = rectX + halfRectWidth;
			float top = rectY + halfRectHeight;
			float bottom = rectY - halfRectHeight;

			if (pointX < left) return false;
			else if (pointX > right) return false;
			if (pointY > top) return false;
			else if (pointY < bottom) return false;

			return true;
		}

		/*
		 * 近似値で正規化する
		 */
		public static Vector2 ApproximateNormalize(float x, float y)
		{
			// √２の値をとっておく
			const float ROOT_2 = 1.41421f;
			// 近似の長さを求める 予測できる一番長い数値を求める
			float absX = Mathf.Abs(x);
			float absY = Mathf.Abs(y);
			float apprLength = Mathf.Max(absX, absY) * ROOT_2;

			Vector2 vec = Vector2.zero;
			if (apprLength != 0)
			{
				// 近似の長さで悪
				vec.x = x / apprLength;
				vec.y = y / apprLength;
			}

			return vec;
		}

		public static Vector2 ApproximateNormalize(ref float x, ref float y)
		{
			// √２の値をとっておく
			const float ROOT_2 = 1.41421f;
			// 近似の長さを求める 予測できる一番長い数値を求める
			float apprLength = Mathf.Max(Abs(x), Abs(y)) * ROOT_2;

			Vector2 vec = Vector2.zero;
			if (apprLength != 0)
			{
				// 近似の長さで悪
				vec.x = x * apprLength;
				vec.y = y * apprLength;
			}

			return vec;
		}

		public static float Abs(float a)
		{
			if (a < 0)
			{
				a = -a;
			}
			return a;
		}

		public static Vector2 ApproximateNormalize(Vector2 vec)
		{
			return ApproximateNormalize(vec.x, vec.y);
		}

		/// <summary>
		/// ２つのPositionが、指定の距離の中にあるか
		/// 普通に書いて、 Mathf.Distance(posA, posB) <= distance を早くさせた物
		/// </summary>
		/// <returns><c>true</c> if is in radius the specified posA posB radius; otherwise, <c>false</c>.</returns>
		/// <param name="posA">Position a.</param>
		/// <param name="posB">Position b.</param>
		/// <param name="radius">Radius.</param>
		public static bool IsInDistance(Vector2 posA, Vector2 posB, float distance)
		{
			return (Vector2.SqrMagnitude(posA - posB) <= distance * distance);
		}

		//! ２点間の距離の公式の近似を返す 誤差2.5%~5%
		public static int ApproximateDistance(float dx, float dy)
		{
			int max, min, approx;

			if (dx < 0) dx = -dx;
			if (dy < 0) dy = -dy;

			if (dx < dy)
			{
				min = (int)dx;
				max = (int)dy;
			}
			else
			{
				min = (int)dy;
				max = (int)dx;
			}

			approx = (max * 1007) + (min * 441);
			if (max < (min << 4))
			{
				approx -= (max * 40);
			}

			return ((approx + 512) >> 10);
		}

		/// <summary>
		/// 任意の座標が、4つの座標からなる方形の内側にあれば真を返す
		/// 凸型のみ対応。
		/// 方形の４点を時計回りに辿り、点が常に右側にあれば内側にある。
		/// なお、線上は許容する。
		/// </summary>
		/// <param name="targetPos"></param>
		/// <param name="rectVecs"></param>
		/// <returns></returns>
		public static bool IsInRect(Vector2 targetPos, Vector2[] rectVecs)
		{
			for (int i = 0; i < rectVecs.Length; i++)
			{
				if (positionSide(targetPos, rectVecs[i], rectVecs[(i + 1) % 4]) == PositionSide.LEFT)
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// 任意の座標が線の左右どちら側にあるか調べる。
		/// </summary>
		/// <param name="pos"></param>
		/// <param name="lineStartPos"></param>
		/// <param name="lineEndPos"></param>
		/// <returns></returns>
		private static PositionSide positionSide(Vector2 pos, Vector2 lineStartPos, Vector2 lineEndPos)
		{
			float n = pos.x * (lineStartPos.y - lineEndPos.y) + lineStartPos.x * (lineEndPos.y - pos.y) + lineEndPos.x * (pos.y - lineStartPos.y);
			if (n > 0)
			{
				// 左
				return PositionSide.LEFT;
			}
			else if (n < 0)
			{
				// 右
				return PositionSide.RIGHT;
			}
			else
			{
				// 線上
				return PositionSide.ON_THE_LINE;
			}
		}

		/// <summary>
		/// 2点間で平行四辺形を作り、４頂点の座標を返す
		/// </summary>
		/// <param name="pos"></param>
		/// <param name="selfPos"></param>
		/// <param name="lineWeight"></param>
		/// <returns></returns>
		public static Vector2[] CalcLineRangeRectVertexes(Vector2 pos, Vector2 selfPos, float lineWeight)
		{
			// 先に射程範囲を割り出す
			var dx = pos.x - selfPos.x;
			var dy = pos.y - selfPos.y;
			float angle = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;

			Vector2[] ret = new Vector2[4];

			float width = lineWeight * 0.5f;

			// 計算のため、座標が時計回りになるように割り出す
			if (angle < 45f && angle > -45f)
			{
				// 横長平行四辺形
				//log.D("右方向を縦並行で狙います。");
				ret[0] = new Vector2(selfPos.x, selfPos.y - width);
				ret[1] = new Vector2(selfPos.x, selfPos.y + width);
				ret[2] = new Vector2(pos.x, pos.y + width);
				ret[3] = new Vector2(pos.x, pos.y - width);
			}
			else if (angle >= 45f && angle < 135f)
			{
				// 縦長平行四辺形
				//log.D("上方向を横並行で狙います。");
				ret[0] = new Vector2(selfPos.x + width, selfPos.y);
				ret[1] = new Vector2(selfPos.x - width, selfPos.y);
				ret[2] = new Vector2(pos.x - width, pos.y);
				ret[3] = new Vector2(pos.x + width, pos.y);
			}
			else if (angle <= -45f && angle > -135f)
			{
				// 縦長平行四辺形
				//log.D("下方向を横並行で狙います。");
				ret[0] = new Vector2(selfPos.x + width, selfPos.y);
				ret[1] = new Vector2(pos.x + width, pos.y);
				ret[2] = new Vector2(pos.x - width, pos.y);
				ret[3] = new Vector2(selfPos.x - width, selfPos.y);
			}
			else if (angle >= 135f || angle <= -135f)
			{
				// 横長平行四辺形
				//log.D("左方向を縦並行で狙います。");
				ret[0] = new Vector2(selfPos.x, selfPos.y + width);
				ret[1] = new Vector2(selfPos.x, selfPos.y - width);
				ret[2] = new Vector2(pos.x, pos.y - width);
				ret[3] = new Vector2(pos.x, pos.y + width);
			}

			//log.D("角度: {0} 射程範囲: {1} : {2}, {3} : {4}, {5} : {6}, {7} : {8}",
			//	angle,
			//	ret[0].x, ret[0].y,
			//	ret[1].x, ret[1].y,
			//	ret[2].x, ret[2].y,
			//	ret[3].x, ret[3].y
			//	);

			return ret;
		}

		/// <summary>
		/// 四角形の4頂点を算出して返す。
		/// </summary>
		/// <param name="rectWidth"></param>
		/// <param name="rectHeight"></param>
		/// <param name="targetPosX"></param>
		/// <param name="targetPosY"></param>
		/// <returns></returns>
		public static Vector2[] CalcRectVertexs(float rectWidth, float rectHeight, float targetPosX, float targetPosY)
		{
			float width = rectWidth * 0.5f;
			float height = rectHeight * 0.5f;

			// 四角形の頂点を割り出す
			Vector2[] rectVertexs = new Vector2[4];
			// 左下
			rectVertexs[0] = new Vector2(targetPosX - width, targetPosY - height);
			// 左上
			rectVertexs[1] = new Vector2(targetPosX - width, targetPosY + height);
			// 右上
			rectVertexs[2] = new Vector2(targetPosX + width, targetPosY + height);
			// 右下
			rectVertexs[3] = new Vector2(targetPosX + width, targetPosY - height);

			return rectVertexs;
		}





		/// <summary>
		/// ２つの線が交差していればtrue
		/// </summary>
		/// <param name="lineAStart"></param>
		/// <param name="lineAEnd"></param>
		/// <param name="lineBStart"></param>
		/// <param name="lineBEnd"></param>
		/// <returns> bool </returns>
		public static bool IsLineIntersects(Vector2 lineAStart, Vector2 lineAEnd, Vector2 lineBStart, Vector2 lineBEnd)
		{
			float aStartX = lineAStart.x;
			float aStartY = lineAStart.y;
			float aEndX = lineAEnd.x;
			float aEndY = lineAEnd.y;
			float bStartX = lineBStart.x;
			float bStartY = lineBStart.y;
			float bEndX = lineBEnd.x;
			float bEndY = lineBEnd.y;
			float ta = (bStartX - bEndX) * (aStartY - bStartY) + (bStartY - bEndY) * (bStartX - aStartX);
			float tb = (bStartX - bEndX) * (aEndY - bStartY) + (bStartY - bEndY) * (bStartX - aEndX);
			float tc = (aStartX - aEndX) * (bStartY - aStartY) + (aStartY - aEndY) * (aStartX - bStartX);
			float td = (aStartX - aEndX) * (bEndY - aStartY) + (aStartY - aEndY) * (aStartX - bEndX);

			// 端点を含む
			return tc * td <= 0 && ta * tb <= 0;
		}

		/// <summary>
		/// vをminVとmaxVの中に収める
		/// </summary>
		public static int FitInt(int minV, int v, int maxV)
		{
			if (v < minV)
			{
				v = minV;
			}
			else if (v > maxV)
			{
				v = maxV;
			}
			return v;
		}

		/// <summary>
		/// vをminVとmaxVの中に収める
		/// </summary>
		public static float FitFloat(float minV, float v, float maxV)
		{
			if (v < minV)
			{
				v = minV;
			}
			else if (v > maxV)
			{
				v = maxV;
			}
			return v;
		}

		/// <summary>
		/// vをminVとmaxVの中に収める
		/// Performance確認必要]
		/// </summary>
		public static T FitValue<T>(T minV, T v, T maxV) where T : IComparable
		{
			if (0 < minV.CompareTo(v))
			{
				v = minV;
			}
			else if (0 > maxV.CompareTo(v))
			{
				v = maxV;
			}
			return v;
		}

		/// <summary>
		/// 乗算関数
		/// </summary>
		public static float Pow(float v, int squareNum)
		{
			for (int i = 0; i < squareNum - 1; ++i)
			{
				v *= v;
			}
			return v;
		}

		/// <summary>
		/// 深いコピー
		/// </summary>
		public static T Clone<T>(T obj)
		{
			using (MemoryStream stream = new MemoryStream())
			{
				// バイナリシリアル化を行うためのフォーマッタを作成
				BinaryFormatter f = new BinaryFormatter();

				// 現在のインスタンスをシリアル化してMemoryStreamに格納
				f.Serialize(stream, obj);

				// ストリームの位置を先頭に戻す
				stream.Position = 0L;

				// MemoryStreamに格納された内容を逆シリアル化する
				return (T)f.Deserialize(stream);
			}
		}

		#region comming soon
#if false
		/// <summary>
		/// 矩形と任意の矩形が交差、又はそれぞれ内包していたらtrue
		/// </summary>
		/// <param name="circle"></param>
		/// <param name="rect"></param>
		/// <returns></returns>
		public static bool Overlaps(Rect rect, Vector2[] verts)
		{
			if (IsInRect(rect.position, verts))
			{
				// 矩形の中に矩形が含まれていればtrue
				return true;
			}

			Vector2[] vertsRect = CalcRectVertexs(rect.width, rect.height, rect.position.x, rect.position.y);

			for (int i = 0; i < verts.Length; i++)
			{
				int next = i + 1;
				if (next == verts.Length)
				{
					next = 0;
				}

				if (rect.Contains(verts[i]))
				{
					// 頂点が1個でも入ってればRect中に任意の矩形があるか、交差している。
					return true;
				}

				for (int j = 0; j < vertsRect.Length; j++)
				{
					int nextRect = j + 1;
					if (nextRect == vertsRect.Length)
					{
						nextRect = 0;
					}
					if (IsLineIntersects(verts[i], verts[next], vertsRect[j], vertsRect[nextRect]))
					{
						// 交差している
						return true;
					}
				}
			}

			return false;
		}

		/// <summary>
		/// 円と矩形が交差、又はそれぞれ内包していたらtrue
		/// </summary>
		/// <param name="circle"></param>
		/// <param name="rect"></param>
		/// <returns> bool </returns>
		public static bool Overlaps(ShapeCircle circle, Rect rect)
		{
			Vector2[] verts = CalcRectVertexs(rect.width, rect.height, rect.position.x, rect.position.y);
			return Overlaps(circle, verts);
		}

		/// <summary>
		/// 半円と矩形が交差、又はそれぞれ内包していたらtrue
		/// </summary>
		/// <param name="circle"></param>
		/// <param name="rect"></param>
		/// <returns></returns>
		public static bool Overlaps(ShapeHalfCircle halfCircle, Rect rect)
		{
			switch (halfCircle.Direction)
			{
				case ShapeHalfCircle.DIRECTION.UPWARD:
					if (halfCircle.Position.y > rect.position.y + rect.height * 0.5f)
					{
						// 上向き半円の中心点が、Rectの上端より上なら当たってない
						return false;
					}
					break;
				case ShapeHalfCircle.DIRECTION.DOWNWARD:
					if (halfCircle.Position.y < rect.position.y - rect.height * 0.5f)
					{
						// 下向き半円の中心点が、Rectの下端より下なら当たってない
						return false;
					}
					break;
				default:
					break;
			}
			Vector2[] verts = CalcRectVertexs(rect.width, rect.height, rect.position.x, rect.position.y);
			return Overlaps(halfCircle, verts);
		}

		/// <summary>
		/// 円と矩形が交差、又はそれぞれ内包していたらtrue
		/// </summary>
		/// <param name="circle"></param>
		/// <param name="rect"></param>
		/// <returns></returns>
		public static bool Overlaps(ShapeCircle circle, Vector2[] verts)
		{
			// 矩形の中に円が含まれていればtrue
			if (IsInRect(circle.Position, verts))
			{
				// 矩形の中に円の中心がある
				return true;
			}

			for (int i = 0; i < verts.Length; i++)
			{
				int next = i + 1;
				if (next == verts.Length)
				{
					next = 0;
				}

				if (circle.Contains(verts[i]))
				{
					// 頂点が1個でも入ってれば、円の中に矩形があるか交差している。
					return true;
				}

				if (Overlaps(circle, verts[i], verts[next]))
				{
					// 交差している。
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// 線が円に交差していればtrueを返す
		/// </summary>
		/// <param name="circle"></param>
		/// <param name="lineStartPos"></param>
		/// <param name="lineEndPos"></param>
		/// <returns></returns>
		private static bool Overlaps(ShapeCircle circle, Vector2 lineStartPos, Vector2 lineEndPos)
		{
			Vector2 lineVec = lineEndPos - lineStartPos;
			Vector2 normalisedLine = lineVec.normalized;
			Vector2 startCenterVec = circle.Position - lineStartPos;

			// 始点から円の中心までの距離を内積で求める
			float startToTargetLength = Vector2.Dot(normalisedLine, startCenterVec);

			// 線と円の中心の最短距離を割り出す
			float shortestDistance;
			if (startToTargetLength < 0)
			{
				// 中心から線の始点が最短距離
				shortestDistance = Vector2.Distance(circle.Position, lineStartPos);
			}
			else if (startToTargetLength > Vector2.Distance(lineEndPos, lineStartPos))
			{
				// 中心から線の終点が最短距離
				shortestDistance = Vector2.Distance(circle.Position, lineEndPos);
			}
			else
			{
				// 中心から線への最短距離を割り出す
				shortestDistance = Mathf.Abs(normalisedLine.x * startCenterVec.y - startCenterVec.x * normalisedLine.y);
			}

			if (shortestDistance < circle.Radius)
			{
				// 最短距離が円の半径よりも小さい場合は、交差している
				return true;
			}

			return false;
		}

		/// <summary>
		/// 形状同士がぶつかっていればtrue
		/// </summary>
		/// <param name="circle1"></param>
		/// <param name="circle2"></param>
		/// <returns></returns>
		public static bool Overlaps(ShapeCircle circle1, ShapeCircle circle2)
		{
			return CommonFunc.CollisionCircle(circle1.X, circle1.Y, circle1.Radius, circle2.X, circle2.Y, circle2.Radius);
		}

		/// <summary>
		/// 形状同士がぶつかっていればtrue
		/// </summary>
		/// <param name="rect1"></param>
		/// <param name="rect2"></param>
		/// <returns></returns>
		public static bool Overlaps(ShapeUnityRect rect1, ShapeUnityRect rect2)
		{
			return rect1.Rect.Overlaps(rect2.Rect);
		}

		/// <summary>
		/// 形状同士がぶつかっていればtrue
		/// </summary>
		/// <param name="circle"></param>
		/// <param name="anyRect"></param>
		/// <returns></returns>
		public static bool Overlaps(ShapeCircle circle, ShapeAnyRect anyRect)
		{
			return Overlaps(circle, anyRect.Vertexes);
		}

		/// <summary>
		/// 形状同士がぶつかっていればtrue
		/// </summary>
		/// <param name="halfCircle"></param>
		/// <param name="circle"></param>
		/// <returns></returns>
		public static bool Overlaps(ShapeCircle circle, ShapeHalfCircle halfCircle)
		{
			return halfCircle.Overlaps(circle);
		}

		/// <summary>
		/// 形状同士がぶつかっていればtrue
		/// </summary>
		/// <param name="halfCircle1"></param>
		/// <param name="halfCircle2"></param>
		/// <returns></returns>
		public static bool Overlaps(ShapeHalfCircle halfCircle1, ShapeHalfCircle halfCircle2)
		{
			return halfCircle1.Overlaps(halfCircle2);
		}

		/// <summary>
		/// 形状同士がぶつかっていればtrue
		/// </summary>
		/// <param name="halfCircle1"></param>
		/// <param name="rect"></param>
		/// <returns></returns>
		public static bool Overlaps(ShapeUnityRect rect, ShapeHalfCircle halfCircle1)
		{
			return Overlaps(halfCircle1, rect.Rect);
		}

		/// <summary>
		/// 形状同士がぶつかっていればtrue
		/// </summary>
		/// <param name="anyRect"></param>
		/// <param name="circle"></param>
		/// <returns></returns>
		public static bool Overlaps(ShapeAnyRect anyRect, ShapeCircle circle)
		{
			return Overlaps(circle, anyRect.Vertexes);
		}

		/// <summary>
		/// 形状同士がぶつかっていればtrue
		/// </summary>
		/// <param name="circle"></param>
		/// <param name="rect"></param>
		/// <returns></returns>
		public static bool Overlaps(ShapeCircle circle, ShapeUnityRect rect)
		{
			return Overlaps(circle, rect.Rect);
		}

		/// <summary>
		/// 形状同士がぶつかっていればtrue
		/// </summary>
		/// <param name="rect"></param>
		/// <param name="circle"></param>
		/// <returns></returns>
		public static bool Overlaps(ShapeUnityRect rect, ShapeCircle circle)
		{
			return Overlaps(circle, rect.Rect);
		}

		/// <summary>
		/// 形状同士がぶつかっていればtrue
		/// </summary>
		/// <param name="anyRect"></param>
		/// <param name="rect"></param>
		/// <returns></returns>
		public static bool Overlaps(ShapeAnyRect anyRect, ShapeUnityRect rect)
		{
			return Overlaps(rect.Rect, anyRect.Vertexes);
		}

		/// <summary>
		/// 形状同士がぶつかっていればtrue
		/// </summary>
		/// <param name="rect"></param>
		/// <param name="anyRect"></param>
		/// <returns></returns>
		public static bool Overlaps(ShapeUnityRect rect, ShapeAnyRect anyRect)
		{
			return Overlaps(rect.Rect, anyRect.Vertexes);
		}
#endif
		#endregion // comming soon
	}
}