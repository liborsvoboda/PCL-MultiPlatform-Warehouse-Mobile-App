ECHO OFF
REM E:
REM https://developers.google.com/identity/protocols/oauth2/native-app
REM cd E:\Projekty\Expedis\TerminalApp\Terminal\Terminal.Android
"E:\Program Files (x86)\Microsoft Visual Studio\2019\Enterprise\MSBuild\Current\Bin\msbuild.exe" -restore E:\Projekty\Bohemian Technology\HandOver\HandOver\HandOver\HandOver.Android\Terminal.Android.csproj ^
  -t:SignAndroidPackage ^
  -p:Configuration=Ad-Hoc ^
  -p:AndroidKeyStore=False ^
  -p:AndroidSigningKeyStore=TerminalKeys.keystore^
  -p:AndroidSigningStorePass=Terminal
Exit