using System;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;
using System.Collections.Generic;

namespace CheckerBoard
{
    public partial class CBoard : Form
    {
        const int PANELTILESIZE = 40;
        const int GRIDSIZE = 8;
        const int NUMPIECES = 46;
        const int UNIQUEPIECES = 13;
        const int UNOCCUPIEDSPOT = -3;
        const int UNOCCIPEDTILE = -2;
        const int BOARDSIZE = 63;
        const int TILESIZE = 7;

        const string filePath = "c:\\temp\\myworkbook.xlsx";
        const string piecesPath = "c:\\temp\\pieces.xlsx";

        private static readonly Color[] aColors = { Color.Blue, Color.BlueViolet, Color.Brown, Color.Coral, Color.Green, Color.Orange, Color.Teal, Color.DeepPink, Color.Crimson, Color.DarkTurquoise, Color.OrangeRed, Color.Yellow, Color.DarkKhaki };
        private static readonly Color[] bColors = { Color.DodgerBlue, Color.Orchid, Color.Chocolate, Color.LightSalmon, Color.LawnGreen, Color.SaddleBrown, Color.MediumTurquoise, Color.Magenta, Color.DarkRed, Color.Aqua, Color.Tomato, Color.LightYellow, Color.DarkSeaGreen };

        // class member array of Panels to track chessboard tiles
        private Panel[] _chessBoardPanels;

        public int current = 1;

        public CBoard()
        {
            InitializeComponent();
            Load += new EventHandler(Form_Load);
        }

