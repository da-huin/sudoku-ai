using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public partial class Sudoku
{
    public partial class Tool
    {

        public delegate object PointDel(int x, int y);
        public static void DoInSudoku99(PointDel ActFunc)
        {
            Node node = new Node();


            for (int y = 0; y < 9; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    ActFunc(x, y);
                }
            }
        }

        public static bool IsAnswer(int[,] question)
        {
            Board board = ArrToBoard(question);
            if (
                !IsFullNodeArr(board) ||
                LineErrorChecker(board, 'B') ||
                LineErrorChecker(board, 'X') ||
                LineErrorChecker(board, 'Y'))
            {
                return false;
            }
            return true;
        }

        public static List<string> OverlapTest(int[,] question)
        {
            List<string> retValue = new List<string>();
            Board board = ArrToBoard(question);
            if (LineErrorChecker(board, 'B')) retValue.Add("Square");
            if (LineErrorChecker(board, 'X')) retValue.Add("Horizontal");
            if (LineErrorChecker(board, 'Y')) retValue.Add("Vertical");

            return retValue;
        }

        public static int[,] GetArrayForSdoku(int[,] arr)
        {
            return RevorseArrXY(arr);
        }

        public static int[,] GetArrayForArray(int[,] arr)
        {
            return RevorseArrXY(arr);
        }

        public static List<Point> GetOverlapPoint(int[,] Arr, Point pos)
        {
            int OverlapTestNum = Arr[pos.x, pos.y];
            List<Point> OverlapPointList = new List<Point>();
            List<Point> retValue = new List<Point>();


            if (Sudoku.Tool.OverlapTest(Arr).Count != 0)
            {
                DoBXYLines((piece) =>
                {
                    if (piece.Num == OverlapTestNum)
                    {
                        // 중복된 포지션이 있으면 삭제
                        // 결과가 홀수이면 삭제
                        OverlapPointList.Add(piece.pos);
                        
                    }
                }, Arr, pos);


                foreach (var piece in OverlapPointList)
                {
                    bool canAdd = true;
                    foreach (var value in retValue)
                    {
                        if (value.x == piece.x && value.y == piece.y)
                            canAdd = false;
                    }

                    if (canAdd == true)
                    {
                        retValue.Add(piece);
                    }
                }
            }
            // 한 라인에 그 겹치는 숫자가 2개 이상일 때만 추가
            if (retValue.Count == 1)
                retValue.Clear();
            return retValue;
        }

    }
}
