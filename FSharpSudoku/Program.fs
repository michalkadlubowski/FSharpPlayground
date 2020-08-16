open System
open Sudoku

[<EntryPoint>]
let main _ =
   // let board  = array2D [
   //    [0;0;0; 2;6;0; 7;0;1];
   //    [6;8;0; 0;7;0; 0;9;0];
   //    [1;9;0; 0;0;4; 5;0;0];
                     
   //    [8;2;0; 1;0;0; 0;4;0];
   //    [0;0;4; 6;0;2; 9;0;0];
   //    [0;5;0; 0;0;3; 0;2;8];
                     
   //    [0;0;9; 3;0;0; 0;7;4];
   //    [0;4;0; 0;5;0; 0;3;6];
   //    [7;0;3; 0;1;8; 0;0;0];
   // ]

   let board  = array2D [
      [0;2;0; 0;0;0; 0;0;0];
      [0;0;0; 6;0;0; 0;0;3];
      [0;7;4; 0;8;0; 0;0;0];
                     
      [0;0;0; 0;0;3; 0;0;2];
      [0;8;0; 0;4;0; 0;1;0];
      [6;0;0; 5;0;0; 0;0;0];
                     
      [0;0;0; 0;1;0; 7;8;0];
      [5;0;0; 0;0;9; 0;0;0];
      [0;0;0; 0;0;0; 0;4;0];
   ]   

   let printBoard (array2d: SudokuBoard) =
      for x in 0..8 do
         for y in 0..8 do
            let field = array2d.[x, y];
            let print = if field.IsNone then "X" else field.Value.ToString()
            Console.Write(print)
            Console.Write(" ")
         Console.Write(Environment.NewLine);  

   let fieldBoard = board |> Array2D.map(mapToSudokuField)

   let result = Sudoku.tryGetSolvedBoard fieldBoard
   if result.IsSome then
      printBoard result.Value
   else
     printf "No solution"
   0

