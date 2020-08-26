using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckerBoard
{
    public class Pentominos
    {

        int[] board;          // The 8-by-8 board is actually represented
                              // conceptually as a 10-by-10 data structure
                              // in which the cells along the border
                              // are declared permanently "filled"
                              // This simplifies testing whether a given
                              // piece fits at a given position on the 
                              // board.  Furthermore, this 10-by-10 board
                              // is represented by a 1-dimensional array
                              // in which the 10*i+j-th entry represents
                              // row j and column i on the board.

        bool[] used;           //  used[i] tells whether piece # i is already on the board

        int numused;           // number of pieces currently on the board, from 0 to 12

        int[][] pieces = {
                { 1, 1,2,3,4 },         // This array represents everything the program
                { 1, 10,20,30,40 },     // knows about the individual pentominos.  Each
                { 2, 9,10,11,20 },      // row in the array represents a particular
                { 3, 1,10,19,20 },      // pentomino in a particular orientation.  Different
                { 3, 10,11,12,22 },     // orientations are obtained by rotating or flipping
                { 3, 1,11,21,22 },      // the pentomino over.  Note that the program must
                { 3, 8,9,10,18 },       // try each pentomino in each possible orientation,
                { 4, 10,20,21,22 },     // but must be careful not to reuse a piece if
                { 4, 1,2,10,20 },       // it has already been used on the board in a
                { 4, 10,18,19,20 },     // different orientation.
                { 4, 1,2,12,22 },       // The pentominoes are numbered from 1 to 12.
                { 5, 1,2,11,21 },       // The first number on each row here tells which pentomino
                { 5, 8,9,10,20 },       // that line represents.  Note that there can be
                { 5, 10,19,20,21 },     // up to 8 different rows for each pentomino.
                { 5, 10,11,12,20 },     // some pentominos have fewer rows because they are
                { 6, 10,11,21,22 },     // symmetric.  For example, the pentomino that looks
                { 6, 9,10,18,19 },      // like:
                { 6, 1,11,12,22 },      //           GGG
                { 6, 1,9,10,19 },       //           G G
                { 7, 1,2,10,12 },       //
                { 7, 1,11,20,21 },      // can be rotated into three additional positions,
                { 7, 2,10,11,12 },      // but flipping it over will give nothing new.
                { 7, 1,10,20,21 },      // So, it has only 4 rows in the array.
                { 8, 10,11,12,13 },     //     The four remaining entries in the array
                { 8, 10,20,29,30 },     // describe the given piece in the given orientation,
                { 8, 1,2,3,13 },        // in a way convenient for placing the piece into
                { 8, 1,10,20,30 },      // the one-dimensional array that represents the
                { 8, 1,11,21,31 },      // board.  As an example, consider the row
                { 8, 1,2,3,10 },        //
                { 8, 10,20,30,31 },     //           { 7, 1,2,10,19 }
                { 8, 7,8,9,10 },        //
                { 9, 1,8,9,10 },        // If this piece is placed on the board so that
                { 9, 10,11,21,31 },     // its topmost/leftmost square fills position
                { 9, 1,2,9,10 },        // p in the array, then the other four squares
                { 9, 10,20,21,31 },     // will be at positions  p+1, p+2, p+10, and p+19.
                { 9, 1,11,12,13 },      // To see whether the piece can be played at that
                { 9, 10,19,20,29 },     // position, it suffices to check whether any of
                { 9, 1,2,12,13 },       // these five squares are filled. 
                { 9, 9,10,19,29 },      //     On the board, each piece will be shown
                { 10, 8,9,10,11 },      // in its own color.
                { 10, 9,10,20,30 },
                { 10, 1,2,3,11 },
                { 10, 10,20,21,30 },
                { 10, 1,2,3,12 },
                { 10, 10,11,20,30 },
                { 10, 9,10,11,12 },
                { 10, 10,19,20,30 },
                { 11, 9,10,11,21 },
                { 11, 1,9,10,20 },
                { 11, 10,11,12,21 },
                { 11, 10,11,19,20 },
                { 11, 8,9,10,19},
                { 11, 1,11,12,21 },
                { 11, 9,10,11,19 },
                { 11, 9,10,20,21 },
                { 12, 1,10,11,21 },
                { 12, 1,2,10,11 },
                { 12, 10,11,20,21 },
                { 12, 1,9,10,11 },
                { 12, 1,10,11,12 },
                { 12, 9,10,19,20 },
                { 12, 1,2,11,12 },
                { 12, 1,10,11,20 },
                };

        bool aborted;  // used in play() to test whether the puzzle is solved (or aborted)

        void play(int square)
        {   // recursive procedure that tries to solve the puzzle
            // parameter "square" is the number of the next empty
            // to be filled
            for (int p = 0; p < 63; p++)
                if (!aborted && (used[pieces[p][0]] == false) && putPiece(p, square))
                {  // try piece p
                   // a piece has been placed on the board.
                    used[pieces[p][0]] = true;
                    numused++;
                    boolean stepping = false;
                    int message = getMessage();
                    if (message > 0)
                    {
                        if (message == pauseMessage || message == stepMessage)
                        {
                            stepping = true;
                            setMessage(0);
                        }
                        else if (message == clearMessage || message == randomMessage)
                        {
                            aborted = true;
                            return;  // note: don't setMessage(0), since run() has to handle message
                        }
                        else  // go message
                            setMessage(0);
                    }
                    if (numused == 12)
                    {  // puzzle is solved
                        synchronized(this) {
                            goButton.enable();
                            stepButton.enable();
                            pauseButton.disable();
                            comment.setText("Solution found:");
                        }
                        doDelay(-1);  // wait indefinitely for user command
                        comment.setText("Solving...");
                    }
                    else
                    {
                        if (stepping)    // pause after placing a piece
                            doDelay(-1);
                        else
                            doDelay(delay);
                        int nextSquare = square;
                        while (board[nextSquare] != 0)  // find next empty square
                            nextSquare++;
                        play(nextSquare);  // and try to complete the solution
                        if (aborted)
                            return;
                    }
                    boardcanvas.removePiece(pieces[p], square);  // backtrack
                    numused--;
                    used[pieces[p][0]] = false;
                }
        }

        bool putPiece(int p, int square)
        {  // try to place a piece on the board,
           // return true if it fits
            if (board[square] != 0)
                return false;
            for (int i = 1; i <= 4; i++)
                if (board[square + pieces[p][i]] != 0)  // one of the squares needed is already occupied
                    return false;
            boardcanvas.playPiece(pieces[p], square);  // color in the squares to represent the piece
            return true;
        }

        void clearBoard()
        {
            for (int i = 0; i < 100; i++) // fill in the border with -1's
                board[i] = -1;
            for (int i = 1; i < 9; i++)   // fill in the rest of the board with empty spaces (0's)
                for (int j = 1; j < 9; j++)
                    board[j * 10 + i] = 0;
            boardcanvas.repaint();
        }



        void setUpRandomBoard()
        {
            for (int i = 0; i < 100; i++) // fill in the border with -1's
                board[i] = -1;
            for (int i = 1; i < 9; i++)   // fill in the rest of the board with empty spaces (0's)
                for (int j = 1; j < 9; j++)
                    board[j * 10 + i] = 0;
            int x, y;
            switch ((int)(5 * Math.random()))
            {
                case 0:
                    for (int i = 0; i < 4; i++)
                    {
                        do
                        {
                            x = 1 + (int)(8 * Math.random());
                            y = 1 + (int)(8 * Math.random());
                        } while (board[y * 10 + x] == -1);
                        board[y * 10 + x] = -1;
                    }
                    break;
                case 1:
                case 2:
                    do
                    {
                        x = 1 + (int)(8 * Math.random());
                        y = 1 + (int)(8 * Math.random());
                    } while (y == 5 || x == 5);
                    board[10 * y + x] = -1;
                    board[10 * y + (9 - x)] = -1;
                    board[10 * (9 - y) + x] = -1;
                    board[10 * (9 - y) + (9 - x)] = -1;
                    break;
                default:
                    x = (int)(6 * Math.random()) + 1;
                    y = (int)((x) * Math.random()) + 1;
                    board[y * 10 + x] = -1;
                    board[y * 10 + x + 1] = -1;
                    board[y * 10 + x + 10] = -1;
                    board[y * 10 + x + 11] = -1;
                    break;
            }
            boardcanvas.repaint();
        }

    }
}
