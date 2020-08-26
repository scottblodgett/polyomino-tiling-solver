using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SQLite;
using System.Linq;

namespace CheckerBoard
{
    public class Results
    {
        public int[] Board;
        public int[] BWBoard;
        public int id;

        public Results(int[] board, int[] bWBoard, int id)
        {
            Board = board;
            BWBoard = bWBoard;
            this.id = id;
        }
    }

    public class BoardModel
    {
        public BoardModel(int id, int x, int y, int val)
        {
            this.id = id;
            this.x = x;
            this.y = y;
            this.val = val;
        }

        public int id { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int val { get; set; }
    }

    public class NewBoardModel
    {
        public int id { get; set; }
        public string val { get; set; }

        public int pieceNo { get; set; }

        public int position { get; set; }

        public NewBoardModel(int id, string val)
        {
            this.id = id;
            this.val = val;
        }

        public NewBoardModel(int id, string val, int pieceNo, int position)
        {
            this.id = id;
            this.val = val;
            this.pieceNo = pieceNo;
            this.position = position;
        }
    }

    public class DebugModel
    {
        public string msg { get; set; }

        public DebugModel(string msg)
        {
            this.msg = msg;
        }
    }

    public class SqliteDataAccess
    {
        public static void SaveBoard(List<Results> listOfResults)
        {

            foreach (var result in listOfResults)
            {
                SaveBoard(result.Board, result.id, 0, 0);
                SaveBoard(result.BWBoard, -result.id, 0, 0);
            }
        }

        public static void SaveBoard(List<int[]> myBoards, int inOrOut)
        {
            SaveBoard(myBoards, inOrOut, -1, -1);
        }

        public static void SaveBoard(List<int[]> myBoards, int inOrOut, int pieceNo, int position)
        {
            using (var cnn = new SQLiteConnection(LoadConnectionString()))
            {
                for (var i = 0; i < myBoards.Count; i++)
                {

                    var currentBoard = myBoards[i];
                    var currentBoardAsString = string.Join(",", currentBoard);
                    var activeBoard = new NewBoardModel(inOrOut, currentBoardAsString, pieceNo, position);

                    try
                    {
                        cnn.Execute("insert into boardsOfString (id, val, pieceNo, position) values (@id, @val, @pieceNo, @position)", activeBoard);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
            }
        }

        public static void SaveBoard(int[] myBoard, int inOrOut)
        {
            var myBoards = new List<int[]>();
            myBoards.Add(myBoard);
            SaveBoard(myBoards, inOrOut);
        }

        public static void SaveBoard(int[] myBoard, int inOrOut, int pieceNo, int position)
        {
            var myBoards = new List<int[]>();
            myBoards.Add(myBoard);
            SaveBoard(myBoards, inOrOut, pieceNo, position);
        }

        public static void SaveBoard(List<NewBoardModel> listOfDebugBoards)
        {
            var results = new List<int>();
            string sqlInsert = @"insert into boardsOfString (id, val, pieceNo, position) values (@id, @val, @pieceNo, @position);";
            using (var conn = new SQLiteConnection(LoadConnectionString()))
            {
                {
                    conn.Open();
                    using (var transaction = conn.BeginTransaction())
                    {
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = sqlInsert;
                            cmd.Parameters.AddWithValue("@id", "id");
                            cmd.Parameters.AddWithValue("@val", "val");
                            cmd.Parameters.AddWithValue("@pieceNo", "pieceNo");
                            cmd.Parameters.AddWithValue("@position", "position");

                            foreach (var activeBoard in listOfDebugBoards)
                            {
                                cmd.Parameters["@id"].Value = activeBoard.id;
                                cmd.Parameters["@val"].Value = activeBoard.val;
                                cmd.Parameters["@pieceNo"].Value = activeBoard.pieceNo;
                                cmd.Parameters["@position"].Value = activeBoard.position;
                                results.Add(cmd.ExecuteNonQuery());
                            }
                        }
                        transaction.Commit();
                    }
                }
                conn.Close();
            }
        }

        private static string LoadConnectionString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }

        public static Pieces[] getPieces()
        {
            var piecesList = new List<Pieces>();
            string sqlQuery = "select PieceNo, Num1, Num2, Num3, Num4, Num5, Num6, Num7, BW1, BW2, BW3, BW4, BW5, BW6, BW7 from pieces";
            using (var conn = new SQLiteConnection(LoadConnectionString()))
            {
                var pieces = conn.Query<PiecesDBModel>(sqlQuery);

                foreach (var piece in pieces)
                {
                    var currentPiece = new Pieces();
                    currentPiece.Name = piece.PieceNo;
                    currentPiece.Orientation = new int[7];
                    currentPiece.Orientation[0] = piece.Num1;
                    currentPiece.Orientation[1] = piece.Num2;
                    currentPiece.Orientation[2] = piece.Num3;
                    currentPiece.Orientation[3] = piece.Num4;
                    currentPiece.Orientation[4] = piece.Num5;
                    currentPiece.Orientation[5] = piece.Num6;
                    currentPiece.Orientation[6] = piece.Num7;
                    currentPiece.BorW = new int[7];
                    currentPiece.BorW[0] = piece.BW1;
                    currentPiece.BorW[1] = piece.BW2;
                    currentPiece.BorW[2] = piece.BW3;
                    currentPiece.BorW[3] = piece.BW4;
                    currentPiece.BorW[4] = piece.BW5;
                    currentPiece.BorW[5] = piece.BW6;
                    currentPiece.BorW[6] = piece.BW7;
                    piecesList.Add(currentPiece);
                }
            }
            return piecesList.ToArray();
        }

        public static string getBoard(int boardId)
        {
            IEnumerable<string> board;
            string sqlQuery = String.Format($"select val from boardsOfString where id = {boardId}");

            using (var conn = new SQLiteConnection(LoadConnectionString()))
            {
                board = conn.Query<string>(sqlQuery);
            }
            return board.First();
        }

        public static void clearDB()
        {
            string sqlQuery = String.Format($"DELETE FROM boardsOfString");
            using (var conn = new SQLiteConnection(LoadConnectionString()))
            {
                var affectedRows = conn.Execute(sqlQuery);
            }
        }
    }
}
