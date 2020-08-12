using System.Collections;
using System.Collections.Generic;


public partial class Sudoku
 { 
    public class Point
    {
        public int x;
        public int y;

        public Point() { }
        public Point(int x, int y) { this.x = x; this.y = y; }
    }


    private class Board
    {
        public Node[,] nodeBoard = new Node[9, 9];
        public Node selectedNode;
        public int selectedNum;
        //public List<int> unSelectedNums = new List<int>();


        public Node this[int num, int num2]
        {
            get
            {
                return nodeBoard[num, num2];
            }
            set
            {
                nodeBoard[num, num2] = value;
            }
        }
        public Node this[Point pos]
        {
            get
            {
                return nodeBoard[pos.x, pos.y];
            }
            set
            {
                nodeBoard[pos.x, pos.y] = value;
            }

        }

        public static implicit operator Node[,] (Board board)
        {
            return board.nodeBoard;
        }

        public Board()
        {
            Tool.DoInSudoku99((x, y) => 
            {
                Node node = new Node();
                node.pos.x = x;
                node.pos.y = y;

                nodeBoard[x, y] = node;
                return null;
            });

        }
        public Board(Board BeClone)
        {

            for (int y = 0; y < 9; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    this.nodeBoard[x, y] = new Node();
                    this.nodeBoard[x, y].Num = BeClone.nodeBoard[x, y].Num;
                    this.nodeBoard[x, y].pos = BeClone.nodeBoard[x, y].pos;
                    this.nodeBoard[x, y].possibleNums = new List<int>();
                    foreach (var piece in BeClone.nodeBoard[x, y].possibleNums)
                    {
                        nodeBoard[x, y].possibleNums.Add(piece);
                    }
                }
            }
        }
    }


    private class Node
    {
        public Point pos = new Point();
        public int Num;
        public List<int> possibleNums = new List<int>();
    }

    private static Board ArrToBoard(int[,] Arr)
    {
        Board retValue = new Board();

        Tool.DoInSudoku99((x, y) =>
        {
            retValue[x, y].Num = Arr[x, y];

            return null;
        });

        return retValue;
    }



    private static int[,] BoardToArr(Board board)
    {
        int[,] retValue = new int[9, 9];

        Tool.DoInSudoku99((x, y) =>
        {
            retValue[x, y] = board[x, y].Num;

            return null;
        });

        return retValue;
    }


    private static bool IsFullNodeArr(Node[,] nodeArr)
    {
        foreach (var piece in nodeArr)
        {
            if (piece.Num == 0)
            {
                return false;
            }
        }
        return true;
    }

    private static void SetNodeArrFromintArr(Node[,] nodeArr, int[,] sudokuArr)
    {
        Tool.DoInSudoku99(
        (x, y) =>
        {
        Node newNode = new Node();

        newNode.Num = sudokuArr[y, x];
        newNode.pos.x = x;
        newNode.pos.y = y;

            nodeArr[x, y] = newNode;
        return null;
        }
    );

    }
    private delegate void BXYDel(Node piece);
    private static void DoBXYLines(BXYDel Actor, int[,] Arr, Point pos)
    {
        foreach (var piece in Sudoku.GetBXYLine(ArrToBoard(Arr), pos.x, 'Y'))
        {
            Actor(piece);
        }
        foreach (var piece in Sudoku.GetBXYLine(ArrToBoard(Arr), pos.y, 'X'))
        {
            Actor(piece);
        }
        foreach (var piece in Sudoku.GetBXYLine(ArrToBoard(Arr), GetPointToBoxLine(pos), 'B'))
        {
            Actor(piece);
        }
    }


    private static IEnumerable<Node> GetBXYLine(Node[,] _allNode, int _line, char _BXY)
    {
        if (_BXY == 'X')
        {
            for (int x = 0; x < 9; x++)
            {
                yield return _allNode[x, _line];
            }
        }
        else if (_BXY == 'Y')
        {
            for (int y = 0; y < 9; y++)
            {
                yield return _allNode[_line, y];
            }
        }
        else if (_BXY == 'B')
        {
            int startX = 0;
            int startY = 0;

            switch (_line)
            {
                case 0:
                    startX = 0 * 3;
                    startY = 0 * 3;
                    break;
                case 1:
                    startX = 1 * 3;
                    startY = 0 * 3;
                    break;
                case 2:
                    startX = 2 * 3;
                    startY = 0 * 3;
                    break;
                case 3:
                    startX = 0 * 3;
                    startY = 1 * 3;
                    break;
                case 4:
                    startX = 1 * 3;
                    startY = 1 * 3;
                    break;
                case 5:
                    startX = 2 * 3;
                    startY = 1 * 3;
                    break;
                case 6:
                    startX = 0 * 3;
                    startY = 2 * 3;
                    break;
                case 7:
                    startX = 1 * 3;
                    startY = 2 * 3;
                    break;
                case 8:
                    startX = 2 * 3;
                    startY = 2 * 3;
                    break;
                default:
                    break;
            }


            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    yield return _allNode[startX + x, startY + y];
                }
            }

        }
    }
    private static IEnumerable<int> GetPossibleNums(Node[,] _allNode, Point _point)
    {
        bool[] addArr = new bool[9];
        foreach (Node node in GetBXYLine(_allNode, _point.y, 'X'))
        {
            if (node.Num != 0)
                addArr[node.Num - 1] = true;
        }
        foreach (Node node in GetBXYLine(_allNode, _point.x, 'Y'))
        {
            if (node.Num != 0)
                addArr[node.Num - 1] = true;
        }
        foreach (Node node in GetBXYLine(_allNode, GetPointToBoxLine(_point), 'B'))
        {
            if (node.Num != 0)
                addArr[node.Num - 1] = true;
        }

        for (int i = 0; i < 9; i++)
        {
            if (addArr[i] == false)
                yield return i + 1;
        }
    }
    private static void RemoveLinePossiableNums(Node[,] allNode, Point pos, int num)
    {
        {
            foreach (var piece in GetBXYLine(allNode, GetPointToBoxLine(pos), 'B'))
            {
                for (int i = 0; i < piece.possibleNums.Count; i++)
                {
                    if (piece.possibleNums[i] == num)
                    {
                        piece.possibleNums.RemoveAt(i);
                        i--;
                    }
                }
            }
            foreach (var piece in GetBXYLine(allNode, pos.y, 'X'))
            {
                for (int i = 0; i < piece.possibleNums.Count; i++)
                {
                    if (piece.possibleNums[i] == num)
                    {
                        piece.possibleNums.RemoveAt(i);
                        i--;
                    }
                }
            }
            foreach (var piece in GetBXYLine(allNode, pos.x, 'Y'))
            {
                for (int i = 0; i < piece.possibleNums.Count; i++)
                {
                    if (piece.possibleNums[i] == num)
                    {
                        piece.possibleNums.RemoveAt(i);
                        i--;
                    }
                }
            }

            //return null;
        };
    }

    private static int GetPointToBoxLine(Point _point)
    {
        int line = 0;
        line += _point.x / 3;
        line += (_point.y / 3) * 3;
        return line;
    }

    private static void SetPossibleNums(Board board)
    {
        foreach (var piece in board.nodeBoard)
        {
            foreach (var num in GetPossibleNums(board, piece.pos))
            {
                piece.possibleNums.Add(num);
            }
        }
    }

    private static Node GetEmptyNodeInArr(Node[,] nodeArr, int selCount)
    {
        int count = 0;
        foreach (var piece in nodeArr)
        {
            if (piece.Num == 0)
            {
                if (count >= selCount)
                {
                    return piece;
                }
                count++;
            }
        }
        return null;
    }

    private static int GetBoardNodeCount(Board board, char Fill_Empty)
    {
        int retValue = 0;
        foreach(var piece in board.nodeBoard)
        {
            if(piece.Num == 0)
            {
                retValue++;
            }
        }

        if(Fill_Empty == 'F')
        {
            return 9 * 9 - retValue;
        }
        else if (Fill_Empty == 'E')
        {
            return retValue;
        }
        return -1;
    }


    private static bool ErrorCheck(Node[,] _allNode)
    {
        if (LineErrorChecker(_allNode, 'B') || LineErrorChecker(_allNode, 'X') || LineErrorChecker(_allNode, 'Y') || PossibleErrorChekcer(_allNode))
            return true;
        else
            return false;
    }

    private static bool LineErrorAllCheck(Node[,] _allNode)
    {
        if (LineErrorChecker(_allNode, 'B') || LineErrorChecker(_allNode, 'X') || LineErrorChecker(_allNode, 'Y'))
            return true;
        else
            return false;
    }


    private static bool PossibleErrorChekcer(Node[,] _allNode)
    {
        foreach (var piece in _allNode)
        {
            if (piece.Num == 0)
            {
                if (piece.possibleNums.Count == 0)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private static bool LineErrorChecker(Node[,] _allNode, char BXY)
    {
        //if (BXY == 'X') BXY = 'Y';
        //else if (BXY == 'Y') BXY = 'X';

        for (int line = 0; line < 9; line++)
        {
            bool[] checker = new bool[9];
            foreach (Node node in GetBXYLine(_allNode, line, BXY))
            {
                if (node.Num != 0)
                {
                    if (checker[node.Num - 1] == false)
                        checker[node.Num - 1] = true;
                    else if (checker[node.Num - 1] == true)
                        return true;
                }
            }

        }
        return false;
    }

    private static int[,] RevorseArrXY(int[,] arr)
    {
        int[,] tempArr = new int[9, 9];

        Tool.DoInSudoku99((x, y) =>
        {
            tempArr[x, y] = arr[y, x];
            return null;
        });

        return tempArr;
    }

}