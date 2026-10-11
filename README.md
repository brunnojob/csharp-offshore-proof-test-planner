# csharp-offshore-proof-test-planner

[View execution evidence](https://brunnojob.github.io/devstart-lab/proofs/csharp-offshore-proof-test-planner/)

Prioritize overdue protective-device proof tests from failure-rate and test-coverage assumptions.

```sh
dotnet run -- --self-test
dotnet run < examples/input.txt
```

Offline engineering analysis. Installation-specific limits and source records determine the result; this program does not operate equipment.

MIT. Built by [brunnodev](https://brunnodev.store).

## Execution proof

[![Executable proof](https://github.com/brunnojob/csharp-offshore-proof-test-planner/actions/workflows/proof.yml/badge.svg)](https://github.com/brunnojob/csharp-offshore-proof-test-planner/actions/workflows/proof.yml)

![Recorded program execution](docs/proof/execution.svg)

[Verified run](https://github.com/brunnojob/csharp-offshore-proof-test-planner/actions/runs/38018091874) · [Execution report](docs/proof/evidence.json)

Run `python .proof/record.py` after installing the prerequisites above. The scenarios execute repository code and verify exit codes and expected output. CI publishes `execution-proof` with the transcript, input fingerprints and source commit. The downloadable report identifies the exact tested version; the workflow badge tracks the latest run.

## Co-authorship

Contribution trailer:

```text
Co-authored-by: nyctophile <33561761+ineedfoundmyway@users.noreply.github.com>
```
