using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Threading;
using System.Linq;

namespace CheckerBoard
{
    public partial class CBoard : Form
    {
        //Checks to see if a piece fits at a given square
        private bool putPiece(int p, int square, ref Pieces[] _pieces, ref int[] board, ref int[] bwboard, ref int piecesAttempted)
        {  // try to place a piece on the board,
           // return true if it fits

            var lastSquares = new List<int> { 7, 15, 23, 31, 39, 47, 55, 63 };

            try
            {
                if (board[square] != UNOCCUPIEDSPOT)  //-3 means unoccupied spot on board
                    return false;

                piecesAttempted++;

                int offset = _pieces[p].Orientation[0];
                for (var i = 0; i < TILESIZE; i++)
                {
                    if (_pieces[p].Orientation[i] == UNOCCIPEDTILE)  // unoccupied tile
                        break;

                    if (square + _pieces[p].Orientation[i] - offset > BOARDSIZE)  // may need something here
                        return false;

                    if (i == 0)
                        continue;  // if not at the last tile assume first time will fit

                    if (board[square + _pieces[p].Orientation[i] - offset] != UNOCCUPIEDSPOT) // one of the squares needed is already occupied
                        return false;
                }

                //Check for horizontal overflow
                for (var i = 0; i < TILESIZE; i++)
                {
                    if (_pieces[p].Orientation[i] == UNOCCIPEDTILE)  //unoccupied tile
                        break;

                    var previousSquare = 0;
                    if (i > 0)
                        previousSquare = square + _pieces[p].Orientation[i - 1] - offset;

                    var currentSquare = square + _pieces[p].Orientation[i] - offset;
                    var nextSquare = UNOCCIPEDTILE;

                    if (i < TILESIZE - 1)
                        nextSquare = square + _pieces[p].Orientation[i + 1] - offset;

                    //Check for horizontal overflow
                    if (lastSquares.Contains(currentSquare))
                        if (nextSquare == currentSquare + 1)
                            return false;

                    //These pertain to the board
                    var beforeTile = currentSquare - 1;
                    var nextTile = currentSquare + 1;
                    var aboveTile = currentSquare - GRIDSIZE;
                    var belowTile = currentSquare + GRIDSIZE;

                    if (beforeTile > 0)
                        if (bwboard[beforeTile] == _pieces[p].BorW[i])
                            if (!(lastSquares.Contains(beforeTile)))
                                return false;
                    if (nextTile < BOARDSIZE + 1)
                        if (bwboard[nextTile] == _pieces[p].BorW[i])
                            if (!(lastSquares.Contains(currentSquare)))
                                return false;
                    if (aboveTile > 0)
                        if (bwboard[aboveTile] == _pieces[p].BorW[i])
                            return false;
                    if (belowTile < BOARDSIZE + 1)
                        if (bwboard[belowTile] == _pieces[p].BorW[i])
                            return false;
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }

            return true;
        }

        //If determined that a piece fits then add it to the board
        private void playPiece(Pieces pieceData, int startSquare, bool updateUX, ref int[] board, ref int[] bwboard, ref int piecesPlaced)
        {

            piecesPlaced++;
            int offset = pieceData.Orientation[0];
            try
            {
                for (var x = 0; x < TILESIZE; x++)
                {
                    if (pieceData.Orientation[x] == UNOCCIPEDTILE)  //unoccupied tile
                        break;

                    if (updateUX)
                        putSquare(pieceData.Name, startSquare + pieceData.Orientation[x], pieceData.BorW[x]);

                    board[startSquare + pieceData.Orientation[x] - offset] = pieceData.Name;
                    bwboard[startSquare + pieceData.Orientation[x] - offset] = pieceData.BorW[x];
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        //Remove piece from the board
        private void removePiece(Pieces pieceData, int startSquare, bool updateUX, ref int[] board, ref int[] bwboard, ref int piecesRemoved)
        {
            piecesRemoved++;
            try
            {
                int offset = pieceData.Orientation[0];

                for (int i = 0; i < TILESIZE; i++)
                {
                    if (pieceData.Orientation[i] == UNOCCIPEDTILE)  //unoccupied tile 
                        break;

                    board[startSquare + pieceData.Orientation[i] - offset] = UNOCCUPIEDSPOT;
                    bwboard[startSquare + pieceData.Orientation[i] - offset] = UNOCCUPIEDSPOT;
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        //Adds completed puzzles to a list to be written to storage
        private void AddBoardsToList(ref int[] board, ref int[] bwboard, ref int count, ref List<Results> listOfResults)
        {

            int[] boardCopy = (int[])board.Clone();
            int[] BWboardCopy = (int[])bwboard.Clone();

            var result = new Results(boardCopy, BWboardCopy, count);
            listOfResults.Add(result);
        }

        //Recursive funciton that tries every piece in every possible orientation to see if it fits.
        private void playNoUX(int square, ref Pieces[] _pieces, ref bool[] used, ref int[] board, ref int[] bwboard, ref int numused, ref int piecesPlaced, ref int piecesAttempted, ref int piecesRemoved, ref int count, ref List<Results> listOfResults)
        {
            int nextSquare;
            try
            {
                for (var p = 0; p < NUMPIECES; p++)
                    if ((used[_pieces[p].Name] == false) && putPiece(p, square, ref _pieces, ref board, ref bwboard, ref piecesAttempted))
                    {
                        playPiece(_pieces[p], square, false, ref board, ref bwboard, ref piecesPlaced);
                        used[_pieces[p].Name] = true;
                        numused++;

                        if (numused == UNIQUEPIECES - 1) // solution found
                        {
                            count++;
                            AddBoardsToList(ref board, ref bwboard, ref count, ref listOfResults);
                        }
                        else
                        {
                            nextSquare = square;
                            while (board[nextSquare] != UNOCCUPIEDSPOT)  // find next empty square
                                nextSquare++;

                            playNoUX(nextSquare, ref _pieces, ref used, ref board, ref bwboard, ref numused, ref piecesPlaced, ref piecesAttempted, ref piecesRemoved, ref count, ref listOfResults);
                        }

                        removePiece(_pieces[p], square, false, ref board, ref bwboard, ref piecesRemoved);  // backtrack
                        numused--;
                        used[_pieces[p].Name] = false;
                    }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        //Initialize variables
        private void initVariables(ref bool[] used, ref int[] board, ref int[] bwboard)
        {
            for (var i = 1; i < UNIQUEPIECES; i++)
                used[i] = false;

            for (var z = 0; z < BOARDSIZE + 1; z++)
            {
                board[z] = UNOCCUPIEDSPOT;
                bwboard[z] = UNOCCUPIEDSPOT;
            }

            SqliteDataAccess.SaveBoard(board, 999);
            SqliteDataAccess.SaveBoard(bwboard, -999);
        }

        //Main execution engine
        private void run()
        {
            var used = new bool[UNIQUEPIECES];
            var board = new int[BOARDSIZE + 1];
            var bwboard = new int[BOARDSIZE + 1];
            var numused = 0;
            var piecesPlaced = 0;
            var piecesAttempted = 0;
            var piecesRemoved = 0;
            var count = 0;
            var listOfResults = new List<Results>();

            try
            {
                clearBoard();
                SqliteDataAccess.clearDB();
                initVariables(ref used, ref board, ref bwboard);
                sendMessageFinal("Execution begins. ");

                var _pieces = SqliteDataAccess.getPieces();
                playNoUX(0, ref _pieces, ref used, ref board, ref bwboard, ref numused, ref piecesPlaced, ref piecesAttempted, ref piecesRemoved, ref count, ref listOfResults);
                sendMessageDebug(String.Format($"Pieces Placed {piecesPlaced}, Pieces Removed {piecesRemoved}, Pieces Attempted {piecesAttempted}, Unique Solutions: {count}"));

                writeResultsFile(listOfResults, count, filePath);
                sendMessageFinal(lblFinal.Text + String.Format($"Results saved to: {filePath}. Execution complete."));

                SqliteDataAccess.SaveBoard(listOfResults);

                putNew(listOfResults[0].Board.ToList(), listOfResults[0].BWBoard.ToList());
                lblNum.Text = String.Format($"1/{count}");
                btnGo.Enabled = false;
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        //This function paints a piece on the checker board
        private void putSquare(int name, int square, int BorW)
        {
            Task.Factory.StartNew(() =>
            {
                this.Invoke((MethodInvoker)delegate
                {
                    try
                    {

                        if (BorW == 1)
                            _chessBoardPanels[square].BackColor = aColors[name];  //name-1
                        else
                            _chessBoardPanels[square].BackColor = bColors[name];

                        var l = new Label();
                        l.Text = name.ToString();
                        _chessBoardPanels[square].Controls.Add(l);

                        _chessBoardPanels[square].Refresh();
                        Thread.Sleep(10);
                    }
                    catch (Exception ex)
                    {
                        lblError.Text = ex.Message;
                    }
                });
            });
        }

    }
}