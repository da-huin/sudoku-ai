using System;
using System.Collections;
using System.Collections.Generic;


public partial class Sudoku
{
    public partial class Resolver
    {
        public static List<int[,]> GetProcessList(int[,] sudokuArr, bool fast)
        {
            return GetVarietyAnswer(sudokuArr, "ProcessList", fast) as List<int[,]>;
        }
            
        public static List<int[,]> GetAnswerList(int[,] sudokuArr)
        {
            return GetVarietyAnswer(sudokuArr, "List",true) as List<int[,]>;
        }


        public static int[,] GetAnswer(int[,] sudokuArr, bool fast)
        {
            return GetVarietyAnswer(sudokuArr, "Arr",fast) as int[,];
        }

        private static object GetVarietyAnswer(int[,] sudokuArr, string ListOrArr, bool fast)
        {
            var processBoard = new Stack<Board>();
            object retValue = null;
            var AllBoard = new Stack<Board>();
            Board currentBoard = new Board();
            Board prevBoard = null;
            do
            {
                if (sudokuArr.Length != 9 * 9)
                {
                    Console.WriteLine("ERROR !SIZE");
                    retValue = null;
                    break;
                }
                if(GetBoardNodeCount(ArrToBoard(sudokuArr), 'F')==0)
                {
                    retValue = sudokuArr;
                    break;
                }


                if (IsFullNodeArr(ArrToBoard(sudokuArr)))
                {
                    //Console.WriteLine("FULL ARR");
                    retValue = sudokuArr;
                    break;
                }

                if (LineErrorAllCheck(currentBoard))
                {
                    retValue = sudokuArr;
                    break;
                }

                AllBoard.Push(currentBoard);

                SetNodeArrFromintArr(currentBoard, sudokuArr);


                // 가능한 숫자를 전부 대입한다
                foreach (var piece in currentBoard.nodeBoard)
                {
                    if (piece.Num != 0) continue;

                    foreach (int num in GetPossibleNums(currentBoard, piece.pos))
                    {
                        piece.possibleNums.Add(num);
                    }
                }

                if (!FillLastOne(currentBoard))
                {
                    //Debug.Log("REAL ERROR First Errorcheck is true");
                    Console.WriteLine("REAL ERROR First Errorcheck is true");
                    retValue = null;
                    break;
                }

                if (IsFullNodeArr(currentBoard))
                {
                    //Console.WriteLine("Complete");
                    retValue = RevorseArrXY(BoardToArr(currentBoard));
                    break;
                }

                while (true)
                {

                    if (ErrorCheck(currentBoard) == true)
                    {
                        Console.WriteLine("TEST ERROR CHECK IS TRUE");
                        //Debug.Log("TEST ERROR CHECK IS TRUE");
                    }


                    currentBoard = GetBoardSelected(currentBoard, fast);
                    if (currentBoard == null)
                    {
                        Console.WriteLine("GetBoardSelected error");
                        //Debug.Log("GetBoardSelected error");
                        retValue = null;
                        break;
                    }

                    // 채우는 것에서 중복이 발생했을 때 
                    if (!FillLastOne(currentBoard))
                    {
                        prevBoard = AllBoard.Pop();
                        CurrentNodeToPrevNode(prevBoard, currentBoard).possibleNums.Remove(currentBoard.selectedNum);

                        while (true)
                        {
                            if (CurrentNodeToPrevNode(prevBoard, currentBoard).possibleNums.Count == 0)
                            {
                                currentBoard = prevBoard;
                                prevBoard = AllBoard.Pop();
                                CurrentNodeToPrevNode(prevBoard, currentBoard).possibleNums.Remove(currentBoard.selectedNum);
                            }
                            else
                            {
                                currentBoard = prevBoard;
                                break;
                            }
                        }
                    }
                    // 채우는 것에서 오류가 발생하지 않았다면
                    else
                    {
                        // 보드가 전부 찼다면
                        if (IsFullNodeArr(currentBoard) == true)
                        {
                            //Console.WriteLine("Complete");
                            retValue = RevorseArrXY(BoardToArr(currentBoard));
                            break;
                        }
                        // 보드가 전부 차진 않았다면(2가지 이상이 또 남았을 때)
                    }
                    //SudokuLibrary.Program.PRINT(BoardToArr(currentBoard));
                    processBoard.Push(currentBoard);
                    AllBoard.Push(currentBoard);
                }

            } while (false);
            if (retValue != null)
            {
                if (ListOrArr == "List")
                {
                    AllBoard.Push(currentBoard);
                    retValue = ReverseStackXY(AllBoard);
                }
                else if (ListOrArr == "ProcessList")
                {
                    processBoard.Push(currentBoard);
                    retValue = ReverseStackXY(processBoard);
                }
            }
            return retValue;
        }

        private static List<int[,]> ReverseStackXY(Stack<Board> allBoard)
        {
            var retValue = new List<int[,]>();
            var arr = allBoard.ToArray();

            foreach (var piece in arr)
            {
                //retValue.Add(RevorseArrXY(BoardToArr(piece)));
                retValue.Insert(0,RevorseArrXY(BoardToArr(piece)));
            }
            return retValue;
        }

    }
}
