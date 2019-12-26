
using System.Collections;
using System.Collections.Generic;
using Moonma.Share;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


//计算几何-两条线段是否相交（三种算法） https://www.sohu.com/a/206775811_821349
public class LineCross
{
    public int rowTotal = 17;
    public int colTotal = 11;
    float posZ = -1f;
    static private LineCross _main = null;
    public static LineCross main
    {
        get
        {
            if (_main == null)
            {
                _main = new LineCross();
            }
            return _main;
        }
    }

    //cross
    public void ClearCross(LineInfo info)
    {
        info.isCross = false; 
    }


    //true 相交
    public bool CheckCross(LineInfo src, LineInfo dst)
    {
        return SegmentIntersectSegment(src.ptStart, src.ptEnd, dst.ptStart, dst.ptEnd);
    }

    //判断直线AB是否与线段CD相交

    bool LineIntersectSegment(Vector2 A, Vector2 B, Vector2 C, Vector2 D)
    {

        // A(x1, y1), B(x2, y2)的直线方程为：

        // f(x, y) = (y - y1) * (x1 - x2) - (x - x1) * (y1 - y2) = 0

        float fC = (C.y - A.y) * (A.x - B.x) - (C.x - A.x) * (A.y - B.y);

        float fD = (D.y - A.y) * (A.x - B.x) - (D.x - A.x) * (A.y - B.y);

        if (fC * fD > 0)

            return false;

        if ((C == A) || (C == B) || (D == A) || (D == B))
        {
            //有一个端点重合
            return false;
        }

        return true;

    }

    //判断线段B是否与线段CD相交
    bool SegmentIntersectSegment(Vector2 A, Vector2 B, Vector2 C, Vector2 D)
    {
        if (!LineIntersectSegment(A, B, C, D))
            return false;

        if (!LineIntersectSegment(C, D, A, B))
            return false;

        return true;

    }
}
