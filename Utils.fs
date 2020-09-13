module Utils

let numberedInst<'t>(i: int) =
    let cases = Reflection.FSharpType.GetUnionCases(typeof<'t>)
    let case = cases.[i]

    Reflection.FSharpValue.MakeUnion(case, [||]) :?> 't

let randInst<'t>() = 
    let cases = Reflection.FSharpType.GetUnionCases(typeof<'t>)
    let index = System.Random().Next(cases.Length)
    let case = cases.[index]
    
    Reflection.FSharpValue.MakeUnion(case, [||]) :?> 't