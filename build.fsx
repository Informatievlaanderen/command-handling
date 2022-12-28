#r "paket:
version 7.0.2-rc001
framework: net6.0
source https://api.nuget.org/v3/index.json
nuget Be.Vlaanderen.Basisregisters.Build.Pipeline 6.0.3 //"

#load "packages/Be.Vlaanderen.Basisregisters.Build.Pipeline/Content/build-generic.fsx"

open Fake.Core
open Fake.Core.TargetOperators
open Fake.IO.FileSystemOperators
open ``Build-generic``

let assemblyVersionNumber = (sprintf "%s.0")
let nugetVersionNumber = (sprintf "%s")

let buildSource = build assemblyVersionNumber
let buildTest = buildTest assemblyVersionNumber
let publishSource = publish assemblyVersionNumber
let pack = packSolution nugetVersionNumber

supportedRuntimeIdentifiers <- [ "linux-x64" ]

// Library ------------------------------------------------------------------------
Target.create "Lib_Build" (fun _ ->
  buildSource "Be.Vlaanderen.Basisregisters.AggregateSource"
  buildSource "Be.Vlaanderen.Basisregisters.AggregateSource.ExplicitRouting"
  buildSource "Be.Vlaanderen.Basisregisters.AggregateSource.SqlStreamStore"
  buildSource "Be.Vlaanderen.Basisregisters.AggregateSource.SqlStreamStore.Autofac"
  buildSource "Be.Vlaanderen.Basisregisters.AggregateSource.SqlStreamStore.Microsoft"
  buildSource "Be.Vlaanderen.Basisregisters.AggregateSource.Testing"
  buildSource "Be.Vlaanderen.Basisregisters.AggregateSource.Testing.CommandHandling"
  buildSource "Be.Vlaanderen.Basisregisters.AggregateSource.Testing.NUnit"
  buildSource "Be.Vlaanderen.Basisregisters.AggregateSource.Testing.SqlStreamStore"
  buildSource "Be.Vlaanderen.Basisregisters.AggregateSource.Testing.SqlStreamStore.Autofac"
  buildSource "Be.Vlaanderen.Basisregisters.AggregateSource.Testing.SqlStreamStore.Microsoft"
  buildSource "Be.Vlaanderen.Basisregisters.AggregateSource.Testing.Xunit"
  buildSource "Be.Vlaanderen.Basisregisters.CommandHandling"
  buildSource "Be.Vlaanderen.Basisregisters.CommandHandling.Idempotency"
  buildSource "Be.Vlaanderen.Basisregisters.CommandHandling.Idempotency.Microsoft"
  buildSource "Be.Vlaanderen.Basisregisters.CommandHandling.SqlStreamStore"
  buildTest "Be.Vlaanderen.Basisregisters.AggregateSource.Tests"
  buildTest "Be.Vlaanderen.Basisregisters.AggregateSource.SqlStreamStore.Tests"
  buildTest "Be.Vlaanderen.Basisregisters.AggregateSource.Testing.Tests"
  buildTest "Be.Vlaanderen.Basisregisters.CommandHandling.Tests"
)

Target.create "Lib_Test" (fun _ ->
  [
    "test" @@ "Be.Vlaanderen.Basisregisters.AggregateSource.Tests"
    "test" @@ "Be.Vlaanderen.Basisregisters.AggregateSource.SqlStreamStore.Tests"
    "test" @@ "Be.Vlaanderen.Basisregisters.AggregateSource.Testing.Tests"
    "test" @@ "Be.Vlaanderen.Basisregisters.CommandHandling.Tests"
  ] |> List.iter testWithDotNet
)

Target.create "Lib_Publish" (fun _ ->
  publishSource "Be.Vlaanderen.Basisregisters.AggregateSource"
  publishSource "Be.Vlaanderen.Basisregisters.AggregateSource.ExplicitRouting"
  publishSource "Be.Vlaanderen.Basisregisters.AggregateSource.SqlStreamStore"
  publishSource "Be.Vlaanderen.Basisregisters.AggregateSource.SqlStreamStore.Autofac"
  publishSource "Be.Vlaanderen.Basisregisters.AggregateSource.SqlStreamStore.Microsoft"
  publishSource "Be.Vlaanderen.Basisregisters.AggregateSource.Testing"
  publishSource "Be.Vlaanderen.Basisregisters.AggregateSource.Testing.CommandHandling"
  publishSource "Be.Vlaanderen.Basisregisters.AggregateSource.Testing.NUnit"
  publishSource "Be.Vlaanderen.Basisregisters.AggregateSource.Testing.SqlStreamStore"
  publishSource "Be.Vlaanderen.Basisregisters.AggregateSource.Testing.SqlStreamStore.Autofac"
  publishSource "Be.Vlaanderen.Basisregisters.AggregateSource.Testing.SqlStreamStore.Microsoft"
  publishSource "Be.Vlaanderen.Basisregisters.AggregateSource.Testing.Xunit"
  publishSource "Be.Vlaanderen.Basisregisters.CommandHandling"
  publishSource "Be.Vlaanderen.Basisregisters.CommandHandling.Idempotency"
  publishSource "Be.Vlaanderen.Basisregisters.CommandHandling.Idempotency.Microsoft"
  publishSource "Be.Vlaanderen.Basisregisters.CommandHandling.SqlStreamStore"
)

Target.create "Lib_Pack" (fun _ -> pack "Be.Vlaanderen.Basisregisters.CommandHandling")

// --------------------------------------------------------------------------------
Target.create "PublishAll" ignore
Target.create "PackageAll" ignore

// Publish ends up with artifacts in the build folder
"DotNetCli"
==> "Clean"
==> "Restore"
==> "Lib_Build"
==> "Lib_Test"
==> "Lib_Publish"
==> "PublishAll"

// Package ends up with local NuGet packages
"PublishAll"
==> "Lib_Pack"
==> "PackageAll"

Target.runOrDefault "Lib_Test"
