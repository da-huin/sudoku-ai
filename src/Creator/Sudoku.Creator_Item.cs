using System;
using System.Collections;
using System.Collections.Generic;

public partial class Sudoku
{
    public partial class Creator
    {
        public static int[,] GetQuestion(int VoidSquareCount)
        {
            if (VoidSquareCount > 9 * 9)
            {
                VoidSquareCount = 9 * 9;
            }

            int[,] fullQuestion;
            Board board = new Board();
            List<int[,]> OverlapTestBoards = new List<int[,]>();
            


            SetBoardRandomFirstSquare(board);

            fullQuestion = Resolver.GetAnswer(BoardToArr(board),false);
            SetBoardRandomHole(fullQuestion, VoidSquareCount);
            

            return fullQuestion;
        }
    }



    private static bool IsSameBoard(Board board, Board board2)
    {

        for (int y = 0; y < 9; y++)
        {
            for (int x=0; x<9; x++)
            {
                if(board[x,y].Num != board2[x,y].Num)
                {
                    return false;
                }
            }
        }

        return true;
    }

    private static bool IsSameBoard(int[,] Arr, int[,] Arr2)
    {
        for (int y = 0; y < 9; y++)
        {
            for (int x = 0; x < 9; x++)
            {
                if (Arr[x, y] != Arr2[x, y])
                {
                    return false;
                }
            }
        }
        return true;
    }

    private static void SetBoardRandomFirstSquare(Board board)
    {
        SetPossibleNums(board);

        // 첫 번째 칸을 랜덤으로 채운다.
        for (int y = 0; y < 3; y++)
        {
            for (int x = 0; x < 3; x++)
            {
                Point selectedPos = new Point();
                int selectedNumber;
                selectedPos.x = x;
                selectedPos.y = y;

                selectedNumber = GetRandomNumber(board, selectedPos);
                board[selectedPos.x, selectedPos.y].Num = selectedNumber;
                RemoveLinePossiableNums(board, selectedPos, selectedNumber);
            }
        }

    }
    private static void SetBoardRandomHole(int[,] arr, int VoidSquareCount)
    {
        Random[] rand = new Random[3];
        rand[2] = new Random();
        rand[0] = new Random(67890 * rand[2].Next());
        rand[1] = new Random(12345 * rand[2].Next());

        // 구멍뚫기
        for (int i = 0; i < VoidSquareCount; i++)
        {
            int randX = 0;
            int randY = 0;
            do
            {
                randX = rand[0].Next(9);
                randY = rand[1].Next(9);
            } while (arr[randX, randY] == 0);
            arr[randX, randY] = 0;
        }
    }

    private static void SetBoardRandomHole(Board board, int VoidSquareCount)
    {
        Random[] rand = new Random[3];
        rand[2] = new Random();
        rand[0] = new Random(67890 * rand[2].Next());
        rand[1] = new Random(12345 * rand[2].Next());

        // 구멍뚫기
        for (int i = 0; i < VoidSquareCount; i++)
        {
            int randX = 0;
            int randY = 0;
            do
            {
                randX = rand[0].Next(9);
                randY = rand[1].Next(9);
            } while (board[randX, randY].Num == 0);
            board[randX, randY].Num = 0;
        }
    }

    // 배열에 대입 후 랜덤으로 선택하고 리무브한다.(리무브는 따로)
    private static int GetRandomNumber(Board board, Point pos)
    {
        int[] randSelectArr = new int[9];
        int possibleNumsCount;

        possibleNumsCount = board[pos.x, pos.y].possibleNums.Count;

        for (int i = 0; i < possibleNumsCount; i++)
        {
            randSelectArr[i] = board[pos.x, pos.y].possibleNums[i];
        }

        return randSelectArr[new Random().Next(possibleNumsCount)];
    }


}


