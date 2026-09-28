# Polyomino Tiling Solver

An exhaustive backtracking solver for a constrained tiling problem: fit twelve
irregular polyomino pieces onto an 8x8 board so that the board is completely
covered **and** the finished board is a valid checkerboard.

The second half of that sentence is the whole problem. Covering 64 squares with
twelve pieces is a packing exercise. Requiring that the result also alternate
colors correctly, while each piece carries its own fixed color pattern that
rotates with it, prunes the search space to something much smaller and much less
obvious.

**Result: 66 distinct solutions.** All 66 are stored in `CheckerBoard/checkers.db`
and every one is a complete 64-square tiling using each piece exactly once.

```
 1  1  1  1  1  6  6  6        one of the 66, pieces numbered 1-12
 1  1  2  2  2  2  2  6
 9  9  2 12  8  8  8  6
 9  9  5 12  8 10 10  6
 7  9  5 12  8 11 10 10
 7  5  5  4  4 11 11 10
 7  5  5  3  4  4  4  4
 7  7  7  3  3  3  3  3
```

## The pieces

Twelve pieces, sized 7, 6, 6, 6, 6, 6, 6, 5, 5, 5, 3, 3. That sums to exactly 64,
so a solution leaves no holes and has no spare pieces. Each piece is stored in
four rotations, except the one with rotational symmetry, which has two. **46 rows
in total** in the `pieces` table.

A piece is not just a shape. Each of its tiles carries a black-or-white value
that travels with the rotation, so the same shape in two orientations is two
genuinely different pieces as far as the constraint is concerned.

## How the board is modelled

The board is a flat `int[64]`, not a 2D array, and a piece is a list of **offsets**
from its own first tile. Placing a piece is `board[square + offset[i] - offset[0]]`,
which makes placement a handful of additions rather than any coordinate
arithmetic.

Two parallel boards are maintained:

| | |
| --- | --- |
| `board` | which piece occupies each square, or `-3` for empty |
| `bwboard` | the color of each occupied square |

Keeping color in its own array is what makes the parity check cheap: it is a
lookup, not a recomputation from the piece's geometry.

## The constraint check

`putPiece` (`CheckerBoard/CBoard.cs`) rejects a candidate placement on four grounds,
cheapest test first:

1. the anchor square is already occupied
2. any square the piece needs is already occupied
3. the piece would run off the right edge. A flat array has no natural edge, so
   wrapping from column 7 to column 0 of the next row is legal arithmetic and
   illegal geometry. The right-hand column is enumerated explicitly and a
   placement whose next tile is `current + 1` from there is rejected
4. any of the four orthogonal neighbors of a tile already carries the **same**
   color. This is the checkerboard rule, and it is checked against `bwboard`
   rather than re-derived

Number 3 is the bug that a flat-array board invites and it is worth reading if
you have ever written one.

## The search

`playNoUX` is plain recursive backtracking over the lowest-numbered empty square:
try each unused piece in each orientation, recurse, and on return undo the
placement. Two details matter.

The recursion always fills the **first** empty square rather than choosing one.
That is what makes the enumeration exhaustive without needing a visited set: every
solution is reached by exactly one path, so the 66 are 66 and not 66 permutations
of fewer.

The search does not stop at the first solution. It runs to exhaustion, appending a
cloned copy of both boards to a result list each time all twelve pieces are down.

There is a second, parallel implementation that drives the UI and repaints after
each placement so you can watch it work. It is deliberately separate: the animated
version is for understanding the algorithm, and the silent version is for getting
the answer.

## Persistence and output

- **SQLite via Dapper.** The piece definitions live in a table rather than a
  source-code array, so the piece set can be changed without a rebuild. Solutions
  are written back to the same database as comma-joined strings.
- **Excel export via EPPlus.** Every solution is written to a worksheet as an 8x8
  block of cells, each cell filled with its piece's color, so all 66 can be read
  at a glance on one sheet.

## Running it

Windows Forms on **.NET Framework 4.6.1**, `packages.config`-style NuGet.
Open `CheckerBoard.sln` in Visual Studio, restore packages, and run.

| Control | What it does |
| --- | --- |
| **Go** | full enumeration, then paints the first solution and writes the Excel file |
| **>>** / **<<** | step through the 66 stored solutions |
| **Another One** | drop the piece chosen in the left dropdown at the square chosen in the right one, or pick either at random. This is the one to use when you want to see exactly which rule rejected a placement |
| **Place All** | walk every piece in turn, a second apart, reporting which ones fit at the chosen square |
| **Pieces File** | paint each of the 46 orientations in turn |
| **Super** | paint an arbitrary board pasted into the two text boxes, as comma-separated piece ids and colors. This is how a partial or hand-built position gets rendered |
| **Place Many** | a fixed sequence of placements, kept as a hand-checked fixture |
| **Clear** | reset the board |

Dependencies: Dapper 2.0.35, EPPlus 4.5.3.2, System.Data.SQLite 1.0.113.1.

## Known rough edges

Stated rather than hidden, because they are the honest state of it.

- The Excel output path is hardcoded to `c:\temp\myworkbook.xlsx` and the run will
  report an exception in the UI rather than prompting if that directory is missing
- `App.config` names the provider `System.Data.SqlClinet`, which is both a typo and
  the wrong provider. Nothing reads it; Dapper is handed a `SQLiteConnection`
  directly, and the connection string beside it is the part that matters
- The solution count 66 is hardcoded in the **>>** and **<<** handlers rather than
  read from the database, so changing the piece set breaks the browser even though
  it would not break the solver
- The solver passes state by `ref` through a long parameter list. It works and it is
  explicit about what mutates, but it is the part most worth extracting into a
  solver class
- The button labels are archaeology. **Place Many** runs a fixed fixture and
  **Another One** is the general case, which is backwards from how they read, and
  **Pieces File** no longer writes a file because the export call under it is
  commented out. The names record the order the buttons were added rather than what
  they now do
