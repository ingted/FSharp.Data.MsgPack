let referenceBinaries = [ "FSharp.Data.MsgPack.dll" ]
let website = "/FSharp.Data.MsgPack"
let info =
  [ "project-name", "Yet another MessagePack implemented in F#"
    "project-author", "pocketberserker"
    "project-summary", "Yet another MessagePack implemented in F#"
    "project-github", "https://github.com/pocketberserker/FSharp.Data.MsgPack"
    "project-nuget", ""]

#I "../../packages/FSharp.Formatting.2.4.1/lib/net40"
#I "../../packages/RazorEngine.3.3.0/lib/net40"
#I "../../packages/FSharp.Compiler.Service.0.0.36/lib/net40"
#r "RazorEngine.dll"
#r "FSharp.Compiler.Service.dll"
#r "FSharp.Literate.dll"
#r "FSharp.CodeFormat.dll"
#r "FSharp.MetadataFormat.dll"

open System.IO
open FSharp.Literate
open FSharp.MetadataFormat

let (@@) path1 path2 =
    Path.Combine(path1, path2)

#if RELEASE
let root = website
#else
let root = "file://" + (__SOURCE_DIRECTORY__ @@ "../output")
#endif

let bin         = __SOURCE_DIRECTORY__ @@ "../../bin"
let content     = __SOURCE_DIRECTORY__ @@ "../content"
let output      = __SOURCE_DIRECTORY__ @@ "../output"
let files       = __SOURCE_DIRECTORY__ @@ "../files"
let templates   = __SOURCE_DIRECTORY__ @@ "templates"
let formatting  = __SOURCE_DIRECTORY__ @@ "../../packages/FSharp.Formatting.2.4.1/"
let docTemplate = formatting @@ "templates/docpage.cshtml"

let layoutRoots =
  [ templates
    formatting @@ "templates"
    formatting @@ "templates/reference" ]

let buildDocumentation () =
    let subdirs = Directory.EnumerateDirectories(content, "*", SearchOption.AllDirectories)
    for dir in Seq.append [content] subdirs do
        let sub = if dir.Length > content.Length then dir.Substring(content.Length + 1) else "."
        Literate.ProcessDirectory
            ( dir, docTemplate, output @@ sub, replacements = ("root", root)::info,
              layoutRoots = layoutRoots )

buildDocumentation()
