.\build-bin\nuget.exe pack .\postmark.nuspec

:: "strong" build commented out below

:: mkdir sn-build
:: cd sn-build
:: mkdir PCL
:: mkdir Convenience
:: cd ..

:: build-bin\ILRepack.exe /internalize /keyfile:src\Postmark.PCL\key.snk src\Postmark.PCL\bin\Release\Postmark.dll src\Postmark.PCL\bin\Release\Newtonsoft.Json.dll /out:sn-build\PCL\Postmark.dll 

:: build-bin\ILRepack.exe /internalize src\Postmark.Convenience\bin\Release\Postmark.dll src\Postmark.Convenience\bin\Release\Newtonsoft.Json.dll /out:Postmark.dll
:: build-bin\ILRepack.exe /keyfile:src\Postmark.PCL\key.snk Postmark.dll src\Postmark.Convenience\bin\Release\Postmark.Convenience.dll /out:sn-build\Convenience\Postmark.dll

:: ./build-bin/nuget.exe pack ./postmark-strong.nuspec