        // event handler of Form Load... init things here
        private void Form_Load(object sender, EventArgs e)
        {
            _chessBoardPanels = new Panel[GRIDSIZE * GRIDSIZE];
            setupBoard();
            btnNext.Enabled = false;
            btnPrev.Enabled = false;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {

            clearBoard();
            lblDebug.Text = string.Empty;
            lblError.Text = string.Empty;
            lblFinal.Text = string.Empty;
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            btnGo.Enabled = false;
            run();
            btnGo.Enabled = true;
            btnNext.Enabled = true;
            btnPrev.Enabled = true;
        }

        private void btnPlaceOne_Click(object sender, EventArgs e)
        {
            var rnd = new Random();
            var item = Convert.ToInt16(ddlItems.SelectedIndex);
            var pos = Convert.ToInt32(ddlPos.SelectedIndex);

            var board = new int[BOARDSIZE + 1];  //NO GOOD
            var bwboard = new int[BOARDSIZE + 1];
            var piecesPlacedLocal = 0;
            var piecesAttemptedLocal=0;
            if (item >= NUMPIECES)
            {
                item = Convert.ToInt16(rnd.Next(1, NUMPIECES));
                lblDebug.Text = "Random piece nubmer selected is: " + item.ToString() + ". ";
            }

            if (pos >= 64)
            {
                pos = Convert.ToInt16(rnd.Next(0, 64));
                lblDebug.Text += "Random start nubmer selected is: " + pos.ToString();
            }

            Pieces[] _pieces = SqliteDataAccess.getPieces();

            if (pos - _pieces[item].Orientation[0] > 0)
                pos = pos - _pieces[item].Orientation[0];


            if (putPiece(0, 0, ref _pieces, ref board, ref bwboard, ref piecesAttemptedLocal))
            {
                playPiece(_pieces[0], 0, true, ref board, ref bwboard, ref piecesPlacedLocal);
                lblError.Text = string.Empty;
            }
            else
                lblError.Text = "Piece doesn't fit";
            if (putPiece(21, 5, ref _pieces, ref board, ref bwboard, ref piecesAttemptedLocal))
                playPiece(_pieces[21], 5, true, ref board, ref bwboard, ref piecesPlacedLocal);
            else
                lblError.Text = "Piece doesn't fit";

            if (putPiece(4, 10, ref _pieces, ref board, ref bwboard, ref piecesAttemptedLocal))
                playPiece(_pieces[4], 10, true, ref board, ref bwboard, ref piecesPlacedLocal);
            else
                lblError.Text = "Piece doesn't fit";

            if (putPiece(33, 16, ref _pieces, ref board, ref bwboard, ref piecesAttemptedLocal))
                playPiece(_pieces[33], 16, true, ref board, ref bwboard, ref piecesPlacedLocal);
            else
                lblError.Text = "Piece doesn't fit";

            if (putPiece(45, 19, ref _pieces, ref board, ref bwboard, ref piecesAttemptedLocal))
                playPiece(_pieces[45], 19, true, ref board, ref bwboard, ref piecesPlacedLocal);
            else
                lblError.Text = "Piece doesn't fit";

            if (putPiece(28, 20, ref _pieces, ref board, ref bwboard, ref piecesAttemptedLocal))
                playPiece(_pieces[28], 20, true, ref board, ref bwboard, ref piecesPlacedLocal);
            else
                lblError.Text = "Piece doesn't fit";

            if (putPiece(38, 29, ref _pieces, ref board, ref bwboard, ref piecesAttemptedLocal))
                playPiece(_pieces[38], 29, true, ref board, ref bwboard, ref piecesPlacedLocal);
            else
                lblError.Text = "Piece doesn't fit";

            if (putPiece(43, 37, ref _pieces, ref board, ref bwboard, ref piecesAttemptedLocal))
                playPiece(_pieces[43], 37, true, ref board, ref bwboard, ref piecesPlacedLocal);
            else
                lblError.Text = "Piece doesn't fit";

            if (putPiece(27, 32, ref _pieces, ref board, ref bwboard, ref piecesAttemptedLocal))
                playPiece(_pieces[27], 32, true, ref board, ref bwboard, ref piecesPlacedLocal);
            else
                lblError.Text = "Piece doesn't fit";

            if (putPiece(12, 43, ref _pieces, ref board, ref bwboard, ref piecesAttemptedLocal))
                playPiece(_pieces[12], 43, true, ref board, ref bwboard, ref piecesPlacedLocal);
            else
                lblError.Text = "Piece doesn't fit";

            if (putPiece(8, 51, ref _pieces, ref board, ref bwboard, ref piecesAttemptedLocal))
                playPiece(_pieces[8], 51, true, ref board, ref bwboard, ref piecesPlacedLocal);
            else
                lblError.Text = "Piece doesn't fit";

            if (putPiece(19, 26, ref _pieces, ref board, ref bwboard, ref piecesAttemptedLocal))
                playPiece(_pieces[19], 26, true, ref board, ref bwboard, ref piecesPlacedLocal);
            else
                lblError.Text = "Piece doesn't fit";

            //lblError.Text = string.Empty;
        }

        private void btnAll_Click(object sender, EventArgs e)
        {
            var pos = Convert.ToInt16(ddlPos.SelectedIndex);
            runDebugPieces(pos);
        }

        private void btnSuper_Click(object sender, EventArgs e)
        {
            // var foo = "1,1,1,1,1,2,5,5,1,1,12,12,12,2,5,5,3,3,3,3,3,2,5,-3,4,-3,-3,-3,3,2,5,-3,4,-3,-3,-3,-3,2,2,-3,4,-3,-3,-3,-3,-3,-3,4,4,-3,-3,-3,-3,-3,-3,4,-3,-3,-3,-3,-3,-3,-3,-3";
            // var bar = "0,1,0,1,0,1,0,1,1,0,1,0,1,0,1,0,0,1,0,1,0,1,0,-3,1,-3,-3,-3,1,0,1,-3,0,-3,-3,-3,-3,1,0,-3,1,-3,-3,-3,-3,-3,-3,1,0,-3,-3,-3,-3,-3,-3,0,-3,-3,-3,-3,-3,-3,-3,-3";
            var board = txtBoard.Text.Split(',').Select(Int32.Parse).ToList();
            var bwboard = txtBW.Text.Split(',').Select(Int32.Parse).ToList();
            try
            {
                putNew(board, bwboard);
            }
            catch (Exception ex)
            {
                lblError.Text = "Super " + ex.Message;
            }
        }

        private void btnAnother_Click(object sender, EventArgs e)
        {
            var rnd = new Random();

            var item = Convert.ToInt16(ddlItems.SelectedIndex);
            var pos = Convert.ToInt32(ddlPos.SelectedIndex);

            var rawBoard = SqliteDataAccess.getBoard(999);
            var rawBwboard = SqliteDataAccess.getBoard(-999);

            var board = rawBoard.Split(',').Select(Int32.Parse).ToArray();
            var bwboard = rawBwboard.Split(',').Select(Int32.Parse).ToArray();

            var piecesPlacedLocal = 0;
            var piecesAttemptedLocal = 0;

            Pieces[] _pieces = SqliteDataAccess.getPieces();

            if (item >= NUMPIECES)
            {
                item = Convert.ToInt16(rnd.Next(1, NUMPIECES));
                lblDebug.Text = "Random piece nubmer selected is: " + item.ToString() + ". ";
            }

            if (pos >= 64)
            {
                pos = Convert.ToInt16(rnd.Next(0, 64));
                lblDebug.Text += "Random start nubmer selected is: " + pos.ToString();
            }

            if (pos - _pieces[item].Orientation[0] > 0)
                pos = pos - _pieces[item].Orientation[0];


            if (putPiece(item, pos, ref _pieces, ref board, ref bwboard, ref piecesAttemptedLocal))
            {
                playPiece(_pieces[item], pos, true, ref board, ref bwboard, ref piecesPlacedLocal);
                lblError.Text = string.Empty;
            }
            else
                lblError.Text = "Piece doesn't fit";
        }

        private void btnShowPieces_Click(object sender, EventArgs e)
        {

            var listOfBoards = new List<int[]>();
            Pieces[] _pieces = SqliteDataAccess.getPieces();
            var rawBoard = SqliteDataAccess.getBoard(999);
            var rawBwboard = SqliteDataAccess.getBoard(-999);

            var board = rawBoard.Split(',').Select(Int32.Parse).ToArray();
            var bwboard = rawBwboard.Split(',').Select(Int32.Parse).ToArray();

            var piecesPlacedLocal = 0;

            for (var i = 0; i < NUMPIECES; i++)
            {
                clearBoard();
                playPiece(_pieces[i], 0, true, ref board, ref bwboard, ref piecesPlacedLocal);
                int[] boardCopy = (int[])board.Clone();
                listOfBoards.Add(boardCopy);
            }

          //  writeResultsFile(listOfBoards, NUMPIECES, piecesPath);
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {

            if (current > 1)
            {
                var currentBoard = SqliteDataAccess.getBoard(current);
                var currentBWBoard = SqliteDataAccess.getBoard(-current);

                var board = currentBoard.Split(',').Select(Int32.Parse).ToList();
                var bwboard = currentBWBoard.Split(',').Select(Int32.Parse).ToList();
                try
                {
                    putNew(board, bwboard);
                }
                catch (Exception ex)
                {
                    lblError.Text = "Prev: " + ex.Message;
                }
                current--;
                lblNum.Text = String.Format($"{current}/66");
            }
            else
                lblDebug.Text = "At the beginning.";
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (current < 66)
            {
                var currentBoard = SqliteDataAccess.getBoard(current);
                var currentBWBoard = SqliteDataAccess.getBoard(-current);

                var board = currentBoard.Split(',').Select(Int32.Parse).ToList();
                var bwboard = currentBWBoard.Split(',').Select(Int32.Parse).ToList();

                try
                {
                    putNew(board, bwboard);
                }
                catch (Exception ex)
                {
                    lblError.Text = "Next: " + ex.Message;
                }
                current++;
                lblNum.Text = String.Format($"{current}/66");
            }
            else
                lblDebug.Text = "No more solutions.";
        }
    }
}
