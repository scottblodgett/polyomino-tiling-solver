namespace CheckerBoard
{
    public class Pieces
    {
        public int Name { get; set; }
        public int[] Orientation { get; set; }
        public int[] BorW { get; set; }
    }

    public class PiecesDBModel
    {
        public int PieceNo { get; set; }
        public int Num1 { get; set; }
        public int Num2 { get; set; }
        public int Num3 { get; set; }
        public int Num4 { get; set; }
        public int Num5 { get; set; }
        public int Num6 { get; set; }
        public int Num7 { get; set; }
        public int BW1 { get; set; }
        public int BW2{ get; set; }
        public int BW3 { get; set; }
        public int BW4 { get; set; }
        public int BW5 { get; set; }
        public int BW6 { get; set; }
        public int BW7 { get; set; }
    }
}
