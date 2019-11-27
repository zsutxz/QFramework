using System.Collections.Generic;
using UnityEngine;

public class GridArray {
    public const int ColNum = 5;
    public const int RowNum = 4;
    private GridPos[, ] array;

    private int[] OffsetX = { 0, 1, -1, 0 };
    private int[] OffsetY = { 1, 0, 0, -1 };
    public GridArray () {
        //RowNum+1 其中+1是看看不见的那行，用于生成位置使用。
        array = new GridPos[RowNum + 1, ColNum];
        for (int i = 0; i < RowNum + 1; i++) {
            for (int j = 0; j < ColNum; j++) {
                array[i, j] = new GridPos () { PosRow = i, PosCol = j };
            }
        }
    }

    public GridPos this [int row, int col] {
        get { return array[row, col]; }
        set { array[row, col] = value; }
    }
    public List<List<Stone>> CalculateArray () {

        int[, ] travelFlag = new int[RowNum, ColNum];
        for (int i = 0; i < RowNum; i++) {
            for (int j = 0; j < ColNum; j++) {
                travelFlag[i, j] = 0;
            }
        }
        List<List<Stone>> ret = new List<List<Stone>> ();

        int travelId = 1; // 用来标准是否遍历的标志
        for (int i = 0; i < RowNum; i++) {
            for (int j = 0; j < ColNum; j++) {
                GridPos g = array[i, j];

                if (travelFlag[i, j] != 0) { continue; } //以及被遍历了

                if (g.Stone == null) {; continue; }

                //一个新的遍历起点
                travelId++;
                travelFlag[i, j] = travelId;

                //找到一个符文开始遍历周围， 使用的是广度搜索
                List<Stone> sameType = new List<Stone> ();
                Queue<GridPos> q = new Queue<GridPos> ();
                ret.Add (sameType);
                q.Enqueue (g);
                sameType.Add (g.Stone);
                StonType type = g.Stone.type;
                while (q.Count > 0) {
                    GridPos p = q.Dequeue ();
                    for (int offset = 0; offset < 4; offset++) {
                        int r = p.PosRow + OffsetX[offset];
                        int c = p.PosCol + OffsetY[offset];

                        //出了边界，被访问过
                        if (r < 0 || c < 0 || r >= RowNum || c >= ColNum || travelFlag[r, c] == travelId)
                            continue;

                        if (array[r, c].Stone == null) //空
                            continue;

                        Stone s = array[r, c].Stone;
                        if (s.type != type)
                            continue;

                        q.Enqueue (array[r, c]);
                        travelFlag[r, c] = travelId; //标记被访问过该位置

                        //防止同一个石头重复加入，超过1X1可能会这种情况
                        if (sameType.IndexOf (s) < 0)
                            sameType.Add (s);
                    }
                }
            }
        }
        return ret;
    }

    //整列下落填满空隙
    public bool DropFillEmpty () {
        bool moveFlag = false;
        for (int r = 1; r < RowNum; r++) {
            for (int c = 0; c < ColNum; c++) {
                Stone s = array[r, c].Stone;
                if (s == null)
                    continue;
                GridPos g = CanDropDown2Grid (s);
                if (g == s.PositionGrid) //没有空格可以掉落
                    continue;

                ClearStonPosition (s);
                PutStoneToPosition (g, s);
                moveFlag = true;
            }
        }
        return moveFlag;

    }

    public bool FeedNewSton () {
        bool feedFlag = false;
        //处理补给问题
        while (true) {
            int c = 0;
            bool move = false;
            for (c = 0; c < ColNum; c++) {
                int size = GetEmptySize (RowNum - 1, c);
                if (size == 0) //没空位不需要补。
                    continue;

                int generateSize = 1;
                if (size > 1) //空位可以允许可以产生长条的数据
                {
                    if (Random.Range (0f, 1f) > 0.8f)
                        generateSize = 2;
                }

                Stone s = ResInterface.Instance.GetRandomStone (generateSize);
                s.PositionGrid = array[RowNum, c]; //在最高位置
                s.RevisePositionImmediately ();
                GridPos g = CanDropDown2Grid (s);
                feedFlag = true;
                PutStoneToPosition (g, s);
                move = true;
            }

            if (!move)
                break;
        }
        return feedFlag;
    }

    public int GetEmptySize (int r, int c) {
        int ret = 0;
        var i = c;
        for (; i < ColNum; i++) {
            if (!array[r, i].IsEmpty ())
                break;
            else
                ret++;
        }
        return ret;
    }
    public void PutStoneToPosition (GridPos grid, Stone stone) {
        stone.PositionGrid = grid;
        for (var i = 0; i < stone.size; i++) {
            array[grid.PosRow, grid.PosCol + i].Stone = stone;
        }
        stone.DropDow ();
    }

    public void ClearStonPosition (Stone s) {
        var r = s.PositionGrid.PosRow;
        var c = s.PositionGrid.PosCol;
        for (var i = 0; i < s.size; i++) {
            array[r, c + i].Stone = null;
        }
        s.PositionGrid = null;
    }
    public GridPos CanDropDown2Grid (Stone stone) {
        int c = stone.PositionGrid.PosCol;
        int size = stone.size;
        int r = stone.PositionGrid.PosRow;
        for (r = stone.PositionGrid.PosRow - 1; r >= 0; r--) {
            if (!IsEmpty (r, c, size)) {
                break;
            }
        }
        r += 1;
        return array[r, c];
    }

    public bool IsEmpty (int r, int c, int size) {
        for (var s = 0; s < size; s++) {
            if (!array[r, c + s].IsEmpty ()) {
                return false;
            }
        }
        return true;
    }

    public Stone GetNearBottomStone (int c) {
        for (var i = 0; i < RowNum; i++) {
            if (array[i, c].Stone != null)
                return array[i, c].Stone;

        }
        return null;
    }

    public bool AllStoneStatic () {
        for (int r = 1; r < RowNum; r++) {
            for (int c = 0; c < ColNum; c++) {
                Stone s = array[r, c].Stone;
                if (s == null)
                    continue;
                if (s.IsMoving ())
                    return false;
            }
        }
        return true;
    }
}