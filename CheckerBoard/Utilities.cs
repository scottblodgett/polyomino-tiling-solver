using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Linq;

namespace CheckerBoard
{
    public partial class CBoard : Form
    {

        delegate void SetTextCallback(string text);

        private void setupBoard()
        {
            var clr1 = Color.DarkGray;
            var clr2 = Color.White;

            // double for loop to handle all rows and columns
            for (var n = 0; n < GRIDSIZE; n++)
                for (var m = 0; m < GRIDSIZE; m++)
                {
                    // create new Panel control which will be one 
                    // chess board tile
                    var newPanel = new Panel
                    {
                        Size = new Size(PANELTILESIZE, PANELTILESIZE),
                        Location = new Point(PANELTILESIZE * n + 25, PANELTILESIZE * m + 35)
                    };

                    // add to Form's Controls so that they show up
                    Controls.Add(newPanel);
                    var l = new Label();
                    l.Text = (GRIDSIZE * m + n).ToString();
                    newPanel.Controls.Add(l);

                    // add to our 2d array of panels for future use
                    _chessBoardPanels[8 * m + n] = newPanel;

                    // color the backgrounds
                    if (n % 2 == 0)
                        newPanel.BackColor = m % 2 != 0 ? clr1 : clr2;
                    else
                        newPanel.BackColor = m % 2 != 0 ? clr2 : clr1;
                }

            for (var i = 1; i <= NUMPIECES; i++)
                ddlItems.Items.Add(i.ToString());
            ddlItems.Items.Add("Rnd");

            ddlItems.SelectedIndex = 0;

            for (var i = 0; i <= 63; i++) //should be NUMPIECES 
                ddlPos.Items.Add(i.ToString());
            ddlPos.Items.Add("Rnd");

            ddlPos.SelectedIndex = 0;
        }

        private void clearBoard()
        {
            var clr1 = Color.DarkGray;
            var clr2 = Color.White;

            // double for loop to handle all rows and columns
            for (var n = 0; n < GRIDSIZE; n++)
                for (var m = 0; m < GRIDSIZE; m++)
                    // color the backgrounds
                    if (n % 2 == 0)
                        _chessBoardPanels[n * GRIDSIZE + m].BackColor = m % 2 != 0 ? clr1 : clr2;
                    else
                        _chessBoardPanels[n * GRIDSIZE + m].BackColor = m % 2 != 0 ? clr2 : clr1;
        }

        private void sendMessageDebug(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.lblDebug.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(sendMessageDebug);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.lblDebug.Text = text;
            }

        }

