module Sudoku
open System

    type SudokuBoard = int option[,]

    let getRowByFieldNo (board: 'a[,]) (rowNo : int) = board.[rowNo,*]
    let getColByFieldNo (board: 'a[,]) (colNo : int) = board.[*,colNo]
    
    let getFieldsInSquare (board: 'a[,]) (squareNo : int) = 
        let firstCol = (((squareNo - 1) * 3 ) + 1) % 9;
        let firstRow = ((squareNo - 1) / 3) * 3;
        let fields = seq {
            for row in firstRow .. firstRow + 2 do
                for col in firstCol .. firstCol + 2 ->
                    (row, col - 1)
        }
        fields |> Seq.map(fun x -> board.[fst x, snd x])
    
    let getSquareNoByFieldNo rowNo colnNo =
        let rowModifier = rowNo/3
        let colModifier = colnNo/3
        rowModifier * 3 + colModifier + 1

    let getSquareByFieldNo (board: 'a[,]) (rowNo : int) (colNo : int) =
        getFieldsInSquare board  <| getSquareNoByFieldNo rowNo colNo

    let areFieldsDistinct (fields : seq<int option>) = 
        fields
        |> Seq.countBy id
        |> Seq.fold(fun acc item -> 
            let elem, count = fst item, snd item
            (elem.IsNone || count < 2) && acc
            ) true

    let find2D item (arr: 'a[,]) = Seq.tryPick id <| seq {
        for i in 0..(arr.GetLength 0 - 1) do
            for j in 0..(arr.GetLength 1 - 1) do
                if arr.[i,j] = item 
                    then yield Some (i,j) 
                    else yield None
    }

    let isCellValid sudokuBoard rowNo collNo = 
        getRowByFieldNo sudokuBoard rowNo |> areFieldsDistinct &&
        getColByFieldNo sudokuBoard collNo |> areFieldsDistinct &&
        getSquareByFieldNo sudokuBoard rowNo collNo |> areFieldsDistinct

    let mapToSudokuField (a : int) =
        match a with
            | x when x > 0 && x < 10 -> Some a
            | 0 -> None
            | _ -> raise (ArgumentOutOfRangeException "Filed value out of range")

    // Solve sudoku using backtracking
    let rec trySolveBoard (board: SudokuBoard) : SudokuBoard option =
        //exit condition - all fields set
        let emptyFieldCoordinates = find2D None board
        if (emptyFieldCoordinates.IsNone) then
            Some board

        else
            let row, col = fst emptyFieldCoordinates.Value, snd emptyFieldCoordinates.Value
            [1..9] 
            |> Seq.map (fun value -> 
                board.SetValue(mapToSudokuField value, row, col)           
                match (isCellValid board row col && trySolveBoard board |> Option.isSome) with
                | true  -> Some board
                | false -> 
                    board.SetValue(None, row, col)
                    None)
            |> Seq.tryPick id        

    let rec tryGetSolvedBoard (board: SudokuBoard) : SudokuBoard option =
        let copied = Array2D.copy(board);
        trySolveBoard copied            