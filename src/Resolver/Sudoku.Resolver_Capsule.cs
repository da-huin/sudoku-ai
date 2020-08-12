using System;
using System.Collections;
using System.Collections.Generic;

public partial class Sudoku
{
    public partial class Resolver
    {
        private static Board GetBoardSelected(Board board, bool fast)
        {

                Board retBoard = new Board(board);
            if (fast == false)
            {
                retBoard.selectedNode = GetEmptyNodeInArr(retBoard, new Random().Next(GetBoardNodeCount(retBoard, 'E')));
                //if (retBoard.selectedNode == null) return retBoard;
            }
            else if (fast == true)
            {
                // 경우의 수가 가장 적은 수 가져오기
                int selectIndex = GetLeastPossibleNodeIndex(retBoard);
                retBoard.selectedNode = GetEmptyNodeInArr(retBoard, selectIndex);
            }
            while (true)
            {
                int count = retBoard.selectedNode.possibleNums.Count;

                if (count == 1) break;
                else if (count < 1)
                {
                    //Debug.Log("ERROR!");
                    return null;
                }

                retBoard.selectedNode.possibleNums.RemoveAt(0);
            }
            retBoard.selectedNum = retBoard.selectedNode.possibleNums[0];

            return retBoard;
        }

        private static int GetLeastPossibleNodeIndex(Board board)
        {
            int LeastCount = 9 * 9;
            int LeastCountIndex = 0;
            int nowCount = 0;

            for (int i = 0; i < GetBoardNodeCount(board, 'E'); i++)
            {
                nowCount = (GetEmptyNodeInArr(board, i).possibleNums.Count);
                if (nowCount < LeastCount)
                {
                    LeastCount = nowCount;
                    LeastCountIndex = i;
                }
            }
            return LeastCountIndex;
        }


        private static Node CurrentNodeToPrevNode(Board prevBoard, Board currentBoard)
        {
            return prevBoard.nodeBoard[currentBoard.selectedNode.pos.x, currentBoard.selectedNode.pos.y];
        }


        private static bool FillLastOne(Board board)
        {
            while (true)
            {
                bool addCheck = false;
                foreach (var piece in board.nodeBoard)
                {
                    if (piece.Num != 0) continue;

                    if (piece.possibleNums.Count == 1)
                    {

                        piece.Num = piece.possibleNums[0];
                        piece.possibleNums.RemoveAt(0);
                        RemoveLinePossiableNums(board, piece.pos, piece.Num);

                        addCheck = true;

                        // 에러체크
                        // 가능한 숫자가 0개 인 것이 있을 때 실패한 것으로 간주
                        if (ErrorCheck(board))
                        {
                            return false;
                        }

                    }
                }

                if (addCheck == false)
                    break;
            }
            return true;


        }




    }
}