        private void sendMessageFinal(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.lblFinal.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(sendMessageFinal);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.lblFinal.Text = text;
            }
        }

        private void writeResultsFile(List<Results> list, int count, string filePath)
        {
            //Creates a blank workbook. Use the using statment, so the package is disposed when we are done.

            try
            {
                //create a fileinfo object of an excel file on the disk
                var file = new FileInfo(filePath);

                using (var excelPackage = new ExcelPackage())
                {
                    var ws = excelPackage.Workbook.Worksheets.Add("Squares");

                    int offset;
                    for (var i = 0; i < count; i++)
                    {
                        offset = (8 * i); // is is sheetnubmer
                        var currentBoard = list[i];

                        //A workbook must have at least on cell, so lets add one... 
                        //var ws = excelPackage.Workbook.Worksheets.Add(sheetName);
                        //To set values in the spreadsheet use the Cells indexer.
                        //create an instance of the the first sheet in the loaded file

                        for (var yy = 0; yy < GRIDSIZE; yy++)
                        {
                            for (var xx = 0; xx < GRIDSIZE; xx++)
                            {
                                ws.Cells[yy + 1 + offset + i, xx + 1].Value = currentBoard.Board[yy * GRIDSIZE + xx];
                                ws.Cells[yy + 1 + offset + i, xx + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                ws.Cells[yy + 1 + offset + i, xx + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                var colorIndex = currentBoard.Board[yy * GRIDSIZE + xx] - 1;
                                if (colorIndex == -4) //These are the unused squares
                                    colorIndex = 12;
                                //if (currentBoard.BWBoard[yy * GRIDSIZE + xx] == 1) 
                                    ws.Cells[yy + 1 + offset + i, xx + 1].Style.Fill.BackgroundColor.SetColor(aColors[colorIndex]);
                                //else
                                   // ws.Cells[yy + 1 + offset + i, xx + 1].Style.Fill.BackgroundColor.SetColor(bColors[colorIndex]);

                            }
                            ws.Column(yy + 1).Width = 4;
                        }
                        ws.InsertRow(offset + 9 + i, 1);

                        offset++;
                    }
                    excelPackage.SaveAs(file);


                }
            }
            catch (Exception ex)
            {
                lblError.Text = "writeResultsFile: " + ex.Message;
            }
        }

        private async Task debugPieces(int startPos)
        {
            var rawBoard = SqliteDataAccess.getBoard(999);
            var rawBwboard = SqliteDataAccess.getBoard(-999);

            var board = rawBoard.Split(',').Select(Int32.Parse).ToArray();
            var bwboard = rawBwboard.Split(',').Select(Int32.Parse).ToArray();

            var piecesPlacedLocal = 0;
            var piecesAttemptedLocal = 0;
            try
            {
                var _pieces = SqliteDataAccess.getPieces();
                for (var i = 0; i < NUMPIECES; i++)
                {
                    clearBoard();
                    if (putPiece(i, 0, ref _pieces, ref board, ref bwboard, ref piecesAttemptedLocal))
                    {
                        playPiece(_pieces[i], startPos, true, ref board, ref bwboard, ref piecesPlacedLocal);
                        lblError.Text = string.Empty;
                        lblDebug.Text = "Piece nubmer: " + " " + i.ToString();
                    }
                    else
                        lblError.Text = "Piece " + i + " doesn't fit";
                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "DebugPieces: " + ex.Message;
            }
        }

        private async void runDebugPieces(int startPos)
        {

            var cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;

            try
            {
                await Task.Run(async () =>
                {
                    Task t = debugPieces(startPos);
                    await t;
                }, token);
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        //This debug funciton paints the board based on a list of pieces ordered by position. 
        private void putNew(List<int> numbers, List<int> bw)
        {
            Task.Factory.StartNew(() =>
            {
                this.Invoke((MethodInvoker)delegate
                {
                    foreach (Control c in this.Controls)
                    {
                        if (c is Panel)
                            c.Visible = false;
                    }

                    for (var n = 0; n < GRIDSIZE; n++)
                        for (var m = 0; m < GRIDSIZE; m++)
                        {
                            // create new Panel control which will be one 
                            // chess board tile
                            var newPanel = new Panel
                            {
                                Size = new Size(PANELTILESIZE, PANELTILESIZE),
                                Location = new Point(PANELTILESIZE * n + 25, PANELTILESIZE * m + 35)
                            };

                            var counter = GRIDSIZE * m + n;

                            // add to Form's Controls so that they show up
                            Controls.Add(newPanel);
                            var l = new Label();
                            l.Text = numbers[counter].ToString();
                            newPanel.Controls.Add(l);

                            var oldColor = _chessBoardPanels[counter].BackColor;

                            if (numbers[counter] != UNOCCUPIEDSPOT)
                            {
                                // color the backgrounds
                                if (bw[counter] == 0)
                                    newPanel.BackColor = aColors[numbers[counter]];
                                else
                                    newPanel.BackColor = bColors[numbers[counter]];
                            }
                            else
                                newPanel.BackColor = oldColor;

                            // add to our 2d array of panels for future use
                            _chessBoardPanels[counter] = newPanel;
                        }
                });
            });
        }
    }
}
