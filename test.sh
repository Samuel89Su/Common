rm -r ./test/TestResults/*

dotnet test -c:Release -r:"test/TestResults" --collect:"XPlat Code Coverage"

for report in $(find ./test/TestResults -name "coverage.cobertura.xml");
do
  dotnet ./test/reportgenerator/4.8.0/tools/net5.0/ReportGenerator.dll "-reports:$report" "-targetdir:test/coveragereport" -reporttypes:Html "-historydir:test/coveragereport/history"
done
